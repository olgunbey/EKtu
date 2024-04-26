using EKtu.AuthServer;
using EKtu.AuthServer.PasswordValidator;
using EKtu.Persistence;
using EKtu.Persistence.Repository.EmailPasswordRepository;
using EKtu.Persistence.Service.EmailPasswordService;
using EKtu.Repository.IRepository;
using EKtu.Repository.IService.EmailPasswordService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddIdentityServer()
    .AddInMemoryApiScopes(Config.GetApiScopes())
    .AddInMemoryClients(Config.GetClients())
    .AddInMemoryApiResources(Config.GetApiResources())
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddResourceOwnerValidator<PasswordValidator>().AddDeveloperSigningCredential();
builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseSqlServer(SqlConnectionString.GetConnectionString));


builder.Services.AddScoped(typeof(IPasswordService<>), typeof(PasswordService<>));
builder.Services.AddScoped(typeof(IPasswordRepository<>), typeof(EmailPasswordRepository<>));
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

app.UseAuthorization();
app.UseAuthentication();
app.UseIdentityServer();
app.MapRazorPages();

app.Run();
