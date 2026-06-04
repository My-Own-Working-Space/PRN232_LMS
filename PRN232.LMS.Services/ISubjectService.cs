using PRN232.LMS.Services.Models;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.Services
{
    public interface ISubjectService
    {
        public SubjectModel GetSubjectById(int id);
        public Task<PagedResult<dynamic>> GetSubjectsAsync(QueryParameters queryParams);
        public SubjectModel CreateSubject(SubjectModel model);
    }
}
