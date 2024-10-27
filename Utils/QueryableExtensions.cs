using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SocialMediaServer.Utils
{
    public static class QueryableExtensions
    {
      
        public static IQueryable<T> ApplyIncludes<T>(this IQueryable<T> query, string? includes) where T : class
        {
            if (string.IsNullOrWhiteSpace(includes))
            {
                return query;
            }

            var includesArray = includes.Split(",");
            foreach (var include in includesArray)
            {
                var trimmedInclude = include.Trim();
                if (string.IsNullOrEmpty(trimmedInclude)) continue;

                query = query.Include(trimmedInclude);
            }

            return query;
        }

        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? sortExpression) where T : class
        {
            if (string.IsNullOrEmpty(sortExpression)) return query;

            var sortExpressions = sortExpression.Split(",");
            IOrderedQueryable<T> orderedQuery = null;
            foreach (var expression in sortExpressions)
            {
                var trimmedExpression = expression.Trim();
                var isDescending = trimmedExpression.StartsWith("-");
                var propertyName = isDescending ? trimmedExpression.Substring(1) : trimmedExpression;

                orderedQuery = orderedQuery == null
                    ? (isDescending ? query.OrderByDescending(GetProperty<T>(propertyName)) : query.OrderBy(GetProperty<T>(propertyName)))
                    : (isDescending ? orderedQuery.ThenByDescending(GetProperty<T>(propertyName)) : orderedQuery.ThenBy(GetProperty<T>(propertyName)));
            }

            return orderedQuery ?? query;
        }

        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, Dictionary<string, object?> filters) where T : class
        {
            if (filters == null || filters.Count == 0) return query;

            foreach (var filter in filters)
            {
                var key = filter.Key;
                var value = filter.Value;

                if (value is string filterString && filterString.Contains(":"))
                {
                    var parts = filterString.Split(":", 2);
                    var operatorType = parts[0];
                    var filterValue = parts[1];

                    var parameter = Expression.Parameter(typeof(T), "x");
                    var member = Expression.PropertyOrField(parameter, key);
                    var constant = Expression.Constant(ConvertValue(member.Type, filterValue));
                    Expression expression = null;

                    switch (operatorType)
                    {
                        case "eq":
                            expression = Expression.Equal(member, constant);
                            break;
                        case "neq":
                            expression = Expression.NotEqual(member, constant);
                            break;
                        case "gt":
                            expression = Expression.GreaterThan(member, constant);
                            break;
                        case "gte":
                            expression = Expression.GreaterThanOrEqual(member, constant);
                            break;
                        case "lt":
                            expression = Expression.LessThan(member, constant);
                            break;
                        case "lte":
                            expression = Expression.LessThanOrEqual(member, constant);
                            break;
                        case "like":
                            expression = Expression.Call(member, typeof(string).GetMethod("Contains", new[] { typeof(string) }), constant);
                            break;
                        case "in":
                            var values = filterValue.Split(",");
                            var inValues = values.Select(v => ConvertValue(member.Type, v)).ToList();
                            var inExpression = Expression.Call(typeof(Enumerable), "Contains", new[] { member.Type }, Expression.Constant(inValues), member);
                            expression = inExpression;
                            break;
                        default:
                            throw new ArgumentException($"Invalid filter operator: {operatorType}");
                    }

                    var lambda = Expression.Lambda<Func<T, bool>>(expression, parameter);
                    query = query.Where(lambda);
                }
                else if (value != null)
                {
                    query = query.Where(BuildEqualPredicate<T>(key, value));
                }
            }

            return query;
        }

        private static object ConvertValue(Type type, string value)
        {
            if (type == typeof(int)) return int.Parse(value);
            if (type == typeof(long)) return long.Parse(value);
            if (type == typeof(double)) return double.Parse(value);
            if (type == typeof(DateTime)) return DateTime.Parse(value);
            return value;
        }

        private static Expression<Func<T, bool>> BuildEqualPredicate<T>(string propertyName, object value) where T : class
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var member = Expression.PropertyOrField(parameter, propertyName);
            var constant = Expression.Constant(value);
            var body = Expression.Equal(member, constant);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static Expression<Func<T, object>> GetProperty<T>(string propertyName) where T : class
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var member = Expression.PropertyOrField(parameter, propertyName);
            return Expression.Lambda<Func<T, object>>(Expression.Convert(member, typeof(object)), parameter);
        }

        public static async Task<PaginatedResult<T>> ApplyPaginationAsync<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<T>(items, totalItems, page, pageSize);
        }
    }
}
