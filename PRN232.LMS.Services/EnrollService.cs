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
        public EnrollModel GetEnrollmentById(int id)
        {
            var enrollment = _enrollmentRepository.GetEnrollmentById(id);

            EnrollModel enrollModel = new EnrollModel
            {
                EnrollmentId = enrollment.EnrollmentId,
                CourseId = enrollment.CourseId,
                StudentId = enrollment.StudentId,
                EnrollDate = enrollment.EnrollDate,
                Status = enrollment.Status
            };

            return enrollModel;

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
