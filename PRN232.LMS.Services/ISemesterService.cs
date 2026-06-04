using PRN232.LMS.Services.Models;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.Services
{
    public interface ISemesterService
    {
        public SemesterModel GetSemesterById(int id);
        public Task<PagedResult<dynamic>> GetSemestersAsync(QueryParameters queryParams);
        public SemesterModel CreateSemester(SemesterModel model);
    }
}
