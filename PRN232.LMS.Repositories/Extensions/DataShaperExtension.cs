using System.Dynamic;
using System.Reflection;

namespace PRN232.LMS.Repositories.Extensions
{
    public static class DataShaperExtension
    {
        public static IEnumerable<dynamic> ShapeData<T>(this IEnumerable<T> entities, string? fields)
        {
            if (string.IsNullOrWhiteSpace(fields)) return entities.Cast<dynamic>();
          
            var chosenFields = fields.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(f => f.Trim().ToLower())
                    .ToList();
            
            var properties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => chosenFields.Contains(p.Name.ToLower()))
                .ToList();

            var result = new List<ExpandoObject>();
            foreach (var entity in entities)
            {
                var obj = (IDictionary<string, object>)new ExpandoObject();
                foreach (var prop in properties)
                {
                    obj[prop.Name] = prop.GetValue(entity)!;
                }
                result.Add((ExpandoObject)obj);
            }
            return result;
        }
    }
}