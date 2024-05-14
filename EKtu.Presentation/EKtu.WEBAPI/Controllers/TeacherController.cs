using EKtu.Domain.Entities;
using EKtu.Persistence.Builder.IBuilder;
using EKtu.Repository.Dtos;
using EKtu.Repository.ICacheService.StudentCacheService;
using EKtu.Repository.IService.AddPersonService;
using EKtu.Repository.IService.TeacherService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        public TeacherController(IAddPersonService<Teacher> addteacherService, ITeacherBuilder teacherBuilder, ITeacherService teacherService, IStudentCacheService studentCacheService)
        {
            this.addTeacherService = addteacherService;
            this.teacherBuilder = teacherBuilder;
            this.teacherService = teacherService;
            this.studentCacheService = studentCacheService;
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
        [Authorize(Policy = "TeacherClassLessonList")]
        public async Task<IActionResult> TeacherClassList()
        {
            var teacherId = User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            return ResponseData<List<TeacherClassReponseDto>>(await teacherService.TeacherClass(Convert.ToInt32(teacherId.Value)));
        }
        [HttpGet]
        [Authorize(Policy = "TeacherClassLessonList")]
        public async Task<IActionResult> TeacherClassLessonList([FromHeader]int classId)
        {
         var teacherId= User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

          return ResponseData<List<TeacherLessonDto>>(await teacherService.TeacherLesson(classId, Convert.ToInt32(teacherId.Value)));

        }

        [HttpPost]
        public async Task<IActionResult> EnteringStudentGrades([FromBody]List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos) 
        {
           var resp=  teacherService.EnteringStudentGrades(enteringStudentGradesRequestDtos,out var data);
            if(!data.Any())
            {
                await studentCacheService.StudentNewExamGrande(); //bu veritabanındaki bütün notları cachler
            }
            else
            {
               await teacherService.UpdateStudentGrades(enteringStudentGradesRequestDtos);
               await studentCacheService.SetStudentCache(data,1); //parametreden gelen datalari cacchede güncelleyecek
                //buradaki classid'yi ayarla
            }
            return ResponseData(resp);
        }
    }
}
