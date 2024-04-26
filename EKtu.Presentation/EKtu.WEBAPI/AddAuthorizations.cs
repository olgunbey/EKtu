using ServiceStack;
using System.Runtime.CompilerServices;

namespace EKtu.WEBAPI
{
    public static class AddAuthorizations
    {
        public static void Authorization(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddAuthorization(x =>
            {
                x.AddPolicy("StudentList", y =>
                {
                    y.RequireClaim("scope", "exam.list");
                });
            });
        }
    }
}
