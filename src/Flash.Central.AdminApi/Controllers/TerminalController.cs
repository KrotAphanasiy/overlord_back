using DigitalSkynet.DotnetCore.Api.Controllers;
using DigitalSkynet.DotnetCore.DataStructures.Models.Response;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.ViewModel.GasStation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flash.Central.Foundation.Pagination;

namespace Flash.Central.AdminApi.Controllers
{
    /// <summary>
    /// Class of the controller. Represents endpoints responsible for work with terminal's datas.
    /// Derived from BaseApiController.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TerminalController : BaseApiController
    {
        private readonly ITerminalService _terminalService;

        /// <summary>
        /// Constructor. Initializes controller's parameters.
        /// </summary>
        /// <param name="terminalService">Defines methods bound to terminals</param>
        public TerminalController(ITerminalService terminalService)
        {
            _terminalService = terminalService;
        }

        /// <summary>
        /// Gets all terminals. Can be filtered by gas station's id
        /// </summary>
        /// <param name="gasStationId">Optional, gas station id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection of terminal's objects</returns>
        [HttpGet]
        public async Task<ActionResult<ApiCollectionResponseEnvelope<TerminalVm>>> GetAllTerminals([FromQuery] long? gasStationId, CancellationToken ct)
        {
            List<TerminalVm> result;
            if (gasStationId == null)
            {
                result = await _terminalService.GetAll(ct);
            }
            else
            {
                result = await _terminalService.GetForGasStation((long)gasStationId, ct);
            }
            return CollectionResponse(result);
        }

        /// <summary>
        /// Returns all terminals of gas station by id in pages
        /// </summary>
        /// <param name="gasStationId">Gas station's id</param>
        /// <param name="pagination">The object of PaginationModel
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of terminal's objects</returns>
        [HttpGet("paged")]
        public async Task<ActionResult<ApiPagedResponseEnvelope<TerminalVm>>> GetPagedGasStationTerminals(
            [FromQuery] long gasStationId, [FromQuery] PaginationModel pagination, CancellationToken ct)
        {
            var result = await _terminalService.GetPagedForGasStation(gasStationId, pagination, ct);
            return PagedCollectionResponse(result.Data, result.Total, result.PageNumber, result.PageSize);
        }

        /// <summary>
        /// Gets terminals of gas station, which are not covered by regions
        /// </summary>
        /// <param name="gasStationId">Gas station's id</param>
        /// <param name="pagination">The object of PaginationModel
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of terminal's objects</returns>
        [HttpGet("paged/available")]
        public async Task<ActionResult<ApiPagedResponseEnvelope<TerminalVm>>> GetPagedGasStationAvailableTerminals(
            [FromQuery] long gasStationId, [FromQuery] PaginationModel pagination, CancellationToken ct)
        {
            var result = await _terminalService.GetPagedAvailableForGasStation(gasStationId, pagination, ct);
            return PagedCollectionResponse(result.Data, result.Total, result.PageNumber, result.PageSize);
        }


        /// <summary>
        /// Creates new terminal
        /// </summary>
        /// <param name="model">The object of TerminalModel
        /// <see cref="TerminalModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Newly created teminal's object</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponseEnvelope<TerminalVm>>> CreateTerminal(TerminalModel model, CancellationToken ct)
        {
            var result = await _terminalService.Create(model, ct);
            return ResponseModel(result);
        }

        /// <summary>
        /// Updates terminal by id
        /// </summary>
        /// <param name="id">Terminal's id</param>
        /// <param name="model">The object of TerminalModel
        /// <see cref="TerminalModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Updated terminal's object</returns>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<ApiResponseEnvelope<TerminalVm>>> UpdateTerminal([FromRoute] long id, TerminalModel model, CancellationToken ct)
        {
            var result = await _terminalService.Update(id, model, ct);
            return Ok(result);
        }

        /// <summary>
        /// Deletes terminal by id
        /// </summary>
        /// <param name="id">Terminal's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value of deletion's result</returns>
        [HttpDelete("{id:long}")]
        public async Task<ActionResult<ApiResponseEnvelope<bool>>> DeleteTerminal([FromRoute] long id, CancellationToken ct)
        {
            var result = await _terminalService.Delete(id, ct);
            return Ok(result);
        }
    }
}
