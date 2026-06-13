using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Interfaces;
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

            var mapped = enrollments.Select(e => new EnrollModel
            {
                EnrollmentId = e.EnrollmentId,
                CourseId = e.CourseId,
                StudentId = e.StudentId,
                EnrollDate = e.EnrollDate,
                Status = e.Status
            }).ToList();

            var shaped = mapped.ShapeData(queryParams.Fields);

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

        public async Task<PagedResult<dynamic>> GetEnrollmentsByCourseIdAsync(int courseId, QueryParameters queryParams)
        {
            Expression<Func<Enrollment, bool>> searchFilter = e => e.CourseId == courseId;
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                searchFilter = e => e.CourseId == courseId && e.Status.Contains(queryParams.Search);
            }

            var (enrollments, totalCount) = await _enrollmentRepository
                .GetCollectionAsync(queryParams.Expand, queryParams.Sort, queryParams.Page, queryParams.Size, searchFilter);

            var mapped = enrollments.Select(e => new EnrollModel
            {
                EnrollmentId = e.EnrollmentId,
                CourseId = e.CourseId,
                StudentId = e.StudentId,
                EnrollDate = e.EnrollDate,
                Status = e.Status
            }).ToList();

            var shaped = mapped.ShapeData(queryParams.Fields);

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

        public async Task<PagedResult<dynamic>> GetStudentsByCourseIdAsync(int courseId, QueryParameters queryParams)
        {
            Expression<Func<Enrollment, bool>> searchFilter = e => e.CourseId == courseId;
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                searchFilter = e => e.CourseId == courseId && 
                    (e.Student.FullName.Contains(queryParams.Search) || e.Student.Email.Contains(queryParams.Search));
            }

            var (enrollments, totalCount) = await _enrollmentRepository
                .GetCollectionAsync("Student", queryParams.Sort, queryParams.Page, queryParams.Size, searchFilter);

            var students = enrollments.Select(e => new StudentModel
            {
                Id = e.Student.StudentId,
                FullName = e.Student.FullName,
                Email = e.Student.Email,
                DateOfBirth = e.Student.DateOfBirth
            }).ToList();

            var shaped = students.ShapeData(queryParams.Fields);

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

        public async Task<PagedResult<dynamic>> GetEnrollmentsByStudentIdAsync(int studentId, QueryParameters queryParams)
        {
            Expression<Func<Enrollment, bool>> searchFilter = e => e.StudentId == studentId;
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                searchFilter = e => e.StudentId == studentId && e.Status.Contains(queryParams.Search);
            }

            var (enrollments, totalCount) = await _enrollmentRepository
                .GetCollectionAsync(queryParams.Expand, queryParams.Sort, queryParams.Page, queryParams.Size, searchFilter);

            var mapped = enrollments.Select(e => new EnrollModel
            {
                EnrollmentId = e.EnrollmentId,
                CourseId = e.CourseId,
                StudentId = e.StudentId,
                EnrollDate = e.EnrollDate,
                Status = e.Status
            }).ToList();

            var shaped = mapped.ShapeData(queryParams.Fields);

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

        public EnrollModel CreateEnrollment(EnrollModel model)
        {
            var exists = _enrollmentRepository.GetEnrolls().Any(e => e.StudentId == model.StudentId && e.CourseId == model.CourseId);
            if (exists)
            {
                throw new InvalidOperationException($"Student with ID {model.StudentId} is already enrolled in Course with ID {model.CourseId}.");
            }

            var enrollment = new Enrollment
            {
                StudentId = model.StudentId,
                CourseId = model.CourseId,
                EnrollDate = model.EnrollDate == default ? DateTime.Now : model.EnrollDate,
                Status = model.Status ?? "Active"
            };

            _enrollmentRepository.AddEnrollment(enrollment);

            model.EnrollmentId = enrollment.EnrollmentId;
            return model;
        }
    }
}
