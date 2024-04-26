using EKtu.Domain.Entities;
using EKtu.Repository.IService;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace EKtu.AuthServer.PasswordValidator
{
    public class PasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IPasswordService<Teacher> teacherpasswordService;
        private readonly IPasswordService<Student> studentpasswordService;
        private readonly IPasswordService<Domain.Entities.Principal> principalpasswordService;
        public PasswordValidator(IPasswordService<Teacher> teacherpasswordservice, IPasswordService<Student> studentpasswordService,IPasswordService<Domain.Entities.Principal> principalpasswordService)
        {
            this.teacherpasswordService = teacherpasswordservice;
            this.studentpasswordService = studentpasswordService;
            this.principalpasswordService = principalpasswordService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            int userId=0;
             userId = context.Request.ClientId switch
            {
                "ResourceOwnerTeacher" =>await teacherpasswordService.EmailAndPassword(context.UserName,context.Password),
                "ResourceOwnerStudent" => await studentpasswordService.EmailAndPassword(context.UserName, context.Password),
                "ResourceOwnerPrincipal" => await principalpasswordService.EmailAndPassword(context.UserName,context.Password),
            };

            if(userId!=0)
            {
                context.Result = new GrantValidationResult(userId.ToString(), OidcConstants.AuthenticationMethods.Password);
                    return;
            }
            context.Result=new GrantValidationResult(TokenRequestErrors.UnauthorizedClient);

        }
    }
}
