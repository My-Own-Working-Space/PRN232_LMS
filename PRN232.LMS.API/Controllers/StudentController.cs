using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.Services;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class StudentController(IStudentService _studentService) : ControllerBase
    {
        [HttpGet("all")]
        public IActionResult GetStudents()
        {
            try
            {
                var students = _studentService.GetStudents();
                if (students == null)
                {
                    return NotFound();
                }

                var studentResponse = new ApiResponse<object>()
                {
                    Success = true,
                    Message = "Request processed successfully",
                    Data = students,
                    Errors = null
                };
                return Ok(studentResponse);

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>()
                {
                    Success = false,
                    Message = "An error occurred while processing the request",
                    Data = null,
                    Errors = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents([FromQuery] QueryParameters queryParams)
        {
            var result = await _studentService.GetStudentsAsync(queryParams);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Students retrieved successfully",
                Data = result
            });
        }
    }
}
