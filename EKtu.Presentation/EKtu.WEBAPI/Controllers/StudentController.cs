using EKtu.Domain.Entities;
using EKtu.Persistence.Builder.BuilderCreate;
using EKtu.Persistence.Builder.IBuilder;
using EKtu.Repository.Dtos;
using EKtu.Repository.IRepository.AddPersonRepository;
using EKtu.Repository.IService.AddPersonService;
using EKtu.Repository.IService.StudentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EKtu.WEBAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentController : ResponseBase
    {
        private readonly IStudentService _studentService;
        private readonly IAddPersonService<Student> addPersonService;
        private readonly IStudentBuilder studentBuilder;
        public StudentController(IStudentService studentService, IAddPersonService<Student> addPersonService, IStudentBuilder studentBuilder)
        {
            _studentService = studentService;
            this.addPersonService = addPersonService;
            this.studentBuilder = studentBuilder;
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
                .TckNo(studentRequestDto.StudentTckNo)
                .Password(studentRequestDto.StudentPassword).GetPerson();

            return ResponseData(await addPersonService.AddAsync(studentBuilders));
        }
        [HttpGet]
        [Authorize(Policy ="StudentList")]
        public async Task<IActionResult> StudentListExamGrande([FromHeader]int studentId)
        {
            var response= await _studentService.StudentListExamGrandeAsync(studentId);
            return ResponseData(response);
        }

        [HttpGet]
        [Authorize(Policy ="StudentId")]
        public async Task<IActionResult> StudentGetById([FromHeader]int id)
        {
            var response = await _studentService.GetByIdAsync(id);
            return ResponseData(response);
        }
    }
}
