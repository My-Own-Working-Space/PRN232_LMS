using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Extensions;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;
using System.Linq.Expressions;

namespace PRN232.LMS.Services
{
    public class StudentService(IStudentRepository _studentRepository) : IStudentService
    {
        public List<StudentModel> GetStudents()
        {
            List<StudentModel> studentModels = _studentRepository.GetStudents().Select(s => new StudentModel()
            {
                Id = s.StudentId,
                StudentCode = s.StudentCode,
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
                StudentCode = student.StudentCode,
                FullName = student.FullName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth
            };
            return studentModel;
        }

        public async Task<PagedResult<dynamic>> GetStudentsAsync(QueryParameters queryParams)
        {
            Expression<Func<Student, bool>>? searchFilter = null;
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                searchFilter = s =>
                    s.FullName.Contains(queryParams.Search) ||
                    s.Email.Contains(queryParams.Search) ||
                    s.StudentCode.Contains(queryParams.Search);
            }

            var (students, totalCount) = await _studentRepository
                .GetCollectionAsync(queryParams.Expand, queryParams.Sort, queryParams.Page, queryParams.Size, searchFilter);

            var businessModels = students.Select(s => new StudentModel
            {
                Id = s.StudentId,
                StudentCode = s.StudentCode,
                FullName = s.FullName,
                Email = s.Email,
                DateOfBirth = s.DateOfBirth
            }).ToList();

            var shaped = businessModels.ShapeData(queryParams.Fields);

            return new PagedResult<dynamic>
            {
                Items = shaped,
                Pagination = new PaginationMetadata
                {
                    Page = queryParams.Page,
                    PageSize = queryParams.Size,
                    TotalItems = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)queryParams.Size)
                }
            };
        }

        public StudentModel CreateStudent(StudentModel model)
        {
            // Check if email already exists in the database
            var emailExists = _studentRepository.GetStudents().Any(s => s.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase));
            if (emailExists)
            {
                throw new InvalidOperationException($"Email '{model.Email}' is already registered.");
            }

            // Check if StudentCode already exists in the database
            var codeExists = _studentRepository.GetStudents().Any(s => s.StudentCode.Equals(model.StudentCode, StringComparison.OrdinalIgnoreCase));
            if (codeExists)
            {
                throw new InvalidOperationException($"Student Code '{model.StudentCode}' is already registered.");
            }

            var student = new Student
            {
                StudentCode = model.StudentCode,
                FullName = model.FullName,
                Email = model.Email,
                DateOfBirth = model.DateOfBirth
            };

            _studentRepository.AddStudent(student);

            model.Id = student.StudentId;
            return model;
        }

        public StudentModel GetStudentByCode(string code)
        {
            var student = _studentRepository.GetStudents()
                .FirstOrDefault(s => s.StudentCode.Equals(code, StringComparison.OrdinalIgnoreCase));
            if (student == null) return null;
            return new StudentModel
            {
                Id = student.StudentId,
                StudentCode = student.StudentCode,
                FullName = student.FullName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth
            };
        }
    }
}
