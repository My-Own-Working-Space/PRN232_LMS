using PRN232.LMS.Services.Models;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.Services.Interfaces
{
    public interface ICourseService
    {
        public CourseModel GetCourseById(int id);
        public Task<PagedResult<dynamic>> GetCoursesAsync(QueryParameters queryParams);
        public CourseModel CreateCourse(CourseModel model);
        public Task<PagedResult<dynamic>> GetCoursesBySemesterIdAsync(int semesterId, QueryParameters queryParams);
    }
}
