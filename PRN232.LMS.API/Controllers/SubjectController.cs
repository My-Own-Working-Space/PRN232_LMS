using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.Services;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;
using FluentValidation;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/subjects")]
    [Route("api/subjects")]
    [Asp.Versioning.ApiVersion("1.0")]
    public class SubjectController(ISubjectService _subjectService) : ControllerBase
    {
        [HttpGet("/api/v{version:apiVersion}/subjects/{id:int}", Name = "GetSubjectById")]
        [HttpGet("/api/subjects/{id:int}")]
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
        public async Task<IActionResult> GetSubjects([FromQuery] QueryParameters queryParams, [FromHeader(Name = "X-Request-Id")] string? requestId)
        {
            if (!string.IsNullOrEmpty(requestId))
            {
                Response.Headers.Append("X-Request-Id", requestId);
            }
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
        public IActionResult CreateSubject([FromBody] CreateSubjectRequest request, [FromServices] IValidator<CreateSubjectRequest> validator)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }

            try
            {
                var model = new SubjectModel
                {
                    SubjectCode = request.SubjectCode,
                    SubjectName = request.SubjectName,
                    Credit = request.Credit
                };
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
