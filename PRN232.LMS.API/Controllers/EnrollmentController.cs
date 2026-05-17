using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.Services;
using PRN232.LMS.Services.Common;

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

                if (enrollment == null)
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
    }
}
