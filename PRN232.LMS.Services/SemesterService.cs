using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Extensions;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;
using System.Linq.Expressions;

namespace PRN232.LMS.Services
{
    public class SemesterService(ISemesterRepository _semesterRepository) : ISemesterService
    {
        public SemesterModel GetSemesterById(int id)
        {
            Semester s = _semesterRepository.GetSemesterById(id);
            return new SemesterModel
            {
                SemesterId = s.SemesterId,
                SemesterName = s.SemesterName,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Courses = s.Courses?.Select(c => new CourseModel
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    SemesterId = c.SemesterId
                }).ToList()
            };
        }

        public async Task<PagedResult<dynamic>> GetSemestersAsync(QueryParameters queryParams)
        {
            Expression<Func<Semester, bool>>? searchFilter = null;
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                searchFilter = s => s.SemesterName.Contains(queryParams.Search);
            }

            var (semesters, totalCount) = await _semesterRepository
                .GetCollectionAsync(queryParams.Expand, queryParams.Sort, queryParams.Page, queryParams.Size, searchFilter);

            var businessModels = semesters.Select(s => new SemesterModel
            {
                SemesterId = s.SemesterId,
                SemesterName = s.SemesterName,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Courses = s.Courses?.Select(c => new CourseModel
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    SemesterId = c.SemesterId
                }).ToList()
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

        public SemesterModel CreateSemester(SemesterModel model)
        {
            var semester = new Semester
            {
                SemesterName = model.SemesterName,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            _semesterRepository.AddSemester(semester);

            model.SemesterId = semester.SemesterId;
            return model;
        }
    }
}
