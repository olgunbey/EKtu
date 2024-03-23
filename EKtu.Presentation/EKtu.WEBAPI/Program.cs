using EKtu.Persistence;
using EKtu.Repository.Dtos;
using EKtu.WEBAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(y => y.UseSqlServer(builder.Configuration.GetConnectionString("mydb"))); //burada kaldým
builder.Services.AddedService();
builder.Services.Configure<Configuration>(builder.Configuration.GetSection("Configuration"));
builder.Services.AddAuthentication("Bearer").AddJwtBearer(x =>
{
    x.Authority = "https://localhost:7134";
    x.Audience = "Api";
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true
    };
});
builder.Services.AddAuthorization(y =>
{
    y.AddPolicy("StudentPolicy", x => x.RequireClaim("scope", "student.read"));

});
builder.Services.AddOptions();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
