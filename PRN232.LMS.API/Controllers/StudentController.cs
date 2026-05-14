using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.Services;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController(IStudentService _studentService) : Controller
    {
        [HttpGet]
        public IActionResult GetStudents()
        {
            try
            {
                var students = _studentService.GetStudents();

                if (students == null)
                {
                    return NotFound();
                }

                var studentResponse = new StudentResponse()
                {
                    success = true,
                    message = "Request processed successfully",
                    data = students,
                    errors = null

                };
                return Ok(studentResponse);


            }catch (Exception ex)
            {
                return BadRequest(new StudentResponse()
                {
                    success = false,
                    message = "An error occurred while processing the request",
                    data = null,
                    errors = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            try
            {
                var student = _studentService.GetStudentById(id);

                if (student == null)
                {
                    return NotFound(new StudentResponse()
                    {
                        success = false,
                        message = $"Student with ID {id} not found",
                        data = null
                    });
                }

                var studentResponse = new StudentResponse()
                {
                    success = true,
                    message = "Request processed successfully",
                    data = student,
                    errors = null
                };

                return Ok(studentResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new StudentResponse()
                {
                    success = false,
                    message = "An error occurred",
                    errors = ex.Message
                });
            }
        }
    }
}
