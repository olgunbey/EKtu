using EKtu.Repository.Dtos;
using EKtu.Repository.IService.StudentService;
using EKtu.WEBAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

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
            return ResponseData(responseDto);
        }
        [HttpGet]
        public async Task<IActionResult> StudentListExamGrande([FromHeader]int studentId)
        {

            var response=  await _studentService.StudentListExamGrandeAsync(studentId);
            return ResponseData(response);
        }

        [TypeFilter(typeof(StudentLoginCache))]
        [HttpPost]
        public async Task<IActionResult> StudentLogin([FromBody]StudentLoginRequestDto studentLoginRequestDto, [FromHeader]int studentId)
        {
            var response = await _studentService.StudentLogin(studentLoginRequestDto);
            return ResponseData(response);
        }
        [HttpGet]
        public async Task<IActionResult> StudentGetById([FromHeader]int id)
        {
            var response = await _studentService.GetByIdAsync(id);
            return ResponseData(response);
        }
    }
}
