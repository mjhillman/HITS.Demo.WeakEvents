using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HITS.Blazor.Grid
{
    /// <summary>
    /// This class is used to sort the list based on a generic object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericSorter<T>
    {
        /// <summary>
        /// Generic Sort Method
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        public IEnumerable<T> Sort(IEnumerable<T> source, string sortBy, string sortDirection)
        {
            var param = Expression.Parameter(typeof(T), "item");

            var sortExpression = Expression.Lambda<Func<T, object>>
                (Expression.Convert(Expression.Property(param, sortBy), typeof(object)), param);

            switch (sortDirection.ToLower())
            {
                case "asc":
                    return source.AsQueryable<T>().OrderBy<T, object>(sortExpression);
                default:
                    return source.AsQueryable<T>().OrderByDescending<T, object>(sortExpression);

            }
        }
    }
}
