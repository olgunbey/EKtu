using EKtu.Repository.IService.PrincipalService;
using EKtu.Repository.IService.StudentService;
using EKtu.Repository.IService.TeacherService;
using IdentityServer4.Models;
using IdentityServer4.Services;
using ServiceStack;
using System.Security.Claims;

namespace EKtu.AuthServer
{
    public class ProfileService : IProfileService
    {
        private readonly IStudentService studentService;
        private readonly ITeacherService teacherService;
        private readonly IPrincipalService principalService;
        public ProfileService(IStudentService studentService,ITeacherService teacherService,IPrincipalService principalService)
        {
            this.studentService = studentService;
            this.teacherService = teacherService;
            this.principalService = principalService;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if(context.Client.ClientId=="ResourceOwnerStudent") //öğrenci
            {
                var IdClaim = context.Subject.Claims.First(x => x.Type == "sub");
                var responseDtos = await studentService.StudentInformation(Convert.ToInt32(IdClaim.Value));
                var resp= new List<Claim>()
                {
                    new Claim("name",responseDtos.Data.StudentName),
                    new Claim("classname",responseDtos.Data.ClassName),
                    new Claim("classId",responseDtos.Data.ClassId.ToString())
                };
                context.IssuedClaims.AddRange(resp);
            }
            else if (context.Client.ClientId == "ResourceOwnerTeacher")
            {
                var IdClaim = context.Subject.Claims.First(x => x.Type == "sub");
              var responseDtos= await teacherService.TeacherInformation(Convert.ToInt32(IdClaim.Value));
                var resp= new List<Claim>()
                {
                    new Claim("name",responseDtos.Data.TeacherName)
                };
                context.IssuedClaims.AddRange(resp);
            }
            else if (context.Client.ClientId == "ResourceOwnerPrincipal")
            {
                var IdClaim = context.Subject.Claims.First(x => x.Type == "sub");
                var responseDtos= await principalService.PrincipalInformation(Convert.ToInt32(IdClaim.Value));
                var resp = new List<Claim>()
                {
                    new Claim("name",responseDtos.Data.PrincipalName)
                };
                context.IssuedClaims.AddRange(resp);
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
