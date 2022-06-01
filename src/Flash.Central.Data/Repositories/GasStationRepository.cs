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
    /// Class. Implements contract for all members of IGasStationRepository.
    /// Derived from GenericDeletableRepository.
    /// </summary>
    public class GasStationRepository : GenericDeletableRepository<CentralDbContext, GasStation, long>, IGasStationRepository
    {
        /// <summary>
        /// Constructor. Initializes parameters.
        /// </summary>
        /// <param name="dbContext">The object of CentralDbContext
        /// <see cref="CentralDbContext"/>
        /// </param>
        /// <param name="mapper">Defines AutoMapper methods and properties</param>
        public GasStationRepository (CentralDbContext dbContext, IMapper mapper) : base (dbContext, mapper)
        { }
        /// <summary>
        /// Counts gas stations by predicate
        /// </summary>
        /// <param name="predicate">Linq qxpression</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<GasStation, bool>> predicate, CancellationToken ct)
        {
            var count = await _dbContext.Set<GasStation>().CountAsync(predicate, ct);
            return count;
        }
        /// <summary>
        /// Gets paged collection
        /// </summary>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="predicate">Linq expression</param>
        /// <param name="args">The object of PaginationArgs
        /// <see cref="PaginationArgs"/>
        /// </param>
        /// <param name="modes">The mode of fetching</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection</returns>
        public Task<List<TProjection>> ProjectToAsync<TProjection>(Expression<Func<GasStation, bool>> predicate, PaginationArgs args, FetchModes modes,
            CancellationToken ct = default)
        {
            var filterQuery = GetBaseQuery (modes).Where (predicate);
            var type = typeof (GasStation);
            var properties = type.GetProperties ();
            var orderedQuery = filterQuery;
            if (args.Sortings.Any ())
            {
                var orderByCommand = args.Sortings.First ().Direction == SortDirections.Asc ? "OrderBy" : "OrderByDescending";
                var isOrderedQuery = false;
                foreach (var sorting in args.Sortings)
                {
                    var property = type.GetProperty (sorting.FieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (property == null)
                    {
                        throw new ApiNotFoundException ("Property is not found");
                    }
                    if (isOrderedQuery)
                    {
                        orderByCommand = sorting.Direction == SortDirections.Asc ? "ThenBy" : "ThenByDescending"; // second and all further orderings must be ThenBy
                    }
                    var parameter = Expression.Parameter (type, "p");
                    var propertyAccess = Expression.MakeMemberAccess (parameter, property);
                    var orderByExpression = Expression.Lambda (propertyAccess, parameter);
                    var resultExpression = Expression.Call (typeof (Queryable), orderByCommand, new Type[] { type, property.PropertyType },
                        orderedQuery.Expression, Expression.Quote (orderByExpression));
                    orderedQuery = orderedQuery.Provider.CreateQuery<GasStation> (resultExpression);
                    isOrderedQuery = true;
                }
            }

            var result = orderedQuery.ProjectTo<TProjection> (_mapper.ConfigurationProvider).ApplyPaging (args).ToListAsync (ct);
            return result;
        }
    }
}
