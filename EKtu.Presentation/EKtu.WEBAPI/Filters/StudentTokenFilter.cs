using EKtu.Repository.Dtos;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EKtu.WEBAPI.Filters
{
    public class StudentTokenFilter : IActionFilter
    {
        public StudentTokenFilter()
        {
            
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            StudentResponseTokenDto studentResponseTokenDto = context.HttpContext.RequestServices.GetRequiredService<StudentResponseTokenDto>();
            if (studentResponseTokenDto.ClassId!=0 || studentResponseTokenDto.UserId!=0)
            {

            }
            else
            {
                var userId = context.HttpContext.User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                var classId = context.HttpContext.User.Claims.First(x => x.Type == "classId");

                studentResponseTokenDto.UserId = Convert.ToInt32(userId.Value);
                studentResponseTokenDto.ClassId = Convert.ToInt32(classId.Value);
            }
           
        }
    }
}
