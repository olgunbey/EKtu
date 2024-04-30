using EKtu.Domain.Entities;
using EKtu.Infrastructure.HASH;
using EKtu.Persistence.Builder.IBuilder;
using EKtu.Repository.Dtos;
using EKtu.Repository.IRepository;
using EKtu.Repository.IRepository.PrincipalRepository;
using EKtu.Repository.IService;
using EKtu.Repository.IService.PrincipalService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Service.PrincipalService
{
    public class PrincipalService : BaseService<Principal>, IPrincipalService
    {
        private readonly IPrincipalRepository principalRepository;
        private readonly ISaves _saves;
        private readonly IPrincipalBuilder principalBuilder;
        private readonly ILessonBuilder lessonBuilder;
        private readonly ITeacherClassLessonBuilder teacherClassLessonBuilder;

        public PrincipalService(IBaseRepository<Principal> baseRepository, ISaves saves, IPrincipalRepository principalRepository, IPrincipalBuilder principalBuilder, ILessonBuilder lessonBuilder, ITeacherClassLessonBuilder teacherClassLessonBuilder = null) : base(baseRepository, saves)
        {
            this.principalRepository = principalRepository;
            this._saves = saves;
            this.principalBuilder = principalBuilder;
            this.lessonBuilder = lessonBuilder;
            this.teacherClassLessonBuilder = teacherClassLessonBuilder;
        }

        public async Task<Response<NoContent>> AddLessonAsync(AddLessonRequestDto addLessonRequestDto)
        {
          Lesson lesson=  this.lessonBuilder
                .LessonName(addLessonRequestDto.LessonName)
                .OptionalLessonId(addLessonRequestDto.OptionalLessonId)
                .HasOptional(addLessonRequestDto.HasOptional)
                .Grade(addLessonRequestDto.Grade)
                .Term(addLessonRequestDto.Term)
                .GetLesson();

            try
            {
                await principalRepository.AddLessonsAsync(lesson);
                await _saves.SaveChangesAsync();

                return Response<NoContent>.Success(204);
            }
            catch (Exception)
            {
                return Response<NoContent>.Fail("ders eklenemedi", 400);

                throw;
            }
           
        }

        public async Task<Response<NoContent>> AddTeacherClassLessonAsync(AddTeacherClassLessonRequestDto addTeacherClassLessonRequestDto)
        {
         TeacherClassLesson teacherClassLesson=   teacherClassLessonBuilder.TeacherId(addTeacherClassLessonRequestDto.TeacherId)
                .LessonId(addTeacherClassLessonRequestDto.LessonId)
                .ClassId(addTeacherClassLessonRequestDto.ClassId).Get();
            try
            {
               await principalRepository.TeacherClassLessonAsync(teacherClassLesson);
                await _saves.SaveChangesAsync();
                return Response<NoContent>.Success(204);
            }
            catch (Exception)
            {
                return Response<NoContent>.Fail("hata öğretmen ders sınıf seçimi kaydedilemedi", 400);
            }
        }

        public async Task<Response<NoContent>> AllStudentCalculateLetterGrandeAsync()
        {
            var AvgClassLesson= await(await principalRepository.AllStudentCalculateLetterGrandeAsync()).ToListAsync(); //burada sınıfların derslere gore gruplanmıs ortalaması var

            foreach (var item in AvgClassLesson) //burada o sınıfa ait dersleri alan öğrencilerin examgrandesini hesaplayacagız
            {
             var data =  (await principalRepository.StudentCalculateLetterGrandeAsync(item.ClassId, item.LessonId)).ToList();

             var StudentGpas=  data.Select(y => new StudentCalculateExamLetterResponseDto()
                {
                    ExamNoteId = y.ExamNote.Id,
                    GradeAverage = (y.ExamNote.Exam1 + y.ExamNote.Exam2) / data.Count(),
                    LatterGrande = (y.ExamNote.Exam2 + y.ExamNote.Exam1) / data.Count() < item.Avg ? "FF" :
                    ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / data.Count()) == 0 ? "FF" :
                ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / data.Count()) > item.Avg + 20 && ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / data.Count()) <= item.Avg + 24 ? "AA" :
                ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / data.Count()) > item.Avg + 16 && ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / data.Count()) <= item.Avg + 20 ? "AB" :
                ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / data.Count()) > item.Avg + 12 && ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / data.Count()) <= item.Avg + 16 ? "BB" :
                ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / data.Count()) > item.Avg + 8 && ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / data.Count()) <= item.Avg + 12 ? "CB" :
                ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / data.Count()) > item.Avg + 4 && ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / data.Count()) <= item.Avg + 8 ? "CC" :
                ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / data.Count()) <= item.Avg + 4 ? "DC" :""
                
                }).ToList() ;


               var returnDatas= StudentGpas.Select(y => new ExamNote()
                {
                    Id = y.ExamNoteId,
                    LetterGrade = y.LatterGrande
                }).ToList();


                try
                {
                    await principalRepository.StudentCalculateUpdatedAsync(returnDatas);
                }
                catch (Exception)
                {

                    return Response<NoContent>.Fail("harf notları hesaplanamadı", 400);
                }
            }
            await _saves.SaveChangesAsync();
            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> StudentCalculateLetterGrandeAsync(StudentCalculateLetterGrandeDto studentCalculateLetterGrandeDto)
        {
            var StudentClassLessons = (await principalRepository.StudentCalculateLetterGrandeAsync(studentCalculateLetterGrandeDto.ClassId, studentCalculateLetterGrandeDto.LessonId));



            int ClassAvg= StudentClassLessons.Sum(y => (y.ExamNote.Exam1 + y.ExamNote.Exam2) / StudentClassLessons.Count())/StudentClassLessons.Count(); //sınıf ortalaması

            var StudentGpa = StudentClassLessons.Select(y => new StudentCalculateExamLetterResponseDto() //öğrencilerin not ortalaması 60 //sınıf not ortalaması 40 CB olması gerekiyor
            { 
                ExamNoteId=y.ExamNote.Id,
                GradeAverage=(y.ExamNote.Exam2+y.ExamNote.Exam1)/StudentClassLessons.Count(),
                LatterGrande=(y.ExamNote.Exam2+y.ExamNote.Exam1)/StudentClassLessons.Count()<ClassAvg ? "FF":
                ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / StudentClassLessons.Count()) > ClassAvg + 20 && ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / StudentClassLessons.Count()) <= ClassAvg + 24 ?"AA":
                ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / StudentClassLessons.Count()) > ClassAvg + 16 && ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / StudentClassLessons.Count()) <= ClassAvg + 20 ? "AB" :
                ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / StudentClassLessons.Count()) > ClassAvg + 12 && ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / StudentClassLessons.Count()) <= ClassAvg + 16 ? "BB" :
                ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / StudentClassLessons.Count()) > ClassAvg + 8 && ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / StudentClassLessons.Count()) <= ClassAvg + 12 ? "CB" :
                ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / StudentClassLessons.Count()) > ClassAvg + 4 && ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / StudentClassLessons.Count())<=ClassAvg+8 ? "CC" :
                ((y.ExamNote.Exam2 + y.ExamNote.Exam1) / StudentClassLessons.Count()) <= ClassAvg + 4 ? "DC" :
                "Hesaplanamadı"
            }).ToList();

            var examNotes= StudentGpa.Select(y => new ExamNote()
            {
                Id = y.ExamNoteId,
                LetterGrade = y.LatterGrande
            }).ToList();
            try
            {
                await principalRepository.StudentCalculateUpdatedAsync(examNotes);
                await _saves.SaveChangesAsync();

                return Response<NoContent>.Success(204);
            }
            catch (Exception)
            {
                return Response<NoContent>.Fail("harf notları hesaplanamadı", 400);
            }
           
        }

        public async Task<Response<NoContent>> StudentChooseApproveAsync()
        {
            try
            {
                await principalRepository.StudentLessonApproveAsync();
                await _saves.SaveChangesAsync();
                return Response<NoContent>.Success(204);
            }
            catch (Exception)
            {
                return Response<NoContent>.Fail("Dersler kaydedilmedi", 400);
            }
        }
    }
}
