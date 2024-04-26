using EKtu.Domain.Entities;
using EKtu.Repository.Dtos;
using EKtu.Repository.IService.AddPersonService;
using EKtu.Repository.IService.TeacherService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EKtu.WEBAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TeacherController : ResponseBase
    {
        private readonly IAddPersonService<Teacher> addTeacherService;

        public TeacherController(IAddPersonService<Teacher> addteacherService)
        {
           this.addTeacherService = addteacherService;
        }
        [HttpPost]
        [Authorize(Policy ="ClientCredentials")]
        public async Task<IActionResult> AddStudent([FromBody]Teacher teacher)
        {
            return ResponseData<NoContent>(await this.addTeacherService.AddAsync(teacher));
        }
    }
}
