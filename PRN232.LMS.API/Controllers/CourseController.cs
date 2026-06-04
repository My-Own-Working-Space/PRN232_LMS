using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.Services;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CourseController(ICourseService _courseService, IEnrollService _enrollService, ICourseSubjectService _courseSubjectService) : ControllerBase
    {
        [HttpGet("{id}/enrollments")]
        public async Task<IActionResult> GetEnrollmentsByCourseId(int id, [FromQuery] QueryParameters queryParams)
        {
            try
            {
                var course = _courseService.GetCourseById(id);

                if (course == null || course.CourseId == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Course with ID {id} does not exist.",
                        Data = null
                    });
                }

                var result = await _enrollService.GetEnrollmentsByCourseIdAsync(id, queryParams);

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
        public IActionResult CreateEnrollmentForCourse(int id, [FromBody] EnrollModel model)
        {
            try
            {
                var course = _courseService.GetCourseById(id);
                if (course == null || course.CourseId == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Course with ID {id} does not exist.",
                        Data = null
                    });
                }

                model.CourseId = id;
                var created = _enrollService.CreateEnrollment(model);
                return CreatedAtAction(nameof(GetCourseById), new { id = id }, new ApiResponse<object>
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

        [HttpGet("{id}")]
        public IActionResult GetCourseById(int id)
        {
            try
            {
                var course = _courseService.GetCourseById(id);

                if (course == null || course.CourseId == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Course with ID {id} does not exist.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Request processed successfully",
                    Data = course
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
        public async Task<IActionResult> GetCourses([FromQuery] QueryParameters queryParams)
        {
            var result = await _courseService.GetCoursesAsync(queryParams);

            if (result == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "No courses found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Courses retrieved successfully",
                Data = result
            });
        }

        [HttpPost]
        public IActionResult CreateCourse([FromBody] CourseModel model)
        {
            try
            {
                var created = _courseService.CreateCourse(model);
                return CreatedAtAction(nameof(GetCourseById), new { id = created.CourseId }, new ApiResponse<object>
                {
                    Success = true,
                    Message = "Course created successfully",
                    Data = created
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while creating the course",
                    Errors = ex.Message
                });
            }
        }

        [HttpPost("~/api/semesters/{semesterId}/courses")]
        public IActionResult CreateCourseForSemester(int semesterId, [FromBody] CourseModel model)
        {
            try
            {
                model.SemesterId = semesterId;
                var created = _courseService.CreateCourse(model);
                
                return CreatedAtAction(nameof(GetCourseById), new { id = created.CourseId }, new ApiResponse<object>
                {
                    Success = true,
                    Message = "Course created successfully",
                    Data = created
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while creating the course",
                    Errors = ex.Message
                });
            }
        }

        [HttpGet("~/api/semesters/{semesterId}/courses")]
        public async Task<IActionResult> GetCoursesBySemesterId(int semesterId, [FromQuery] QueryParameters queryParams)
        {
            try
            {
                var result = await _courseService.GetCoursesBySemesterIdAsync(semesterId, queryParams);
                
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"No courses found for semester with ID {semesterId}",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Courses retrieved successfully",
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

        [HttpGet("{id}/subjects")]
        public async Task<IActionResult> GetSubjectsByCourseId(int id, [FromQuery] QueryParameters queryParams)
        {
            try
            {
                var course = _courseService.GetCourseById(id);
                if (course == null || course.CourseId == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Course with ID {id} does not exist.",
                        Data = null
                    });
                }

                var result = await _courseSubjectService.GetSubjectsByCourseIdAsync(id, queryParams);
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Subjects retrieved successfully",
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

        [HttpPost("{id}/subjects")]
        public IActionResult CreateCourseSubjectForCourse(int id, [FromBody] CourseSubjectModel model)
        {
            try
            {
                var course = _courseService.GetCourseById(id);
                if (course == null || course.CourseId == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Course with ID {id} does not exist.",
                        Data = null
                    });
                }

                model.CourseId = id;
                var created = _courseSubjectService.CreateCourseSubject(model);
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Subject linked to course successfully",
                    Data = created
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while linking the subject to the course",
                    Errors = ex.Message
                });
            }
        }
    }
}
