using EKtu.Domain.Entities;
using EKtu.Repository.Dtos;
using EKtu.Repository.IService.EmailService;
using EKtu.Repository.IService.StudentService;
using EKtu.Repository.IService.TokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EKtu.WEBAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmailController : ResponseBase
    {
        private readonly IEmail _mail;
        private readonly ITokenService tokenService;
        private readonly IStudentService studentService;

        public EmailController(IEmail email, ITokenService tokenService, IStudentService studentService)
        {
            _mail = email;
            this.tokenService = tokenService;
            this.studentService = studentService;
        }

        [HttpPost]
        public async Task<IActionResult> EmailSend(string targetEmail)
        {
          Response<int> hasId= await studentService.StudentCheckEmailAsync(targetEmail);
            if(!hasId.IsSuccessfull)
            {
                return ResponseData(hasId);
            }
          var Token=  await tokenService.GeneratedTokenService(new Repository.Dtos.GenerateTokenRequestDto()
            {
             userId=hasId.Data  
            });
           var HasEmail= await  _mail.SendMail(targetEmail, $"https://localhost:6161/Student/pwChange/{Token.Data.AccessToken}");

            return ResponseData(HasEmail);
        }
    }
}
