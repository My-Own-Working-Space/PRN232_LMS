using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories.Interfaces
{
    public interface ICourseSubjectRepository : IRepository<CourseSubject>
    {
        public List<CourseSubject> GetCourseSubjects();
        public CourseSubject GetCourseSubjectById(int id);
        public void AddCourseSubject(CourseSubject courseSubject);
    }
}
