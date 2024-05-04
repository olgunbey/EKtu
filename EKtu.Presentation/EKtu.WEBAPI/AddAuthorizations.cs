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
                x.AddPolicy("ClientCredentials", y =>
                {
                    y.RequireClaim("scope", "base.token");
                });
                x.AddPolicy("StudentChooseLesson", y =>
                {
                    y.RequireClaim("scope", "choose.lesson");
                });
                x.AddPolicy("LessonApprove", y =>
                {
                    y.RequireClaim("scope", "lesson.approve");
                });
                x.AddPolicy("LessonAdded", y =>
                {
                    y.RequireClaim("scope", "lesson.added");
                });
                x.AddPolicy("StudentCalculateLetterGrande", y =>
                {
                    y.RequireClaim("scope", "student.calculateexamgrande");
                });
                x.AddPolicy("AttendancePolicy", y =>
                {
                    y.RequireClaim("scope", "absence.entry");
                });
                x.AddPolicy("TeacherClassLessonList", y =>
                {
                    y.RequireClaim("scope", "teacher.classlessonlist");
                });
                x.AddPolicy("StudentAbsence", y =>
                {
                    y.RequireClaim("scope", "student.absence");
                });
                
            });
        }
    }
}
