using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Extensions;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;
using System.Linq.Expressions;

namespace PRN232.LMS.Services
{
    public class CourseService(ICourseRepository _courseRepository) : ICourseService
    {
        public CourseModel GetCourseById(int id)
        {
            Course course = _courseRepository.GetCourseById(id);
            return new CourseModel
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                SemesterId = course.SemesterId
            };
        }

        public async Task<PagedResult<dynamic>> GetCoursesAsync(QueryParameters queryParams)
        {
            Expression<Func<Course, bool>>? searchFilter = null;
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                searchFilter = c => c.CourseName.Contains(queryParams.Search);
            }

            var (courses, totalCount) = await _courseRepository
                .GetCollectionAsync(queryParams.Expand, queryParams.Sort, queryParams.Page, queryParams.Size, searchFilter);

            var businessModels = courses.Select(c => new CourseModel
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                SemesterId = c.SemesterId
            }).ToList();

            var shaped = businessModels.ShapeData(queryParams.Fields);

            return new PagedResult<dynamic>
            {
                Items = shaped,
                Pagination = new PaginationMetadata
                {
                    Page = queryParams.Page,
                    PageSize = queryParams.Size,
                    TotalItems = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)queryParams.Size)
                }
            };
        }

        public CourseModel CreateCourse(CourseModel model)
        {
            var course = new Course
            {
                CourseName = model.CourseName,
                SemesterId = model.SemesterId
            };

            _courseRepository.AddCourse(course);

            model.CourseId = course.CourseId;
            return model;
        }

        public async Task<PagedResult<dynamic>> GetCoursesBySemesterIdAsync(int semesterId, QueryParameters queryParams)
        {
            Expression<Func<Course, bool>> searchFilter = c => c.SemesterId == semesterId;
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                searchFilter = c => c.SemesterId == semesterId && c.CourseName.Contains(queryParams.Search);
            }

            var (courses, totalCount) = await _courseRepository
                .GetCollectionAsync(queryParams.Expand, queryParams.Sort, queryParams.Page, queryParams.Size, searchFilter);

            var businessModels = courses.Select(c => new CourseModel
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                SemesterId = c.SemesterId
            }).ToList();

            var shaped = businessModels.ShapeData(queryParams.Fields);

            return new PagedResult<dynamic>
            {
                Items = shaped,
                Pagination = new PaginationMetadata
                {
                    Page = queryParams.Page,
                    PageSize = queryParams.Size,
                    TotalItems = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)queryParams.Size)
                }
            };
        }
    }
}
