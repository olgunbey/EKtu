using EKtu.Domain.Entities;
using EKtu.Persistence.Builder.IBuilder;
using EKtu.Repository.Dtos;
using EKtu.Repository.ICacheService.StudentCacheService;
using EKtu.Repository.IService.AddPersonService;
using EKtu.Repository.IService.TeacherService;
using EKtu.WEBAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EKtu.WEBAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TeacherController : ResponseBase
    {
        private readonly IAddPersonService<Teacher> addTeacherService;
        private readonly ITeacherBuilder teacherBuilder;
        private readonly ITeacherService teacherService;
        private readonly IStudentCacheService studentCacheService;
        private readonly TeacherTokenResponseDto teacherTokenResponseDto;

        public TeacherController(IAddPersonService<Teacher> addteacherService, ITeacherBuilder teacherBuilder, ITeacherService teacherService, IStudentCacheService studentCacheService, TeacherTokenResponseDto teacherTokenResponseDto)
        {
            this.addTeacherService = addteacherService;
            this.teacherBuilder = teacherBuilder;
            this.teacherService = teacherService;
            this.studentCacheService = studentCacheService;
            this.teacherTokenResponseDto = teacherTokenResponseDto;
        }
        [HttpPost]
        [Authorize(Policy ="ClientCredentials")]
        public async Task<IActionResult> AddTeacher([FromBody]AddTeacherRequestDto teacherRequestDto)
        {
           Teacher _teacher= this.teacherBuilder
                .FirstName(teacherRequestDto.FirstName)
                .LastName(teacherRequestDto.LastName)
                .Password(teacherRequestDto.Password)
                .TckNo(teacherRequestDto.TckNo)
                .GetPerson();
            return ResponseData<NoContent>(await this.addTeacherService.AddAsync(_teacher));
        }
        [HttpGet]
        [ServiceFilter(typeof(TeacherTokenFilter))]
        [Authorize(Policy = "TeacherClassLessonList")]
        public async Task<IActionResult> TeacherClassList()
        {
            return ResponseData<List<TeacherClassReponseDto>>(await teacherService.TeacherClass(teacherTokenResponseDto.Id));
        }
        [HttpGet]
        [ServiceFilter(typeof(TeacherTokenFilter))]
        [Authorize(Policy = "TeacherClassLessonList")]
        public async Task<IActionResult> TeacherClassLessonList([FromHeader]string classId)
        {
          return ResponseData<List<TeacherLessonDto>>(await teacherService.TeacherLesson(Convert.ToInt32(classId), teacherTokenResponseDto.Id));

        }

        [HttpPost]
        [Authorize(Policy ="TeacherEnteringGrades")]
        public async Task<IActionResult> EnteringStudentGrades([FromBody]List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos, [FromQuery]int classId) 
        {
            //ilk başta veritabanından o sınıf ve o derse ait öğrencilerin degerlerini cachle
            var resp= teacherService.EnteringStudentGrades(enteringStudentGradesRequestDtos,out var data);
            if(!data.Any())//demekki güncelleme yok sadece examnote girişi var
            {
               await studentCacheService.TestCache(enteringStudentGradesRequestDtos, classId);
                //burada veritabanındaki bütün veriler cachlenir.
            }
            else
            {
                //burada güncelleme var....
                 await teacherService.UpdateStudentGrades(enteringStudentGradesRequestDtos); //veritabanında update işlemi yapar
                 await studentCacheService.StudentUpdateExamNote(enteringStudentGradesRequestDtos,classId); //cache'yi günceller
            }
            return ResponseData(resp);
        }
        [HttpPost]   
        public async Task<IActionResult> GetAllStudentByClassIdAndLessonId([FromBody]GetAllStudentByClassIdAndLessonIdRequestDto getAllStudentByClassIdAndLessonIdRequestDto)
        {
         return ResponseData(await teacherService.GetAllStudentByClassIdAndLessonId(getAllStudentByClassIdAndLessonIdRequestDto));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeeacher()
        {
            return Ok();
        }
    }
}
