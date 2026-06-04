using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories
{
    public class GradeRepository(LMSDatabaseContext _context) : Repository<Grade>(_context), IGradeRepository
    {
        public List<Grade> GetGrades()
        {
            return _context.Grades.ToList();
        }

        public Grade GetGradeById(int id)
        {
            return _context.Grades.FirstOrDefault(g => g.GradeId == id) ?? new Grade();
        }
    }
}
