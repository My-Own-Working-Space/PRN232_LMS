using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace PRN232.LMS.Repositories
{
    public class SemesterRepository(LMSDatabaseContext _context) : Repository<Semester>(_context), ISemesterRepository
    {
        public List<Semester> GetSemesters()
        {
            return _context.Semesters.ToList();
        }

        public Semester GetSemesterById(int id)
        {
            return _context.Semesters
                .Include(s => s.Courses)
                .FirstOrDefault(s => s.SemesterId == id) ?? new Semester();
        }

        public void AddSemester(Semester semester)
        {
            _context.Semesters.Add(semester);
            _context.SaveChanges();
        }
    }
}
