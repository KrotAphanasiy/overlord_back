using System.Threading;
using System.Threading.Tasks;
using DigitalSkynet.DotnetCore.Api.Controllers;
using DigitalSkynet.DotnetCore.DataStructures.Models.Response;
using Flash.Central.Api.Authorization;
using Flash.Central.Api.Extensions;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.ViewModel.RecognitionEvent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flash.Central.Api.Controllers
{
    /// <summary>
    /// Class of the controller. Represents endpoints responsible for work with recognition events' datas.
    /// </summary>
    [Route("api/v1/[controller]")]
    public class RecognitionEventController : BaseApiController
    {
        private readonly IRecognitionEventService _recognitionEventService;

        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// Constructor. Initializes controller's parameters.
        /// </summary>
        /// <param name="recognitionEventService">Defines methods bound to recognition events</param>
        /// <param name="authorizationService">Defines methods bound to authorization</param>
        public RecognitionEventController(IRecognitionEventService recognitionEventService, IAuthorizationService authorizationService)
        {
            _recognitionEventService = recognitionEventService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Submits the event. Creates the new event in database. Image example data:image/png;base64, iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==
        /// </summary>
        /// <param name="model">The determined object of RecognitionEventModel</param>
        /// <param name="ct"></param>
        /// <returns>Newly created event</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponseEnvelope<RecognitionEventVm>>> SubmitEvent(RecognitionEventModel model, CancellationToken ct = default)
        {
            await _authorizationService.AuthorizeResourceAsync<long, CameraRegionRequirement>(User, model.CameraRegionId);
            var result = await _recognitionEventService.Create(model, ct);
            return ResponseModel(result);
        }
    }
}
