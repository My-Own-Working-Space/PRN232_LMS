using PRN232.LMS.Repositories.Interfaces;
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

        public async Task<PagedResult<dynamic>> GetStudentsAsync(QueryParameters queryParams)
        {
            Expression<Func<Student, bool>>? searchFilter = null;
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                searchFilter = s =>
                    s.FullName.Contains(queryParams.Search) ||
                    s.Email.Contains(queryParams.Search);
            }

            var (students, totalCount) = await _studentRepository
                .GetCollectionAsync(queryParams.Expand, queryParams.Sort, queryParams.Page, queryParams.Size, searchFilter);

            var businessModels = students.Select(s => new StudentModel
            {
                Id = s.StudentId,
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
        
    }
}
