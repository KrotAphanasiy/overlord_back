using System.Threading;
using System.Threading.Tasks;
using DigitalSkynet.DotnetCore.Api.Controllers;
using DigitalSkynet.DotnetCore.DataStructures.Models.Response;
using Flash.Central.Api.Authorization;
using Flash.Central.Api.Extensions;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.ViewModel.DetectionEvent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flash.Central.Api.Controllers
{
    /// <summary>
    /// Class of the controller. Represents endpoints responsible for work with detection events' datas.
    /// </summary>
    [Route("api/v1/[controller]")]
    public class DetectionEventController : BaseApiController
    {
        private readonly IDetectionEventService _detectionEventService;
        private readonly IAuthorizationService _authorizationService;


        /// <summary>
        /// Constructor. Initializes controller's parameters.
        /// </summary>
        /// <param name="detectionEventService">Defines methods bound to detection events</param>
        /// <param name="authorizationService">Defines methods bound to authorization</param>
        public DetectionEventController(IDetectionEventService detectionEventService,
            IAuthorizationService authorizationService)
        {
            _detectionEventService = detectionEventService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Submits the event. Creates a new event in Kafka topic to be consumed by any ready recognizer
        /// </summary>
        /// <param name="model">The determined object of DetectionEventModel</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Newly created event</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponseEnvelope<DetectionEventVm>>> SubmitEvent(DetectionEventModel model, CancellationToken ct = default)
        {
            await _authorizationService.AuthorizeResourceAsync<long, CameraRegionRequirement>(User, model.CameraRegionId);
            var result = await _detectionEventService.Create(model, ct);
            return ResponseModel(result);
        }

    }
}
