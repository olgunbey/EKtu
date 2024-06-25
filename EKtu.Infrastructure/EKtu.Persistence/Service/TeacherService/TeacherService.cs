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
using System.Text.Json;
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

        public Response<NoContent> EnteringStudentGrades(List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos, out List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos1)
        {

            teacherRepository.EnteringStudentGrades(enteringStudentGradesRequestDtos, out var UpdateExamDtos);
            enteringStudentGradesRequestDtos1 = UpdateExamDtos;
            try
            {
                _saves.SaveChanges();
               return Response<NoContent>.Success(204);
            }
            catch (Exception)
            {
                return Response<NoContent>.Fail("hata", 400);
            }
            
        }

        public async Task<Response<List<GetAllStudentByClassIdAndLessonIdResponseDto>>> GetAllStudentByClassIdAndLessonId(GetAllStudentByClassIdAndLessonIdRequestDto getAllStudentByClassIdAndLessonIdRequestDto)
        {
            var respo =(await teacherRepository.GetAllStudentByClassIdAndLessonIdAsync(getAllStudentByClassIdAndLessonIdRequestDto)).ToList();


          var responseData= respo.Select(y => new GetAllStudentByClassIdAndLessonIdResponseDto()
            {
                StudentId=y.StudentId,
                StudentName=y.Student.FirstName+""+y.Student.LastName,
                Exam_1=y.ExamNote.Exam1,
                Exam_2=y.ExamNote.Exam2,
            }).ToList();

            return Response<List<GetAllStudentByClassIdAndLessonIdResponseDto>>.Success(responseData, 200);

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

        public async Task<Response<TeacherInformationDto>> TeacherInformation(int userId)
        {
         var existTeacher= await teacherRepository.TeacherInformation(userId);
            if(existTeacher is not Teacher)
            {
                throw new Exception("teacher yok");
            }
            return Response<TeacherInformationDto>.Success(new TeacherInformationDto() { TeacherName = existTeacher.FirstName + " " + existTeacher.LastName }, 200);
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

        public async Task<Response<NoContent>> UpdateStudentGrades(List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos)
        {
            string liststudentSerializer="";
          List<Student> liststudent= await teacherRepository.StudentUpdateGrades(enteringStudentGradesRequestDtos);

         var jsonserializerObject=  liststudent.DistinctBy(y=>y.FirstName).Select(y => new UpdateStudentGradesResponseDto()
         {

             ExamNoteResponseDtos=y.LessonConfirmation.Select(y=>y.ExamNote).Select(y=> new ExamNoteResponseDto()
             {
                    Exam_1=y.Exam1,
                    Exam_2=y.Exam2,
             }).ToList(),
             FirstName=y.FirstName,
         }).ToList();




            try
            {
               await _saves.SaveChangesAsync();
               return Response<NoContent>.Success(204);

            }
            catch (Exception e)
            {
                liststudentSerializer = JsonSerializer.Serialize(jsonserializerObject);
                throw new DontUpdateExamOfStudentException(liststudentSerializer);
            }
           
        }
    }
}
