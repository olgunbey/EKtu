using EKtu.Domain.Entities;
using EKtu.Infrastructure.CacheServices;
using EKtu.Infrastructure.EmailService;
using EKtu.Infrastructure.TokenServices;
using EKtu.Persistence.Repository.EmailPasswordRepository;
using EKtu.Persistence.Repository.PrincipalRepository;
using EKtu.Persistence.Repository.StudentRepository;
using EKtu.Persistence.Repository.TeacherRepository;
using EKtu.Persistence.Repositorys;
using EKtu.Persistence.Service;
using EKtu.Persistence.Service.EmailPasswordService;
using EKtu.Persistence.Service.PrincipalService;
using EKtu.Persistence.Service.StudentService;
using EKtu.Persistence.Service.TeacherService;
using EKtu.Repository.IRepository;
using EKtu.Repository.IRepository.PrincipalRepository;
using EKtu.Repository.IRepository.StudentRepository;
using EKtu.Repository.IRepository.TeacherRepository;
using EKtu.Repository.IService;
using EKtu.Repository.IService.CacheService;
using EKtu.Repository.IService.EmailPasswordService;
using EKtu.Repository.IService.EmailService;
using EKtu.Repository.IService.PrincipalService;
using EKtu.Repository.IService.StudentService;
using EKtu.Repository.IService.TeacherService;
using EKtu.Repository.IService.TokenService;

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
        }
    }
}
