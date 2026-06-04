using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories.Interfaces
{
    public interface IGradeRepository : IRepository<Grade>
    {
        public List<Grade> GetGrades();
        public Grade GetGradeById(int id);
    }
}
