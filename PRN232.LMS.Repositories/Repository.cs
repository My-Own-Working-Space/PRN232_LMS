using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Extensions;
using PRN232.LMS.Services.Common;
using System.Linq.Expressions;

namespace PRN232.LMS.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<(IEnumerable<T> Data, int TotalCount)> GetCollectionAsync(
            QueryParameters queryParams,
            Expression<Func<T, bool>>? searchFilter = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            query = query.ApplyExpansion(queryParams.Expand);

            if (searchFilter != null)
                query = query.Where(searchFilter);

            query = query.ApplySorting(queryParams.Sort);

            int totalCount = await query.CountAsync();
            var data = await query
                .Skip((queryParams.Page - 1) * queryParams.Size)
                .Take(queryParams.Size)
                .ToListAsync();

            return (data, totalCount);
        }
    }
}