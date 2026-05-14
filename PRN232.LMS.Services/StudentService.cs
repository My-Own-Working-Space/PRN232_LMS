using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services
{
    public class StudentService(IStudentRepository _studentRepository) : IStudentService
    {
        public List<StudentModel> GetStudents()
        {
            List<StudentModel> studentModels = _studentRepository.GetStudents().Select(s => new StudentModel()
            {
                Id = s.StudentId,
                FullName = s.FullName,
                Email = s.Email,
                DateOfBirth = s.DateOfBirth
            }).ToList();

            return studentModels.OrderBy(s => s.Id).ToList();
        }

        public StudentModel GetStudentById(int id)
        {
            Student student = _studentRepository.GetStudentById(id);
            StudentModel studentModel = new StudentModel()
            {
                Id = student.StudentId,
                FullName = student.FullName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth
            };
            return studentModel;
        }
    }
}
