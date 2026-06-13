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
    [Route("api/v{version:apiVersion}/courses")]
    [Route("api/courses")]
    [Asp.Versioning.ApiVersion("1.0")]
    public class CourseController(ICourseService _courseService, IEnrollService _enrollService, ICourseSubjectService _courseSubjectService) : ControllerBase
    {
        [HttpGet("{id:int}/enrollments")]
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

        [HttpPost("{id:int}/enrollments")]
        public IActionResult CreateEnrollmentForCourse(int id, [FromBody] CreateEnrollmentRequest request)
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

                var model = new EnrollModel
                {
                    CourseId = id,
                    StudentId = request.StudentId,
                    EnrollDate = DateTime.Now,
                    Status = request.Status
                };
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

        [HttpGet("/api/v{version:apiVersion}/courses/{id:int}", Name = "GetCourseById")]
        [HttpGet("/api/courses/{id:int}")]
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
        public IActionResult CreateCourse([FromBody] CreateCourseRequest request)
        {
            try
            {
                var model = new CourseModel
                {
                    CourseName = request.CourseName,
                    SemesterId = request.SemesterId
                };
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
        public IActionResult CreateCourseForSemester(int semesterId, [FromBody] CreateCourseRequest request)
        {
            try
            {
                var model = new CourseModel
                {
                    CourseName = request.CourseName,
                    SemesterId = semesterId
                };
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

        [HttpGet("{id:int}/subjects")]
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

        [HttpPost("{id:int}/subjects")]
        public IActionResult CreateCourseSubjectForCourse(int id, [FromBody] CreateCourseSubjectRequest request)
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

                var model = new CourseSubjectModel
                {
                    CourseId = id,
                    SubjectId = request.SubjectId
                };
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

        [HttpGet("{courseId:int}/students")]
        public async Task<IActionResult> GetStudentsByCourseId(int courseId, [FromQuery] QueryParameters queryParams)
        {
            try
            {
                var course = _courseService.GetCourseById(courseId);
                if (course == null || course.CourseId == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Course with ID {courseId} does not exist.",
                        Data = null
                    });
                }

                var result = await _enrollService.GetStudentsByCourseIdAsync(courseId, queryParams);
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Students retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving students",
                    Errors = ex.Message
                });
            }
        }
    }
}
