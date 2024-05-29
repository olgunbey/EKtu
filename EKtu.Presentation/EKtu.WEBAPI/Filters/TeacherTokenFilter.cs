using EKtu.Repository.Dtos;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EKtu.WEBAPI.Filters
{
    public class TeacherTokenFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            TeacherTokenResponseDto teacherTokenResponseDto = context.HttpContext.RequestServices.GetRequiredService<TeacherTokenResponseDto>();

            if(teacherTokenResponseDto.Id !=0 || teacherTokenResponseDto.TeacherName is not null)
            {

            }
            else
            {
                var teacherId = context.HttpContext.User.Claims.First(y => y.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                var teacherName = context.HttpContext.User.Claims.First(y => y.Type == "name");

                teacherTokenResponseDto.Id = Convert.ToInt32(teacherId.Value);
                teacherTokenResponseDto.TeacherName = teacherName.Value;
            }
           
        }
    }
}
