using EKtu.Domain.Entities;
using EKtu.Repository.Dtos;
using EKtu.Repository.IService.StudentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EKtu.WEBAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentController : ResponseBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody]StudentRequestDto studentRequestDto)
        {
            var responseDto=  await _studentService.AddStudentHashPasswordAsync(studentRequestDto);
            return ResponseData<bool>(responseDto);
        }
        [HttpGet]
        [Authorize(Policy = "StudentPolicy")]
        public async Task<IActionResult> StudentListExamGrande([FromHeader]int studentId)
        {
            var response=  await _studentService.StudentListExamGrandeAsync(studentId);
            return ResponseData<List<StudentListExamGrandeResponseDto>>(response);
        }
        [HttpPost]
        public async Task<IActionResult> StudentLogin([FromBody]StudentLoginRequestDto studentLoginRequestDto)
        {
            var response= await  _studentService.StudentLogin(studentLoginRequestDto);
            return ResponseData(response);
        }
    }
}
