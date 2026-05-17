using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Extensions;
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
            string? expand,
            string? sort,
            int page,
            int size,
            Expression<Func<T, bool>>? searchFilter = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            query = query.ApplyExpansion(expand);

            if (searchFilter != null)
                query = query.Where(searchFilter);

            query = query.ApplySorting(sort);

            int totalCount = await query.CountAsync();
            var data = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return (data, totalCount);
        }
    }
}