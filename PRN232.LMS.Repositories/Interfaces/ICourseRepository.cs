using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        public List<Course> GetCourses();
        public Course GetCourseById(int id);
        public void AddCourse(Course course);
    }
}
