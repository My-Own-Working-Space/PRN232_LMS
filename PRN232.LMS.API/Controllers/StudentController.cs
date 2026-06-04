using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.Services;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController(IStudentService _studentService, IEnrollService _enrollService) : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            try
            {
                var student = _studentService.GetStudentById(id);

                if (student == null || student.Id == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Student with ID {id} does not exist.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Request processed successfully",
                    Data = student
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing the request",
                    Errors = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents([FromQuery] QueryParameters queryParams)
        {
            var result = await _studentService.GetStudentsAsync(queryParams);

            if (result == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "No students found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Students retrieved successfully",
                Data = result
            });
        }

        [HttpPost]
        public IActionResult CreateStudent([FromBody] CreateStudentRequest request)
        {
            try
            {
                var model = new StudentModel
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    DateOfBirth = request.DateOfBirth
                };
                var created = _studentService.CreateStudent(model);
                return CreatedAtAction(nameof(GetStudentById), new { id = created.Id }, new ApiResponse<object>
                {
                    Success = true,
                    Message = "Student created successfully",
                    Data = created
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while creating the student",
                    Errors = ex.Message
                });
            }
        }

        [HttpGet("{id}/enrollments")]
        public async Task<IActionResult> GetEnrollmentsByStudentId(int id, [FromQuery] QueryParameters queryParams)
        {
            try
            {
                var student = _studentService.GetStudentById(id);
                if (student == null || student.Id == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Student with ID {id} does not exist.",
                        Data = null
                    });
                }

                var result = await _enrollService.GetEnrollmentsByStudentIdAsync(id, queryParams);
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Enrollments retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing the request",
                    Errors = ex.Message
                });
            }
        }

        [HttpPost("{id}/enrollments")]
        public IActionResult CreateEnrollmentForStudent(int id, [FromBody] CreateEnrollmentRequest request)
        {
            try
            {
                var student = _studentService.GetStudentById(id);
                if (student == null || student.Id == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Student with ID {id} does not exist.",
                        Data = null
                    });
                }

                var model = new EnrollModel
                {
                    StudentId = id,
                    CourseId = request.CourseId,
                    EnrollDate = DateTime.Now,
                    Status = request.Status
                };
                var created = _enrollService.CreateEnrollment(model);
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Enrollment created successfully",
                    Data = created
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while creating the enrollment",
                    Errors = ex.Message
                });
            }
        }
    }
}
