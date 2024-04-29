using EKtu.Domain.Entities;
using EKtu.Infrastructure.CacheServices;
using EKtu.Infrastructure.EmailService;
using EKtu.Infrastructure.TokenServices;
using EKtu.Persistence.Builder.BuilderCreate;
using EKtu.Persistence.Builder.IBuilder;
using EKtu.Persistence.Repository.AddPersonRepository;
using EKtu.Persistence.Repository.EmailPasswordRepository;
using EKtu.Persistence.Repository.PrincipalRepository;
using EKtu.Persistence.Repository.StudentRepository;
using EKtu.Persistence.Repository.TeacherRepository;
using EKtu.Persistence.Repositorys;
using EKtu.Persistence.Service;
using EKtu.Persistence.Service.AddPersonService;
using EKtu.Persistence.Service.EmailPasswordService;
using EKtu.Persistence.Service.PrincipalService;
using EKtu.Persistence.Service.StudentService;
using EKtu.Persistence.Service.TeacherService;
using EKtu.Repository.IRepository;
using EKtu.Repository.IRepository.AddPersonRepository;
using EKtu.Repository.IRepository.PrincipalRepository;
using EKtu.Repository.IRepository.StudentRepository;
using EKtu.Repository.IRepository.TeacherRepository;
using EKtu.Repository.IService;
using EKtu.Repository.IService.AddPersonService;
using EKtu.Repository.IService.CacheService;
using EKtu.Repository.IService.EmailPasswordService;
using EKtu.Repository.IService.EmailService;
using EKtu.Repository.IService.PrincipalService;
using EKtu.Repository.IService.StudentService;
using EKtu.Repository.IService.TeacherService;
using EKtu.Repository.IService.TokenService;
using System.ComponentModel;

namespace EKtu.WEBAPI
{
    public static class DIContainer
    {
        public static void AddedService(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddScoped<IStudentRepository, StudentRepository>();
            serviceDescriptors.AddScoped<IStudentService, StudentService>();
            serviceDescriptors.AddScoped<ITeacherRepository,TeacherRepository>();
            serviceDescriptors.AddScoped<ITeacherService, TeacherService>();
            serviceDescriptors.AddScoped<ISaves, Saves>();
            serviceDescriptors.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            serviceDescriptors.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            serviceDescriptors.AddScoped<IEmail, Email>();
            serviceDescriptors.AddScoped<ITokenService, TokenService>();
            serviceDescriptors.AddScoped(typeof(ICache<>), typeof(Cache<>));
            serviceDescriptors.AddScoped<IPrincipalService,PrincipalService>();
            serviceDescriptors.AddScoped<IPrincipalRepository, PrincipalRepository>();
            serviceDescriptors.AddScoped(typeof(IPasswordRepository<>), typeof(EmailPasswordRepository<>));
            serviceDescriptors.AddScoped(typeof(IPasswordService<>), typeof(PasswordService<>));
            serviceDescriptors.AddScoped(typeof(IAddPersonService<>), typeof(AddPersonService<>));
            serviceDescriptors.AddScoped(typeof(IAddPersonRepository<>), typeof(AddPersonRepository<>));
            serviceDescriptors.AddScoped<IStudentBuilder, StudentBuilder>();
            serviceDescriptors.AddScoped<ITeacherBuilder,TeacherBuilder>();
            serviceDescriptors.AddScoped<IPrincipalBuilder, PrincipalBuilder>();
            serviceDescriptors.AddScoped<Principal>(y => new Principal());
            serviceDescriptors.AddScoped<Student>(y => new Student());
            serviceDescriptors.AddScoped<Teacher>(y => new Teacher());
            serviceDescriptors.AddScoped<Lesson>(y => new Lesson());
            serviceDescriptors.AddScoped<TeacherClassLesson>(y=> new TeacherClassLesson());
            serviceDescriptors.AddScoped<ILessonBuilder,LessonBuilder>();
            serviceDescriptors.AddScoped<ITeacherClassLessonBuilder,TeacherClassLessonBuilder>();

        }
    }
}
