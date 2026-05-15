using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services
{
    public class EnrollService(IEnrollRepository _enrollRepository) : IEnrollService
    {
        public List<EnrollModel> GetEnrolls()
        {
            List<EnrollModel> enrollments = _enrollRepository.GetEnrolls().Select(e => new EnrollModel()
            {
                EnrollmentId = e.EnrollmentId,
                Student = e.Student,
                CourseId = e.CourseId,
                EnrollDate = e.EnrollDate,
                Status = e.Status,
                Course = e.Course,
                Grades = e.Grades.ToList(),
                Subject = e.Grades.FirstOrDefault()?.Subject,
                Semester = e.Course.Semester
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
                .GetCollectionAsync(queryParams, searchFilter);

            var shaped = enrollments.ShapeData(queryParams.Fields);

            return new PagedResult<dynamic>
            {
                Items = shaped,
                TotalCount = totalCount,
                Page = queryParams.Page,
                Size = queryParams.Size
            };
        }
    }
}
