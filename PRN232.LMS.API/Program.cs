using PRN232.LMS.Services;
using PRN232.LMS.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionStr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSqlServer<PRN232.LMS.Repositories.LMSDatabaseContext>(connectionStr);
builder.Services.AddScoped<PRN232.LMS.Repositories.Interfaces.IStudentRepository, PRN232.LMS.Repositories.StudentRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<PRN232.LMS.Repositories.Interfaces.IEnrollRepository, PRN232.LMS.Repositories.EnrollRepository>();
builder.Services.AddScoped<PRN232.LMS.Repositories.Interfaces.ICourseRepository, PRN232.LMS.Repositories.CourseRepository>();
builder.Services.AddScoped<PRN232.LMS.Repositories.Interfaces.ICourseSubjectRepository, PRN232.LMS.Repositories.CourseSubjectRepository>();
builder.Services.AddScoped<PRN232.LMS.Repositories.Interfaces.IGradeRepository, PRN232.LMS.Repositories.GradeRepository>();
builder.Services.AddScoped<PRN232.LMS.Repositories.Interfaces.ISemesterRepository, PRN232.LMS.Repositories.SemesterRepository>();
builder.Services.AddScoped<PRN232.LMS.Repositories.Interfaces.ISubjectRepository, PRN232.LMS.Repositories.SubjectRepository>();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IEnrollService, EnrollService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseSubjectService, CourseSubjectService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
