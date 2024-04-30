using EKtu.Domain.Entities;
using EKtu.Infrastructure.HASH;
using EKtu.Repository.Dtos;
using EKtu.Repository.IRepository;
using EKtu.Repository.IRepository.TeacherRepository;
using EKtu.Repository.IService;
using EKtu.Repository.IService.TeacherService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Service.TeacherService
{
    public class TeacherService : BaseService<Teacher>, ITeacherService
    {
        private readonly IPasswordRepository<Teacher> passwordRepository;
        private readonly ITeacherRepository teacherRepository;
        public TeacherService(IBaseRepository<Teacher> baseRepository, ISaves saves, IPasswordRepository<Teacher> passwordRepository, ITeacherRepository teacherRepository) : base(baseRepository, saves)
        {
            this.passwordRepository = passwordRepository;
            this.teacherRepository = teacherRepository;
        }

        public async Task<Response<List<TeacherClassReponseDto>>> TeacherClass(int teacherId)
        {
            var teacherClasses = await (await teacherRepository.TeacherClass(teacherId)).ToListAsync();

            var responseDto= teacherClasses.DistinctBy(y=>y.TeacherId).Select(y => new TeacherClassReponseDto()
            {
              ClassId=y.ClassId,
              ClassName=y.Class.ClassName
            }).ToList();

            if(!responseDto.Any())
            {
              return Response<List<TeacherClassReponseDto>>.Fail("öğretmenin girdiği sınıf yok", 400);
            }
            return Response<List<TeacherClassReponseDto>>.Success(responseDto, 200);
        }

        public async Task<Response<List<TeacherLessonDto>>> TeacherLesson(int classId, int teacherId)
        {
          var teacherClassLessons=await teacherRepository.TeacherClassLesson(teacherId, classId);

          List<TeacherLessonDto> teacherLessonDtos= await teacherClassLessons.Select(y => new TeacherLessonDto()
            {
                LessonId=y.LessonId,
                LessonName=y.Lesson.LessonName
            }).ToListAsync();

            if(teacherLessonDtos.Any())
                return Response<List<TeacherLessonDto>>.Success(teacherLessonDtos, 200);

            return Response<List<TeacherLessonDto>>.Fail("öğretmenin bu sınıfa girdiği ders yok", 400);
        }
    }
}
