using EKtu.Domain.Entities;
using EKtu.Persistence.Builder.BuilderCreate;
using EKtu.Persistence.Builder.IBuilder;
using EKtu.Repository.Dtos;
using EKtu.Repository.ICacheService.StudentCacheService;
using EKtu.Repository.IRepository.AddPersonRepository;
using EKtu.Repository.IService.AddPersonService;
using EKtu.Repository.IService.PdfService;
using EKtu.Repository.IService.StudentService;
using EKtu.WEBAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
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
        private readonly StudentResponseTokenDto studentResponseTokenDto;
        public StudentController(IStudentService studentService, IAddPersonService<Student> addPersonService, IStudentBuilder studentBuilder, IPdfService pdfService, IStudentCacheService studentCacheService, StudentResponseTokenDto studentResponseTokenDto)
        {
            _studentService = studentService;
            this.addPersonService = addPersonService;
            this.studentBuilder = studentBuilder;
            _pdfService = pdfService;
            _studentCacheService = studentCacheService;
            this.studentResponseTokenDto = studentResponseTokenDto;
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
        [ServiceFilter(typeof(StudentTokenFilter))]
        [Authorize(Policy ="StudentList")]
         public async Task<IActionResult> StudentListExamGrande([FromHeader]bool term) 
         {
         var resp= await _studentCacheService.GetCacheStudentGradeList(studentResponseTokenDto.ClassId,studentResponseTokenDto.UserId,term);
            if(resp.Data is not null)
            {
                return ResponseData(resp);
            }
            return ResponseData(resp);
         }

        [HttpGet]
        [ServiceFilter(typeof(StudentTokenFilter))]
        [Authorize(Policy ="StudentList")]

        public async Task<IActionResult> StudentGetById()
        {

            var userId = studentResponseTokenDto.UserId;
            var response = await _studentService.GetByIdAsync(userId);
            return ResponseData(response);
        }
        [HttpPost]
        [ServiceFilter(typeof(StudentTokenFilter))]
        [Authorize(Policy ="StudentChooseLesson")]
        public async Task<IActionResult> StudentChooseLesson([FromBody]StudentChooseLessonRequestDto studentChooseLessonRequestDto)
        {
            var response = await _studentService.StudentChooseLessonAsync(studentChooseLessonRequestDto, studentResponseTokenDto.UserId);
            var x = await _studentCacheService.AllStudentCacheLesson();
            return ResponseData<NoContent>(response);
        }
        [HttpGet]
        [ServiceFilter(typeof(StudentTokenFilter))]
        [Authorize(Policy = "StudentAbsence")]
        public async Task<IActionResult> StudentAbsenceList()
        {
            return ResponseData(await _studentService.StudentAbsenceAsync(studentResponseTokenDto.UserId));
        }

        [HttpGet]
        [ServiceFilter(typeof(StudentTokenFilter))]
        [Authorize(Policy = "StudentCertificatePolicy")]
        public async Task<IActionResult> StudentCertificate()
        {
            var Resp = await _studentService.StudentCertificateAsync(studentResponseTokenDto.UserId);

            var bytes = _pdfService.PdfBytes(Resp.Data);
            Random random = new Random();
            var randoms= random.Next(1, 100000);
            await System.IO.File.WriteAllBytesAsync(@$"C:\Users\olgun\OneDrive\Masaüstü\pdf\{randoms}.pdf",bytes);

            return ResponseData(Response<byte[]>.Success(bytes, 200));
        }

        [HttpGet]
        [ServiceFilter(typeof(StudentTokenFilter))]
        [Authorize(Policy ="GetStudentChooseLesson")]
        public async Task<IActionResult> GetStudentChooseLesson()
        {

            var List=  await _studentCacheService.GetStudentCacheLesson(studentResponseTokenDto.UserId);
            if (List.Data is not null)
                return ResponseData(List);
            return ResponseData(await _studentService.GetStudentChooseLessonAsync(studentResponseTokenDto.UserId));
        }

        [HttpPost]
        [ServiceFilter(typeof(StudentTokenFilter))]
        [Authorize(Policy = "GetStudentChooseLesson")] 
        public async Task<IActionResult> StudentChangeLesson(List<StudentChangeLessonRequestDto> studentChangeLessonRequestDtos)
        {
            var resp= await _studentService.StudentChangeLessonAsync(studentChangeLessonRequestDtos,studentResponseTokenDto.UserId);
            await _studentCacheService.AllStudentCacheLesson();
            return ResponseData(resp);
        }

        [HttpGet]
        [Authorize("ClientCredentials")]
        public async Task<IActionResult> GetClassList()
        {
         var responseData=  await _studentService.GetClassList();
            return ResponseData(responseData);
        }
        [HttpGet]
        [ServiceFilter(typeof(StudentTokenFilter))]
        [Authorize(Policy = "GetStudentChooseLesson")]
        public async Task<IActionResult> TermLessonList([FromHeader]TermLessonListRequestDto termLessonListRequestDto)
        {
         var RespData= await  _studentService.GetLessonTerm(termLessonListRequestDto, studentResponseTokenDto.Grade);
            return ResponseData(RespData);
        }
    }
}
