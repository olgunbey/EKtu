using EKtu.AuthServer;
using EKtu.AuthServer.PasswordValidator;
using EKtu.Domain.Entities;
using EKtu.Persistence;
using EKtu.Persistence.Builder.BuilderCreate;
using EKtu.Persistence.Builder.IBuilder;
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
using EKtu.Repository.IService.EmailPasswordService;
using EKtu.Repository.IService.PrincipalService;
using EKtu.Repository.IService.StudentService;
using EKtu.Repository.IService.TeacherService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddIdentityServer()
    .AddInMemoryApiScopes(Config.GetApiScopes())
    .AddInMemoryClients(Config.GetClients())
    .AddInMemoryApiResources(Config.GetApiResources())
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddResourceOwnerValidator<PasswordValidator>().AddProfileService<ProfileService>().AddDeveloperSigningCredential();
builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseSqlServer(SqlConnectionString.GetConnectionString));
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Vuejs",
        builder => builder.WithOrigins("http://localhost:8080")
        .AllowAnyHeader()
        .AllowAnyMethod());
});


builder.Services.AddScoped(typeof(IPasswordService<>), typeof(PasswordService<>));
builder.Services.AddScoped(typeof(IPasswordRepository<>), typeof(EmailPasswordRepository<>));
builder.Services.AddScoped(typeof(IStudentService), typeof(StudentService));
builder.Services.AddScoped(typeof(IStudentRepository), typeof(StudentRepository));
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(ISaves), typeof(Saves));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(typeof(IStudentBuilder), typeof(StudentBuilder));
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
builder.Services.AddScoped(typeof(ITeacherRepository), typeof(TeacherRepository));
builder.Services.AddScoped(typeof(ITeacherService), typeof(TeacherService));
builder.Services.AddScoped(typeof(IPrincipalRepository), typeof(PrincipalRepository));
builder.Services.AddScoped(typeof(IPrincipalService), typeof(PrincipalService));
builder.Services.AddScoped(typeof(IPrincipalBuilder), typeof(PrincipalBuilder));
builder.Services.AddScoped(typeof(ILessonBuilder), typeof(LessonBuilder));
builder.Services.AddScoped(y => new Student());
builder.Services.AddScoped(y => new Principal());
builder.Services.AddScoped(y => new Lesson());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("Vuejs");

app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();
app.MapRazorPages();

app.Run();
