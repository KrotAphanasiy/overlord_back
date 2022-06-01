using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Flash.Central.Dtos.Visit;
using Flash.Central.Foundation.Pagination;
using Flash.Central.ViewModel.Visit;
using Flash.Domain.Entities;
using Hangfire.Server;

namespace Flash.Central.Core.Services.Interfaces
{
    /// <summary>
    /// Interface. Specifies the contract for visit's services.
    /// Derived from IServiceBase.
    /// </summary>
    public interface IVisitService : IServiceBase<VisitDto, VisitVm, long>
    {
        /// <summary>
        /// Gets collection of visits by gas station's id. Can be filtered by dates.
        /// </summary>
        /// <param name="id">Gas station's id</param>
        /// <param name="startDate">Optional. The date of visit's start</param>
        /// <param name="endDate">Optional. The date of visit's start</param>
        /// <param name="pagination">The object of PaginationModel
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection</returns>
        Task<Paged<VisitVm>> GetVisitsByGasStationId(long id, DateTime? startDate, DateTime? endDate, PaginationModel pagination, CancellationToken ct);
        /// <summary>
        /// Creates visit
        /// </summary>
        /// <param name="models">The object of VisitDto
        /// <see cref="VisitDto"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Newly created visit</returns>
        Task<IEnumerable<VisitVm>> Create(IEnumerable<VisitDto> models, CancellationToken ct);
        /// <summary>
        /// Joins adjecent visits
        /// </summary>
        /// <param name="visits">Collection of tuples. Key - visit's object, value - collection of recognition events</param>
        /// <returns>Collection of tuples</returns>
        IEnumerable<Tuple<Visit, List<RecognitionEvent>>> JoinAdjecentVisits(
            List<Tuple<Visit, List<RecognitionEvent>>> visits);
        /// <summary>
        /// Gets visits from events
        /// </summary>
        /// <param name="events">Collection of recognition events</param>
        /// <returns>Collection of tuples</returns>
        IEnumerable<Tuple<Visit, List<RecognitionEvent>>> GetVisitsFromEvents(IReadOnlyList<RecognitionEvent> events);
    }
}
