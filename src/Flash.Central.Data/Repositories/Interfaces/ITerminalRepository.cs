using System;
using System.Collections.Generic;
using System.Linq;
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
    /// IInterface. Specifies the contract for terminal repository classes.
    /// Derived from IGenericDeletableRepository.
    /// </summary>
    public interface ITerminalRepository : IGenericDeletableRepository<Terminal, long>
    {
        /// <summary>
        /// Counts  by predicate.
        /// </summary>
        /// <param name="predicate">Linq expression</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Number</returns>
        Task<int> CountAsync(Expression<Func<Terminal, bool>> predicate, CancellationToken ct);
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
        public Task<List<TProjection>> ProjectToAsync<TProjection>(Expression<Func<Terminal, bool>> predicate,
            PaginationArgs args, FetchModes modes,
            IQueryable<Terminal> baseQuery = default,
            CancellationToken ct = default);
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
        Task<List<TProjection>> GetAvailableAsync<TProjection>(Expression<Func<Terminal, bool>> predicate,
            PaginationArgs args, FetchModes modes, CancellationToken ct = default);

    }
}
