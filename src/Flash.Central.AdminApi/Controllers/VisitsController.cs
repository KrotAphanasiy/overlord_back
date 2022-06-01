using DigitalSkynet.DotnetCore.Api.Controllers;
using DigitalSkynet.DotnetCore.DataStructures.Models.Response;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Dtos.Visit;
using Flash.Central.ViewModel.Visit;
using Flash.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Flash.Central.Foundation.Pagination;

namespace Flash.Central.AdminApi.Controllers
{
    /// <summary>
    /// Class of the controller. Represents endpoints responsible for work with visit's datas.
    /// Derived from BaseApiController.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VisitsController : BaseApiController
    {
        private readonly IVisitService _visitsService;
        /// <summary>
        /// Constructor. Initializes controller's parameters.
        /// </summary>
        /// <param name="visitsService">Defines methods bound to visits</param>
        public VisitsController(IVisitService visitsService)
        {
            _visitsService = visitsService;
        }

        /// <summary>
        /// Gets visits
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>The collection of visit's objects</returns>
        [HttpGet]
        public async Task<ActionResult<ApiCollectionResponseEnvelope<VisitVm>>> GetVisits(CancellationToken ct)
        {
            var result = await _visitsService.GetAll(ct);
            return CollectionResponse(result);
        }

        /// <summary>
        /// Gets visits by gas station Id. Can be filtered by dates
        /// </summary>
        /// <param name="pagination">The object of PaginationModel
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="gasStationId">Gas station's id</param>
        /// <param name="startDate">Optional, the date of visit's start</param>
        /// <param name="endDate">Optional, the date of visit's end</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of visit's objects</returns>
        [HttpGet("station/{gasStationId:long}")]
        public async Task<ActionResult<ApiPagedResponseEnvelope<VisitVm>>> GetVisitsByGasStationId([FromQuery] PaginationModel pagination,
            [FromRoute] long gasStationId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, CancellationToken ct)
        {
            var result = await _visitsService.GetVisitsByGasStationId(gasStationId, startDate, endDate, pagination, ct);
            return PagedCollectionResponse(result.Data, result.Total, result.PageNumber, result.PageSize);
        }

        /// <summary>
        /// Creates new visit
        /// </summary>
        /// <param name="model">The object of VisitDto
        /// <see cref="VisitDto"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Newly created visit's object</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponseEnvelope<VisitVm>>> CreateVisit(VisitDto model, CancellationToken ct)
        {
            var result = await _visitsService.Create(model, ct);
            return ResponseModel(result);
        }

        /// <summary>
        /// Updates visits by id
        /// </summary>
        /// <param name="id">Visit's id</param>
        /// <param name="model">The object of VisitDto
        /// <see cref="VisitDto"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Updated visit's object</returns>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<ApiResponseEnvelope<VisitVm>>> UpdateVisit([FromRoute] long id, VisitDto model, CancellationToken ct)
        {
            var result = await _visitsService.Update(id, model, ct);
            return Ok(result);
        }

        /// <summary>
        /// Deletes visit by id
        /// </summary>
        /// <param name="id">Visit's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value of deletion's result</returns>
        [HttpDelete("{id:long}")]
        public async Task<ActionResult<ApiResponseEnvelope<bool>>> DeleteVisits([FromRoute] long id, CancellationToken ct)
        {
            var result = await _visitsService.Delete(id, ct);
            return Ok(result);
        }
    }
}
