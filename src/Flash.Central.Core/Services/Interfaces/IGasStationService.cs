using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Flash.Central.Foundation.Pagination;
using Flash.Central.ViewModel.GasStation;

namespace Flash.Central.Core.Services.Interfaces
{
    /// <summary>
    /// Interface. Specifies the contract for gas station services.
    /// Derived from IServiceBase.
    /// </summary>
    public interface IGasStationService : IServiceBase<GasStationModel, GasStationVm, long>
    {
        Task<Paged<GasStationVm>> GetPagedAsync(PaginationModel pagination, CancellationToken ct);
    }
}
