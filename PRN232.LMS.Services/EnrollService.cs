using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Extensions;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;
using System.Linq.Expressions;

namespace PRN232.LMS.Services
{
    public class EnrollService(IEnrollRepository _enrollmentRepository) : IEnrollService
    {
        public List<EnrollModel> GetEnrolls()
        {
            List<EnrollModel> enrollments = _enrollmentRepository.GetEnrolls().Select(e => new EnrollModel()
            {
                EnrollmentId = e.EnrollmentId,
                CourseId = e.CourseId,
                StudentId = e.StudentId,
                EnrollDate = e.EnrollDate,
                Status = e.Status
            }).ToList();

            return enrollments;
        }

        public async Task<PagedResult<dynamic>> GetEnrollmentsAsync(QueryParameters queryParams)
        {
            Expression<Func<Enrollment, bool>>? searchFilter = null;
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                searchFilter = e => e.Status.Contains(queryParams.Search);
            }

            var (enrollments, totalCount) = await _enrollmentRepository
                .GetCollectionAsync(queryParams.Expand, queryParams.Sort, queryParams.Page, queryParams.Size, searchFilter);

            var shaped = enrollments.ShapeData(queryParams.Fields);

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
