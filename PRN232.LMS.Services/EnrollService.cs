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
    }
}
