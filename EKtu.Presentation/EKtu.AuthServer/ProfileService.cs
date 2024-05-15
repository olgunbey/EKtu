using EKtu.Repository.IService.StudentService;
using IdentityServer4.Models;
using IdentityServer4.Services;
using ServiceStack;

namespace EKtu.AuthServer
{
    public class ProfileService : IProfileService
    {
        private readonly IStudentService studentService;
        public ProfileService(IStudentService studentService)
        {
            this.studentService = studentService;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {            
             if(context.RequestedResources.Resources.IdentityResources.Select(y => y.UserClaims).Any(y => y.Contains("classId")))
            {
                var IdClaim = context.Subject.Claims.First(x => x.Type == "sub");
              var responseDtos = await  studentService.GetStudentClassIdWithStudentIdAsync(Convert.ToInt32(IdClaim.Value));
                context.IssuedClaims = new List<System.Security.Claims.Claim>()
                {
                    new System.Security.Claims.Claim("classId",responseDtos.Data.ToString())
                };
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
