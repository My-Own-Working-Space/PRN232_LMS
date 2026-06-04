using PRN232.LMS.Services.Models;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.Services
{
    public interface IGradeService
    {
        public GradeModel GetGradeById(int id);
        public Task<PagedResult<dynamic>> GetGradesAsync(QueryParameters queryParams);
    }
}
