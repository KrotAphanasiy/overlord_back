using DigitalSkynet.DotnetCore.Api.Controllers;
using DigitalSkynet.DotnetCore.DataStructures.Models.Response;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.ViewModel.RecognitionEvent;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Flash.Central.Foundation.Pagination;

namespace Flash.Central.AdminApi.Controllers
{
    /// <summary>
    /// Class of the controller. Represents endpoints responsible for work with recognition events' datas.
    /// Derived from BaseApiController.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RecognitionEventController : BaseApiController
    {
        private readonly IRecognitionEventService _recognitionEventService;

        /// <summary>
        /// Constructor. Initializes controller's parameters.
        /// </summary>
        /// <param name="recognitionEventService">Defines methods bound to recognition events</param>
        public RecognitionEventController(IRecognitionEventService recognitionEventService)
        {
            _recognitionEventService = recognitionEventService;
        }

        /// <summary>
        /// Gets paged events of recognition
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <param name="pagination">The object of PaginationModel to enumerate and sort pages
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <returns>Paged collection of events</returns>
        [HttpGet("events")]
        public async Task<ActionResult<ApiPagedResponseEnvelope<RecognitionEventVm>>> GetEvents([FromQuery] PaginationModel pagination, CancellationToken ct = default)
        {
            var result = await _recognitionEventService.GetPagedAsync(pagination, ct);
            return PagedCollectionResponse(result.Data, result.Total, result.PageNumber, result.PageSize);
        }

        /// <summary>
        /// Gets raw events of recognition from exact camera by it`s uid. Can be filtered by dates
        /// </summary>
        /// <param name="pagination">The object of PaginationModel to enumerate and sort pages
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="cameraUid">Camera's guid</param>
        /// <param name="startedAt">Optional, the date of event's start</param>
        /// <param name="endedAt">Optional, the date of event's end</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of events</returns>
        [HttpGet("camera_events")]
        public async Task<ActionResult<ApiPagedResponseEnvelope<RecognitionEventVm>>> GetEventsByCameraUid([FromQuery] PaginationModel pagination, [FromQuery] Guid cameraUid,
            [FromQuery] DateTime? startedAt, [FromQuery] DateTime? endedAt, CancellationToken ct = default)
        {
            var result = await _recognitionEventService.GetPagedByCameraUidAsync(pagination, cameraUid, startedAt,endedAt, ct);
            return PagedCollectionResponse(result.Data, result.Total, result.PageNumber, result.PageSize);
        }

        /// <summary>
        /// Gets raw events of recognition between certain dates by camera's id if necessary
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="startedAt">The date of event's start</param>
        /// <param name="endedAt">The date of event's end</param>
        /// /// <param name="cameraId">Optional, camera's guid</param>
        /// <returns>Collection of events</returns>
        [HttpGet("date_events")]
        public async Task<ActionResult<ApiCollectionResponseEnvelope<RecognitionEventVm>>> GetDateEvents([FromQuery] Guid? cameraId, DateTime startedAt, DateTime endedAt,
            CancellationToken ct = default)
        {
            var result = await _recognitionEventService.GetBetweenDates(cameraId, startedAt, endedAt, ct);
            return CollectionResponse(result);
        }
    }
}
