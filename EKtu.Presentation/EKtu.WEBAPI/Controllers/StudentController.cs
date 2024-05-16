using EKtu.Domain.Entities;
using EKtu.Persistence.Builder.BuilderCreate;
using EKtu.Persistence.Builder.IBuilder;
using EKtu.Repository.Dtos;
using EKtu.Repository.ICacheService.StudentCacheService;
using EKtu.Repository.IRepository.AddPersonRepository;
using EKtu.Repository.IService.AddPersonService;
using EKtu.Repository.IService.PdfService;
using EKtu.Repository.IService.StudentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EKtu.WEBAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentController : ResponseBase
    {
        private readonly IStudentService _studentService;
        private readonly IAddPersonService<Student> addPersonService;
        private readonly IStudentBuilder studentBuilder;
        private readonly IPdfService _pdfService;
        private readonly IStudentCacheService _studentCacheService;
        private readonly TokenRequestDto tokenRequestDto;
        public StudentController(IStudentService studentService, IAddPersonService<Student> addPersonService, IStudentBuilder studentBuilder, IPdfService pdfService, IStudentCacheService studentCacheService, TokenRequestDto tokenRequestDto)
        {
            _studentService = studentService;
            this.addPersonService = addPersonService;
            this.studentBuilder = studentBuilder;
            _pdfService = pdfService;
            _studentCacheService = studentCacheService;
            this.tokenRequestDto = tokenRequestDto;
        }
        [HttpPost]
        [Authorize("ClientCredentials")]    
        public async Task<IActionResult> AddStudent([FromBody]StudentRequestDto studentRequestDto)
        {
            Student studentBuilders = studentBuilder
                .ClassId(studentRequestDto.ClassId)
                .LastName(studentRequestDto.StudentLastName)
                .FirstName(studentRequestDto.StudentName)
                .Email(studentRequestDto.Email)
                .TckNo(studentRequestDto.StudentTckNo).Password(studentRequestDto.StudentPassword).GetPerson();

            return ResponseData(await addPersonService.AddAsync(studentBuilders));
        }
        [HttpGet]
        [Authorize(Policy ="StudentList")]
        public async Task<IActionResult> StudentListExamGrande()
        {
           var userId=tokenRequestDto.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
           var classId = tokenRequestDto.Claims.First(x => x.Type == "classId");
         var resp=  await _studentCacheService.GetCacheStudentGradeList(Convert.ToInt32(classId.Value),Convert.ToInt32(userId.Value));
            if(resp.Data.Any())
            {
                return ResponseData(resp);
            }
           var response= await _studentService.StudentListExamGrandeAsync(Convert.ToInt32(userId.Value));
            return ResponseData(response);
        }

        [HttpGet]
        [Authorize(Policy ="StudentId")]
        public async Task<IActionResult> StudentGetById()
        {
          var userId=  tokenRequestDto.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
          var response = await _studentService.GetByIdAsync(Convert.ToInt32(userId.Value));
            return ResponseData(response);
        }
        [HttpPost]
        [Authorize(Policy ="StudentChooseLesson")]
        public async Task<IActionResult> StudentChooseLesson([FromBody]StudentChooseLessonRequestDto studentChooseLessonRequestDto)
        {
            var response = await _studentService.StudentChooseLessonAsync(studentChooseLessonRequestDto);
            var x =   await _studentCacheService.AllStudentCacheLesson();
            return ResponseData<NoContent>(response);
        }
        [HttpGet]
        [Authorize(Policy = "StudentAbsence")]
        public async Task<IActionResult> StudentAbsenceList()
        {
            var userId=  tokenRequestDto.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            return ResponseData(await _studentService.StudentAbsenceAsync(Convert.ToInt32(userId.Value)));
        }

        [HttpGet]
        [Authorize(Policy = "StudentCertificatePolicy")]
        public async Task<IActionResult> StudentCertificate()
        {
            var userId= tokenRequestDto.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            var Resp = await _studentService.StudentCertificateAsync(Convert.ToInt32(userId.Value));

            var bytes = _pdfService.PdfBytes(Resp.Data);


            Random random = new Random();
            var randoms= random.Next(1, 100000);
            await System.IO.File.WriteAllBytesAsync(@$"C:\Users\olgun\OneDrive\Masaüstü\pdf\{randoms}.pdf",bytes);

            return ResponseData(Response<byte[]>.Success(bytes, 200));
        }

        [HttpGet]
        [Authorize(Policy ="GetStudentChooseLesson")]
        public async Task<IActionResult> GetStudentChooseLesson()
        {
            var userId = tokenRequestDto.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            var List=  await _studentCacheService.GetStudentCacheLesson(Convert.ToInt32(userId.Value));
            if (List.Data.Any())
                return ResponseData(List);
            return ResponseData(await _studentService.GetStudentChooseLessonAsync(Convert.ToInt32(userId.Value)));
        }

        [HttpPost]
        [Authorize(Policy = "GetStudentChooseLesson")]
        public async Task<IActionResult> StudentChangeLesson(List<StudentChangeLessonRequestDto> studentChangeLessonRequestDtos)
        {
            var userId = tokenRequestDto.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            var resp= await _studentService.StudentChangeLessonAsync(studentChangeLessonRequestDtos, int.Parse(userId.Value));
            await _studentCacheService.AllStudentCacheLesson();
            return ResponseData(resp);
        }
    }
}
