using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        public List<Student> GetStudents();
        public Student GetStudentById(int id);
        public void AddStudent(Student student);
    }
}
