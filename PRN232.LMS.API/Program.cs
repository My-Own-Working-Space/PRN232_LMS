using PRN232.LMS.Services;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using PRN232.LMS.API.Middlewares;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.ReturnHttpNotAcceptable = true;
    options.OutputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerOutputFormatter>();
    options.InputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerInputFormatter>();
})
.AddXmlDataContractSerializerFormatters()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"));
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "LMS API v1", Version = "v1" });
    opt.SwaggerDoc("v2", new OpenApiInfo { Title = "LMS API v2", Version = "v2" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

var connectionStr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSqlServer<PRN232.LMS.Repositories.LMSDatabaseContext>(connectionStr);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });
    
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddScoped<PRN232.LMS.Repositories.Interfaces.IStudentRepository, PRN232.LMS.Repositories.StudentRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<PRN232.LMS.Repositories.Interfaces.IEnrollRepository, PRN232.LMS.Repositories.EnrollRepository>();
builder.Services.AddScoped<PRN232.LMS.Repositories.Interfaces.ICourseRepository, PRN232.LMS.Repositories.CourseRepository>();
builder.Services.AddScoped<PRN232.LMS.Repositories.Interfaces.ICourseSubjectRepository, PRN232.LMS.Repositories.CourseSubjectRepository>();
builder.Services.AddScoped<PRN232.LMS.Repositories.Interfaces.IGradeRepository, PRN232.LMS.Repositories.GradeRepository>();
builder.Services.AddScoped<PRN232.LMS.Repositories.Interfaces.ISemesterRepository, PRN232.LMS.Repositories.SemesterRepository>();
builder.Services.AddScoped<PRN232.LMS.Repositories.Interfaces.ISubjectRepository, PRN232.LMS.Repositories.SubjectRepository>();
builder.Services.AddScoped<PRN232.LMS.Repositories.Interfaces.IUserRepository, PRN232.LMS.Repositories.UserRepository>();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IEnrollService, EnrollService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseSubjectService, CourseSubjectService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddValidatorsFromAssemblyContaining<PRN232.LMS.API.Validators.CreateSubjectRequestValidator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "LMS API v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "LMS API v2");
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed Admin User
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PRN232.LMS.Repositories.LMSDatabaseContext>();
    context.Database.Migrate();
    if (!context.Users.Any(u => u.Username == "admin"))
    {
        context.Users.Add(new PRN232.LMS.Repositories.Models.User
        {
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            Role = "Admin"
        });
        context.SaveChanges();
    }
}

app.Run();
