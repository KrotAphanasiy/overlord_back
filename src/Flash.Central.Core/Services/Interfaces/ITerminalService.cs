using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Flash.Central.Foundation.Pagination;
using Flash.Central.ViewModel.GasStation;

namespace Flash.Central.Core.Services.Interfaces
{
    /// <summary>
    /// Interface. Specifies the contract for terminal's services.
    /// Derived from IServiceBase.
    /// </summary>
    public interface ITerminalService : IServiceBase<TerminalModel, TerminalVm, long>
    {
        /// <summary>
        /// Gets paged collection of gas stations by id
        /// </summary>
        /// <param name="gasStationId">Gas station's id</param>
        /// <param name="pagination">The object of PaginationModel
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection</returns>
        Task<Paged<TerminalVm>> GetPagedForGasStation(long gasStationId, PaginationModel pagination,
            CancellationToken ct);
        /// <summary>
        /// Gets paged collection of terminals for certain gas station
        /// </summary>
        /// <param name="gasStationId">Gas station's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection of terminals</returns>
        Task<List<TerminalVm>> GetForGasStation(long gasStationId, CancellationToken ct);
        /// <summary>
        /// Gets paged collection of terminals available for gas station
        /// </summary>
        /// <param name="gasStationId">Gas station's id</param>
        /// <param name="pagination">The object of PaginationModel
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection</returns>
        Task<Paged<TerminalVm>> GetPagedAvailableForGasStation(long gasStationId, PaginationModel pagination,
            CancellationToken ct);
    }
}
