using PRN232.LMS.Services.Common;
using System.Linq.Expressions;

namespace PRN232.LMS.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<(IEnumerable<T> Data, int TotalCount)> GetCollectionAsync(
            QueryParameters queryParams,
            Expression<Func<T, bool>>? searchFilter = null);
    }
}