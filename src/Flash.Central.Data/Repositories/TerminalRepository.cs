using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DigitalSkynet.DotnetCore.DataAccess.Enums;
using DigitalSkynet.DotnetCore.DataAccess.Repository;
using DigitalSkynet.DotnetCore.DataStructures.Exceptions.Api;
using Flash.Central.Data.Extensions;
using Flash.Central.Data.Repositories.Interfaces;
using Flash.Central.Foundation.Enums;
using Flash.Central.Foundation.Pagination;
using Flash.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flash.Central.Data.Repositories
{
    /// <summary>
    /// Class. Implements contract for all members of ITerminalRepository.
    /// Derived from GenericDeletableRepository.
    /// </summary>
    public class TerminalRepository : GenericDeletableRepository<CentralDbContext, Terminal, long>, ITerminalRepository
    {
        /// <summary>
        /// Constructor. Initializes parameters.
        /// </summary>
        /// <param name="dbContext">The object of CentralDbContext
        /// <see cref="CentralDbContext"/>
        /// </param>
        /// <param name="mapper">Defines AutoMapper methods and properties</param>
        public TerminalRepository(CentralDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        { }
        /// <summary>
        /// Counts terminals by predicate
        /// </summary>
        /// <param name="predicate">Linq expression</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<Terminal, bool>> predicate, CancellationToken ct)
        {
            var count = await _dbContext.Set<Terminal>().CountAsync(predicate, ct);
            return count;
        }
        /// <summary>
        /// Gets collection by predicate
        /// </summary>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="predicate">Linq expression</param>
        /// <param name="args">The object of PaginationArgs
        /// <see cref="PaginationArgs"/>
        /// </param>
        /// <param name="modes">The mode of fetching</param>
        /// <param name="baseQuery">Collection of terminals as Queryable</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection</returns>
        public Task<List<TProjection>> ProjectToAsync<TProjection>(Expression<Func<Terminal, bool>> predicate, PaginationArgs args, FetchModes modes,
            IQueryable<Terminal> baseQuery = default,
            CancellationToken ct = default)
        {

            var filterQuery = baseQuery == default ? GetBaseQuery(modes).Where(predicate) : baseQuery.Where(predicate);

            var type = typeof(Terminal);
            var properties = type.GetProperties();
            var orderedQuery = filterQuery;
            if (args.Sortings.Any())
            {
                var orderByCommand = args.Sortings.First().Direction == SortDirections.Asc ? "OrderBy" : "OrderByDescending";
                var isOrderedQuery = false;
                foreach (var sorting in args.Sortings)
                {
                    var property = type.GetProperty(sorting.FieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (property == null)
                    {
                        throw new ApiNotFoundException("Property is not found");
                    }
                    if (isOrderedQuery)
                    {
                        orderByCommand = sorting.Direction == SortDirections.Asc ? "ThenBy" : "ThenByDescending"; // second and all further orderings must be ThenBy
                    }
                    var parameter = Expression.Parameter(type, "p");
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExpression = Expression.Lambda(propertyAccess, parameter);
                    var resultExpression = Expression.Call(typeof(Queryable), orderByCommand, new Type[] { type, property.PropertyType },
                        orderedQuery.Expression, Expression.Quote(orderByExpression));
                    orderedQuery = orderedQuery.Provider.CreateQuery<Terminal>(resultExpression);
                    isOrderedQuery = true;
                }
            }

            var result = orderedQuery.ProjectTo<TProjection>(_mapper.ConfigurationProvider).ApplyPaging(args).ToListAsync(ct);
            return result;
        }

        /// <summary>
        /// Gets available by predicate
        /// </summary>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="predicate">Linq expression</param>
        /// <param name="args">The object of PaginationArgs
        /// <see cref="PaginationArgs"/>
        /// </param>
        /// <param name="modes">The mode of fetching</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection</returns>
        public Task<List<TProjection>> GetAvailableAsync<TProjection>(Expression<Func<Terminal, bool>> predicate,
            PaginationArgs args, FetchModes modes, CancellationToken ct = default)
        {
            var terminalsQuery = _dbContext.Set<Terminal>().Where(
                x => !_dbContext.Set<CameraRegion>().Select(cr => cr.TerminalId).Contains(x.Id));

            var terminals = ProjectToAsync<TProjection>(predicate, args, modes, terminalsQuery, ct);
            return terminals;
        }
    }
}
