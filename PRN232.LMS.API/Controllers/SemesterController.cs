using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.Services;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/semesters")]
    [Route("api/semesters")]
    [Asp.Versioning.ApiVersion("1.0")]
    public class SemesterController(ISemesterService _semesterService) : ControllerBase
    {
        [HttpGet("/api/v{version:apiVersion}/semesters/{id:int}", Name = "GetSemesterById")]
        [HttpGet("/api/semesters/{id:int}")]
        public IActionResult GetSemesterById(int id)
        {
            try
            {
                var semester = _semesterService.GetSemesterById(id);

                if (semester == null || semester.SemesterId == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Semester with ID {id} does not exist.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Request processed successfully",
                    Data = semester
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
        public async Task<IActionResult> GetSemesters([FromQuery] QueryParameters queryParams)
        {
            var result = await _semesterService.GetSemestersAsync(queryParams);

            if (result == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "No semesters found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Semesters retrieved successfully",
                Data = result
            });
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public IActionResult CreateSemester([FromBody] CreateSemesterRequest request)
        {
            try
            {
                var model = new SemesterModel
                {
                    SemesterName = request.SemesterName,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate
                };
                var created = _semesterService.CreateSemester(model);
                return CreatedAtAction(nameof(GetSemesterById), new { id = created.SemesterId }, new ApiResponse<object>
                {
                    Success = true,
                    Message = "Semester created successfully",
                    Data = created
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while creating the semester",
                    Errors = ex.Message
                });
            }
        }
    }
}
