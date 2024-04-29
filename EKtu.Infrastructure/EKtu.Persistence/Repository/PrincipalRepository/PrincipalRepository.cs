using EKtu.Domain.Entities;
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

        public Task<IQueryable<StudentChooseLesson>> StudentCalculateLetterGrandeAsync(int classId,int lessonId)
        {
           return Task.FromResult(_dbContext.StudentChooseLessons.Where(y => y.LessonId == lessonId).Include(y => y.Student)
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
            List<ExamNote> StudentExamGrandeAdd = await _dbContext.StudentChooseLessons.Select(y => new ExamNote()
            {
                StudentChooseLessonId = y.Id,
                Exam1 = 0,
                Exam2 = 0,
            }).ToListAsync();
           await _dbContext.ExamNote.AddRangeAsync(StudentExamGrandeAdd);
        }

        public async Task TeacherClassLessonAsync(TeacherClassLesson teacherClassLesson)
        {
           await _dbContext.TeacherClassLesson.AddAsync(teacherClassLesson);
        }
    }
}
