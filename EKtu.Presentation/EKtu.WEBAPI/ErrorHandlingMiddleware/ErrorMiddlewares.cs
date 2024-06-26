using EKtu.Repository.Dtos;
using EKtu.WEBAPI.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json;

namespace EKtu.WEBAPI.ErrorHandlingMiddleware
{
    public static class ErrorMiddlewares
    {
        public static void ErrorMiddleware(this WebApplication webApplication)
        {
            //webApplication.UseExceptionHandler(exceptionHandlerApp =>
            //exceptionHandlerApp.Run(async context =>
            //{
            //  var exceptionHandlerFeatures= context.Features.Get<IExceptionHandlerFeature>();


            //    if(exceptionHandlerFeatures!.Path=="/api/teacher/enteringstudentgrades")
            //    {
            //        var requestServices= context.RequestServices.GetRequiredService<MyLogging>();
            //        requestServices.LogInformation(exceptionHandlerFeatures.Error.Message);

            //    }

            //})



            //);
        }
    }
}
