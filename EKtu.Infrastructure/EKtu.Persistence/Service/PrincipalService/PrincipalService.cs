using EKtu.Domain.Entities;
using EKtu.Infrastructure.HASH;
using EKtu.Persistence.Builder.IBuilder;
using EKtu.Repository.Dtos;
using EKtu.Repository.IRepository;
using EKtu.Repository.IRepository.PrincipalRepository;
using EKtu.Repository.IService;
using EKtu.Repository.IService.PrincipalService;
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
