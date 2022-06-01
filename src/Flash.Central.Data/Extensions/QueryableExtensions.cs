using System.Linq;
using Flash.Central.Foundation.Pagination;

namespace Flash.Central.Data.Extensions
{
    /// <summary>
    /// Class. Extends IQueryable
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Applies pagination for entities
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query">Generic entity as Queryable</param>
        /// <param name="args">The object of PaginationArgs
        /// <see cref="PaginationArgs"/>
        /// </param>
        /// <returns>Paged collection</returns>
        public static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> query, PaginationArgs args)
        {
            return query.Skip(args.PageSize * (args.PageNumber - 1)).Take(args.PageSize);
        }
    }
}
