using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories
{
    public class SubjectRepository(LMSDatabaseContext _context) : Repository<Subject>(_context), ISubjectRepository
    {
        public List<Subject> GetSubjects()
        {
            return _context.Subjects.ToList();
        }

        public Subject GetSubjectById(int id)
        {
            return _context.Subjects.FirstOrDefault(s => s.SubjectId == id) ?? new Subject();
        }

        public void AddSubject(Subject subject)
        {
            _context.Subjects.Add(subject);
            _context.SaveChanges();
        }
    }
}
