﻿using EKtu.Domain.Entities;
using EKtu.Persistence.Repositorys;
using EKtu.Repository.Dtos;
using EKtu.Repository.IRepository.PrincipalRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Repository.PrincipalRepository
{
    public class PrincipalRepository : BaseRepository<Principal>, IPrincipalRepository
    {
        public PrincipalRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task AddLessonsAsync(Lesson lesson)
        {
           await _dbContext.Lesson.AddAsync(lesson);
        }

        public Task<IQueryable<ClassExamNoteAverageResponseDto>> AllStudentCalculateLetterGrandeAsync()
        {

         return Task.FromResult(_dbContext.LessonConfirmation.AsNoTracking()
                .Include(y => y.Student)
                .Include(y => y.ExamNote)
                .GroupBy(y => new
                {
                    ClassId=y.Student.ClassId,
                    LessonId=y.LessonId
                }).Select(group =>new ClassExamNoteAverageResponseDto()
                {
                    ClassId=group.Key.ClassId,
                    LessonId=group.Key.LessonId,
                    Avg= group.Sum(y=>y.ExamNote.Exam1+y.ExamNote.Exam2)/(group.Count()*2)
                }));


        }

        public Task<IQueryable<Lesson>> GetAllLessonAsync()
        {
            return Task.FromResult(_dbContext.Lesson.AsQueryable());
        }

        public async Task<int> GetStudentChooseLessonIdAsync(int studentId, int lessonId)
        {
          return (await _dbContext.StudentChooseLessons.FirstOrDefaultAsync(y => y.StudentId==studentId && y.LessonId == lessonId))!.Id;
        }

        public async ValueTask<Principal> PrincipalInformation(int userId)
        {
           return await _dbContext.Principal.FindAsync(userId);
        }

        public async Task StudentAttendanceAddAsync(Attendance attendance)
        {
          await  _dbContext.Attendances.AddAsync(attendance);
        }

        public Task<IQueryable<LessonConfirmation>> StudentCalculateLetterGrandeAsync(int classId,int lessonId)
        {
           return Task.FromResult(_dbContext.LessonConfirmation.Where(y => y.LessonId == lessonId).Include(y => y.Student)
                .Where(y => y.Student.ClassId == classId)
                .Include(y => y.ExamNote).AsQueryable());
        }

        public async Task StudentCalculateUpdatedAsync(List<ExamNote> studentCalculateExamLetterResponseDtos)
        {
            foreach (var examNote in studentCalculateExamLetterResponseDtos)
            {
                ExamNote examNotes= await _dbContext.ExamNote.FirstOrDefaultAsync(y => y.Id == examNote.Id);

                examNotes.LetterGrade=examNote.LetterGrade;
            }
        }

        public async Task StudentLessonApproveAsync()
        {
            List<LessonConfirmation> StudentExamGrandeAdd = await _dbContext.StudentChooseLessons.Select(y => new LessonConfirmation()
            {
                StudentId=y.StudentId,
                LessonId=y.LessonId
            }).ToListAsync();
           await _dbContext.LessonConfirmation.AddRangeAsync(StudentExamGrandeAdd);
        }

        public async Task TeacherClassLessonAsync(TeacherClassLesson teacherClassLesson)
        {
           await _dbContext.TeacherClassLesson.AddAsync(teacherClassLesson);
        }
    }
}
