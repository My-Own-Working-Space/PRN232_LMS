using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232.LMS.Services.Models;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.Services
{
    public interface IStudentService
    {
        public List<StudentModel> GetStudents();
        public StudentModel GetStudentById(int id);
        Task<PagedResult<dynamic>> GetStudentsAsync(QueryParameters queryParams);
        public StudentModel CreateStudent(StudentModel model);
    }
}
