using EKtu.Repository.Dtos;
namespace EKtu.WEBAPI
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenRequestDto tokenRequestDto;
        public TokenMiddleware(RequestDelegate next,TokenRequestDto tokenRequestDto)
        {
            _next = next;
            this.tokenRequestDto = tokenRequestDto;
        }
        public async Task Invoke(HttpContext context)
        {
          
            var userId = context.User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            var classId = context.User.Claims.FirstOrDefault(x => x.Type == "classId");

            if(classId is not null)
            {
                tokenRequestDto.Claims = new List<System.Security.Claims.Claim>()
                {
                new System.Security.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",userId.Value),
                new System.Security.Claims.Claim("classId",classId.Value)
                };
                await _next(context);
            }
            else
            {
                tokenRequestDto.Claims = new List<System.Security.Claims.Claim>()
                {
                new System.Security.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",userId.Value),
                };
                await _next(context);
            }
            
        }
    }
}
