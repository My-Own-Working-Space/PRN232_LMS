using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Extensions;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;
using System.Linq.Expressions;

namespace PRN232.LMS.Services
{
    public class CourseSubjectService(ICourseSubjectRepository _courseSubjectRepository) : ICourseSubjectService
    {
        public CourseSubjectModel GetCourseSubjectById(int id)
        {
            CourseSubject cs = _courseSubjectRepository.GetCourseSubjectById(id);
            return new CourseSubjectModel
            {
                CourseSubjectId = cs.CourseSubjectId,
                CourseId = cs.CourseId,
                SubjectId = cs.SubjectId
            };
        }

        public async Task<PagedResult<dynamic>> GetCourseSubjectsAsync(QueryParameters queryParams)
        {
            Expression<Func<CourseSubject, bool>>? searchFilter = null;
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                searchFilter = cs => cs.Course.CourseName.Contains(queryParams.Search) ||
                                     cs.Subject.SubjectName.Contains(queryParams.Search) ||
                                     cs.Subject.SubjectCode.Contains(queryParams.Search);
            }

            var (courseSubjects, totalCount) = await _courseSubjectRepository
                .GetCollectionAsync(queryParams.Expand, queryParams.Sort, queryParams.Page, queryParams.Size, searchFilter);

            var businessModels = courseSubjects.Select(cs => new CourseSubjectModel
            {
                CourseSubjectId = cs.CourseSubjectId,
                CourseId = cs.CourseId,
                SubjectId = cs.SubjectId
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

        public async Task<PagedResult<dynamic>> GetSubjectsByCourseIdAsync(int courseId, QueryParameters queryParams)
        {
            Expression<Func<CourseSubject, bool>> searchFilter = cs => cs.CourseId == courseId;
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                searchFilter = cs => cs.CourseId == courseId && (cs.Subject.SubjectName.Contains(queryParams.Search) || cs.Subject.SubjectCode.Contains(queryParams.Search));
            }

            var (courseSubjects, totalCount) = await _courseSubjectRepository
                .GetCollectionAsync(queryParams.Expand, queryParams.Sort, queryParams.Page, queryParams.Size, searchFilter);

            var businessModels = courseSubjects.Select(cs => new CourseSubjectModel
            {
                CourseSubjectId = cs.CourseSubjectId,
                CourseId = cs.CourseId,
                SubjectId = cs.SubjectId
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

        public CourseSubjectModel CreateCourseSubject(CourseSubjectModel model)
        {
            var exists = _courseSubjectRepository.GetCourseSubjects().Any(cs => cs.CourseId == model.CourseId && cs.SubjectId == model.SubjectId);
            if (exists)
            {
                throw new InvalidOperationException($"Subject with ID {model.SubjectId} is already linked to Course with ID {model.CourseId}.");
            }

            var courseSubject = new CourseSubject
            {
                CourseId = model.CourseId,
                SubjectId = model.SubjectId
            };

            _courseSubjectRepository.AddCourseSubject(courseSubject);

            model.CourseSubjectId = courseSubject.CourseSubjectId;
            return model;
        }
    }
}
