using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.Services;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentController(IEnrollService _enrollService) : Controller
    {
        [HttpGet("{id}")]
        public IActionResult GetEnrollmentById(int id)
        {
            try
            {
                var enrollment = _enrollService.GetEnrollmentById(id);

                if (enrollment == null || enrollment.EnrollmentId == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Enrollment with ID {id} does not exist.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Request processed successfully",
                    Data = enrollment
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
        public async Task<IActionResult> GetEnrollments([FromQuery] QueryParameters queryParams)
        {
            var result = await _enrollService.GetEnrollmentsAsync(queryParams);
            if (result == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "No enrollments found",
                    Data = null
                });
            }
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Request processed successfully",
                Data = result
            });
        }

        [HttpPost]
        public IActionResult CreateEnrollment([FromBody] CreateEnrollmentRequest request)
        {
            try
            {
                var model = new EnrollModel
                {
                    StudentId = request.StudentId,
                    CourseId = request.CourseId,
                    EnrollDate = DateTime.Now,
                    Status = request.Status
                };
                var created = _enrollService.CreateEnrollment(model);
                return CreatedAtAction(nameof(GetEnrollmentById), new { id = created.EnrollmentId }, new ApiResponse<object>
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
