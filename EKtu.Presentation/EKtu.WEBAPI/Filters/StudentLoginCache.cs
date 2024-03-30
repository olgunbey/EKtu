using EKtu.Repository.Constant;
using EKtu.Repository.Dtos;
using EKtu.Repository.IService.CacheService;
using EKtu.Repository.IService.StudentService;
using EKtu.Repository.IService.TokenService;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceStack;
using System.Text;
using System.Text.Json;

namespace EKtu.WEBAPI.Filters
{
    public class StudentLoginCache : IAsyncActionFilter
    {
        private readonly ICache<CacheRefreshTokenDto> _cacheRefreshTokenDto;
        private readonly IStudentService studentService;
        private readonly ITokenService tokenService;
        public StudentLoginCache(ICache<CacheRefreshTokenDto> cache,IStudentService studentService,ITokenService tokenService)
        {
            _cacheRefreshTokenDto = cache;
            this.studentService = studentService;
            this.tokenService = tokenService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
          var x =  context.HttpContext.Request.Headers["studentId"]; //burada 
          var CacheTokens= await _cacheRefreshTokenDto.GetCache(CacheConstant.StudentLoginRefreshTokenKey);

            if (CacheTokens.Any(y => y.Id == x))
            {
               await context.HttpContext.Response.WriteAsJsonAsync("Bu kullanıcıya ait bir refresh token var");
                return;

            }
            await next();
            await Console.Out.WriteLineAsync("yy");

        }
    }

}
