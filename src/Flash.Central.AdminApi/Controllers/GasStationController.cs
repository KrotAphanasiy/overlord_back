using System;
using DigitalSkynet.DotnetCore.Api.Controllers;
using DigitalSkynet.DotnetCore.DataStructures.Models.Response;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.ViewModel.GasStation;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Flash.Central.Foundation.Pagination;

namespace Flash.Central.AdminApi.Controllers
{
    /// <summary>
    /// Class of the controller. Represents endpoints responsible for work with gas station's datas.
    /// Derived from BaseApiController.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GasStationController : BaseApiController
    {
        private readonly IGasStationService _gasStationService;
        /// <summary>
        /// Constructor. Initializes controller's parameters.
        /// </summary>
        /// <param name="gasStationService">Defines methods bound to gas stations</param>
        public GasStationController(IGasStationService gasStationService)
        {
            _gasStationService = gasStationService;
        }

        /// <summary>
        /// Gets all gas stations
        /// </summary>
        /// <returns>Collection of GasStation's objects</returns>
        [HttpGet]
        public async Task<ActionResult<ApiCollectionResponseEnvelope<GasStationVm>>> GetAllGasStations(CancellationToken ct)
        {
            var result = await _gasStationService.GetAll(ct);
            return CollectionResponse(result);
        }

        /// <summary>
        /// Gets gas station by id
        /// </summary>
        /// <param name="id">GasStation's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Gas station's object</returns>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<ApiResponseEnvelope<GasStationVm>>> Get([FromRoute] long id, CancellationToken ct)
        {
            var result = await _gasStationService.Get(id, ct);
            return ResponseModel(result);
        }

        /// <summary>
        /// Gets all gas stations paged
        /// </summary>
        /// <param name="pagination">The object of PaginationModel to enumerate  pages
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of gas station's objects</returns>
        [HttpGet("paged")]
        public async Task<ActionResult<ApiPagedResponseEnvelope<GasStationVm>>> GetPagedAllGasStations(
            [FromQuery] PaginationModel pagination, CancellationToken ct)
        {
            var result = await _gasStationService.GetPagedAsync(pagination, ct);
            return PagedCollectionResponse(result.Data, result.Total, result.PageNumber, result.PageSize);
        }

        /// <summary>
        /// Creates gas station
        /// </summary>
        /// <param name="model">The object of GasStationModel
        /// <see cref="GasStationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Newly created gas station's object</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponseEnvelope<GasStationVm>>> CreateGasStation(GasStationModel model, CancellationToken ct)
        {
            var result = await _gasStationService.Create(model, ct);
            return ResponseModel(result);
        }

        /// <summary>
        /// Updates gas station by id
        /// </summary>
        /// <param name="id">gas station's id</param>
        /// <param name="model">The object of GasStationModel
        /// <see cref="GasStationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Updated gas station's object</returns>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<ApiResponseEnvelope<GasStationVm>>> UpdateGasStation([FromRoute] long id, GasStationModel model, CancellationToken ct)
        {
            var result = await _gasStationService.Update(id, model, ct);
            return Ok(result);
        }

        /// <summary>
        /// Deletes gas station
        /// </summary>
        /// <param name="id">The id of gas station</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value of deletion's result</returns>
        [HttpDelete("{id:long}")]
        public async Task<ActionResult<ApiResponseEnvelope<bool>>> DeleteGasStation([FromRoute] long id, CancellationToken ct)
        {
            var result = await _gasStationService.Delete(id, ct);
            return Ok(result);
        }
    }
}
