using System.Linq.Expressions;

namespace PRN232.LMS.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<(IEnumerable<T> Data, int TotalCount)> GetCollectionAsync(
            string? expand,
            string? sort,
            int page,
            int size,
            Expression<Func<T, bool>>? searchFilter = null);
    }
}