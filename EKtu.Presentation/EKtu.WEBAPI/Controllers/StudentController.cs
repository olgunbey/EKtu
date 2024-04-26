using EKtu.Repository.Dtos;
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
        [Authorize(Policy ="StudentList")]
        public async Task<IActionResult> StudentListExamGrande([FromHeader]int studentId)
        {

            var response=  await _studentService.StudentListExamGrandeAsync(studentId);
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
