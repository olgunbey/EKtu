using EKtu.Persistence;
using EKtu.Repository.Dtos;
using EKtu.WEBAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServiceStack.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<DatabaseContext>(y => y.UseSqlServer(builder.Configuration.GetConnectionString("mydb"))); //burada kaldým
builder.Services.AddedService();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Vuejs",
        builder => builder.WithOrigins("http://localhost:8080")
        .AllowAnyHeader()
        .AllowAnyMethod());
});


builder.Services.Configure<Configuration>(builder.Configuration.GetSection("Configuration"));
builder.Services.AddScoped(y =>

{
    var ClientManager = new RedisManagerPool(builder.Configuration.GetConnectionString("redis"));
    var valueTask = ClientManager.GetClientAsync();
    return valueTask.Result;
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.Authority = "https://localhost:7134";
    x.Audience = "BaseApi";
    //x.TokenValidationParameters = new()
    //{
    //    ValidAudience = "BaseApi",
    //    ValidateAudience = true,
    //    ValidateIssuerSigningKey = true,
    //    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret")),
    //    ValidateIssuer = true,
    //};
});

builder.Services.AddOptions();
builder.Services.Authorization();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Vuejs");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<TokenMiddleware>();

app.MapControllers();

app.Run();
