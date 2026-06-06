using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.Services;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/grades")]
    public class GradeController(IGradeService _gradeService) : ControllerBase
    {
        [HttpGet("{id:int}", Name = "GetGradeById")]
        public IActionResult GetGradeById(int id)
        {
            try
            {
                var grade = _gradeService.GetGradeById(id);

                if (grade == null || grade.GradeId == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Grade with ID {id} does not exist.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Request processed successfully",
                    Data = grade
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
        public async Task<IActionResult> GetGrades([FromQuery] QueryParameters queryParams)
        {
            var result = await _gradeService.GetGradesAsync(queryParams);

            if (result == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "No grades found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Grades retrieved successfully",
                Data = result
            });
        }
    }
}
