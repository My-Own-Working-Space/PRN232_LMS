using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories
{
    public class CourseSubjectRepository(LMSDatabaseContext _context) : Repository<CourseSubject>(_context), ICourseSubjectRepository
    {
        public List<CourseSubject> GetCourseSubjects()
        {
            return _context.CourseSubjects.ToList();
        }

        public CourseSubject GetCourseSubjectById(int id)
        {
            return _context.CourseSubjects.FirstOrDefault(cs => cs.CourseSubjectId == id) ?? new CourseSubject();
        }

        public void AddCourseSubject(CourseSubject courseSubject)
        {
            _context.CourseSubjects.Add(courseSubject);
            _context.SaveChanges();
        }
    }
}
