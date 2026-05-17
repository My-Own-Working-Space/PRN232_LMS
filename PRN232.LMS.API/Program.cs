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

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IEnrollService, EnrollService>();

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
