using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories.Interfaces
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        public List<Subject> GetSubjects();
        public Subject GetSubjectById(int id);
        public void AddSubject(Subject subject);
    }
}
