using DigitalSkynet.DotnetCore.Api.Controllers;
using Flash.Central.Foundation.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Flash.Central.Api.Controllers
{
    /// <summary>
    /// Controller. Provides version info
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VersionController : BaseApiController
    {
        private readonly VersionOptions _versionOptions;

        /// <summary>
        /// Constructor. Initializes controller's parameters.
        /// </summary>
        /// <param name="versionOptions">Determines version parameters</param>
        public VersionController(IOptions<VersionOptions> versionOptions)
        {
            _versionOptions = versionOptions.Value;
        }

        /// <summary>
        /// Provides version info
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<VersionOptions> Get()
        {
            return Ok(_versionOptions);
        }

    }
}
