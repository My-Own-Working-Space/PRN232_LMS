using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.Services;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/courseSubjects")]
    [Asp.Versioning.ApiVersion("1.0")]
    public class CourseSubjectController(ICourseSubjectService _courseSubjectService) : ControllerBase
    {
        [HttpGet("{id:int}", Name = "GetCourseSubjectById")]
        public IActionResult GetCourseSubjectById(int id)
        {
            try
            {
                var courseSubject = _courseSubjectService.GetCourseSubjectById(id);

                if (courseSubject == null || courseSubject.CourseSubjectId == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"CourseSubject with ID {id} does not exist.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Request processed successfully",
                    Data = courseSubject
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
        public async Task<IActionResult> GetCourseSubjects([FromQuery] QueryParameters queryParams)
        {
            var result = await _courseSubjectService.GetCourseSubjectsAsync(queryParams);

            if (result == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "No course subjects found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Course subjects retrieved successfully",
                Data = result
            });
        }
    }
}
