using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232.LMS.Services.Models;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.Services
{
    public interface IEnrollService
    {
        public List<EnrollModel> GetEnrolls();
        Task<PagedResult<dynamic>> GetEnrollmentsAsync(QueryParameters queryParams);

    }
}
