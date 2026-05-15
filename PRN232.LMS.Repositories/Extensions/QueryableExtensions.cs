using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyExpansion<T>(this IQueryable<T> query, string? expansion) where T : class
        {
            if (string.IsNullOrEmpty(expansion)) return query;
        
            foreach (var item in expansion.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(item.Trim());
            }
            return query;
        }

        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? sort) where T : class
        {
            if (string.IsNullOrEmpty(sort)) return query;

            // Simple manual sorting for core fields to avoid external dependencies
            var fields = sort.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var field in fields)
            {
                var trimmedField = field.Trim();
                bool descending = trimmedField.StartsWith("-");
                var propertyName = descending ? trimmedField.Substring(1) : trimmedField;

                // For a more 'pro' simplest way without Dynamic LINQ, we use a simple reflection-based OrderBy
                // or just handle key fields if it's a specific entity. 
                // For PRN232 Lab 1, a basic reflection approach is clean enough.
                var propertyInfo = typeof(T).GetProperty(propertyName, 
                    System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                if (propertyInfo == null) continue;

                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, propertyInfo);
                var lambda = Expression.Lambda(property, parameter);

                string methodName = descending ? "OrderByDescending" : "OrderBy";
                var resultExpression = Expression.Call(typeof(Queryable), methodName, 
                    new Type[] { typeof(T), propertyInfo.PropertyType },
                    query.Expression, Expression.Quote(lambda));

                query = query.Provider.CreateQuery<T>(resultExpression);
            }

            return query;
        }
    }
}