using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232.LMS.Services.Models;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.Services.Interfaces
{
    public interface IStudentService
    {
        public List<StudentModel> GetStudents();
        public StudentModel GetStudentById(int id);
        public StudentModel GetStudentByCode(string code);
        Task<PagedResult<dynamic>> GetStudentsAsync(QueryParameters queryParams);
        public StudentModel CreateStudent(StudentModel model);
    }
}
