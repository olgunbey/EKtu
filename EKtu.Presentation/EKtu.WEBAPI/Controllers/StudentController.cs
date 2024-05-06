using EKtu.Domain.Entities;
using EKtu.Persistence.Builder.BuilderCreate;
using EKtu.Persistence.Builder.IBuilder;
using EKtu.Repository.Dtos;
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
        public StudentController(IStudentService studentService, IAddPersonService<Student> addPersonService, IStudentBuilder studentBuilder, IPdfService pdfService)
        {
            _studentService = studentService;
            this.addPersonService = addPersonService;
            this.studentBuilder = studentBuilder;
            _pdfService = pdfService;
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
           var userId = User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
           var response= await _studentService.StudentListExamGrandeAsync(Convert.ToInt32(userId.Value));
            return ResponseData(response);
        }

        [HttpGet]
        [Authorize(Policy ="StudentId")]
        public async Task<IActionResult> StudentGetById()
        {
          var userId=  User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            var response = await _studentService.GetByIdAsync(Convert.ToInt32(userId.Value));
            return ResponseData(response);
        }
        [HttpPost]
        [Authorize(Policy ="StudentChooseLesson")]
        public async Task<IActionResult> StudentChooseLesson([FromBody]StudentChooseLessonRequestDto studentChooseLessonRequestDto)
        {
            return ResponseData<NoContent>(await _studentService.StudentChooseLessonAsync(studentChooseLessonRequestDto));
        }
        [HttpGet]
        [Authorize(Policy = "StudentAbsence")]
        public async Task<IActionResult> StudentAbsenceList()
        {
            var userId=  User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            return ResponseData(await _studentService.StudentAbsenceAsync(Convert.ToInt32(userId.Value)));
        }

        [HttpGet]
        [Authorize(Policy = "StudentCertificatePolicy")]
        public async Task<IActionResult> StudentCertificate()
        {
            var userId= User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            var Resp = await _studentService.StudentCertificateAsync(Convert.ToInt32(userId.Value));

            var bytes = _pdfService.PdfBytes(Resp.Data);


            Random random = new Random();
            var randoms= random.Next(1, 100000);
            await System.IO.File.WriteAllBytesAsync(@$"C:\Users\olgun\OneDrive\Masaüstü\pdf\{randoms}.pdf",bytes);

            return ResponseData(Response<byte[]>.Success(bytes, 200));
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentChooseLesson()
        {

            return Ok();
        }
    }
}
