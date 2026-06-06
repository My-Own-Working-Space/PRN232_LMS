using PRN232.LMS.Services.Models;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.Services.Interfaces
{
    public interface ICourseSubjectService
    {
        public CourseSubjectModel GetCourseSubjectById(int id);
        public Task<PagedResult<dynamic>> GetCourseSubjectsAsync(QueryParameters queryParams);
        public Task<PagedResult<dynamic>> GetSubjectsByCourseIdAsync(int courseId, QueryParameters queryParams);
        public CourseSubjectModel CreateCourseSubject(CourseSubjectModel model);
    }
}
