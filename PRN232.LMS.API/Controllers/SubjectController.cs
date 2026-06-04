using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.Services;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/subjects")]
    public class SubjectController(ISubjectService _subjectService) : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetSubjectById(int id)
        {
            try
            {
                var subject = _subjectService.GetSubjectById(id);

                if (subject == null || subject.SubjectId == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Subject with ID {id} does not exist.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Request processed successfully",
                    Data = subject
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
        public async Task<IActionResult> GetSubjects([FromQuery] QueryParameters queryParams)
        {
            var result = await _subjectService.GetSubjectsAsync(queryParams);

            if (result == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "No subjects found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Subjects retrieved successfully",
                Data = result
            });
        }

        [HttpPost]
        public IActionResult CreateSubject([FromBody] SubjectModel model)
        {
            try
            {
                var created = _subjectService.CreateSubject(model);
                return CreatedAtAction(nameof(GetSubjectById), new { id = created.SubjectId }, new ApiResponse<object>
                {
                    Success = true,
                    Message = "Subject created successfully",
                    Data = created
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while creating the subject",
                    Errors = ex.Message
                });
            }
        }
    }
}
