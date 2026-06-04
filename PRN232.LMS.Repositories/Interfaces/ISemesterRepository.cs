using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories.Interfaces
{
    public interface ISemesterRepository : IRepository<Semester>
    {
        public List<Semester> GetSemesters();
        public Semester GetSemesterById(int id);
        public void AddSemester(Semester semester);
    }
}
