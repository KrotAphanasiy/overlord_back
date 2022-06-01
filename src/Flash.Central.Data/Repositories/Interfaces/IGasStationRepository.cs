using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DigitalSkynet.DotnetCore.DataAccess.Enums;
using DigitalSkynet.DotnetCore.DataAccess.Repository;
using Flash.Central.Foundation.Pagination;
using Flash.Domain.Entities;

namespace Flash.Central.Data.Repositories.Interfaces
{
    /// <summary>
    /// IInterface. Specifies the contract for gas station repository classes.
    /// Derived from IGenericDeletableRepository.
    /// </summary>
    public interface IGasStationRepository : IGenericDeletableRepository<GasStation, long>
    {
        /// <summary>
        /// Counts by predicate.
        /// </summary>
        /// <param name="predicate">Linq expression</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Number</returns>
        Task<int> CountAsync(Expression<Func<GasStation, bool>> predicate, CancellationToken ct);
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
        Task<List<TProjection>> ProjectToAsync<TProjection>(Expression<Func<GasStation, bool>> predicate,
            PaginationArgs args, FetchModes modes, CancellationToken ct = default);
    }
}
