using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Foundation.Constants;
using Flash.Central.Foundation.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Flash.Central.Api.Middleware
{

    /// <summary>
    /// Class. Basic camera Authentication handler
    /// </summary>
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ICameraService _cameraService;
        private readonly IEncodingService _encodingService;
        private readonly SuperuserOptions _superuserOptions;

        /// <summary>
        /// Constructor. Initializes parameters.
        /// </summary>
        /// <param name="options">Authentication's scheme parameters</param>
        /// <param name="superuserOptions">Superuser's parameters</param>
        /// <param name="logger">ILoggerFactory</param>
        /// <param name="encoder">UrlEncoder</param>
        /// <param name="cameraService">Defines methods bound to camera</param>
        /// <param name="clock">System clock</param>
        /// <param name="encodingService">Defines methods bound to encoding and decoding</param>
        /// <returns></returns>
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            IOptions<SuperuserOptions> superuserOptions,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ICameraService cameraService,
            ISystemClock clock,
            IEncodingService encodingService) : base(options, logger, encoder, clock)
        {
            _cameraService = cameraService;
            _encodingService = encodingService;
            _superuserOptions = superuserOptions.Value;
        }

        /// <summary>
        /// Authenticates camera by checking required api key
        /// </summary>
        /// <returns>The result of authentication</returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization header");

            Guid cameraUid;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                var decoded = _encodingService.DecodeBase64(authHeader.Parameter);
                var credentials = decoded.Split(new[] { ':' }, 2);

                var cameraUidStr = credentials[0];
                var apiKey = credentials[1];
                cameraUid = new Guid(cameraUidStr);

                //TODO: check api key and return true if it matches
                if (cameraUid == _superuserOptions.Id)
                {
                    if (apiKey != _superuserOptions.ApiKey)
                    {
                        return AuthenticateResult.Fail("Incorrect apiKey");
                    }

                    var superuserClaims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, cameraUid.ToString()),
                        new Claim(ClaimTypes.Role, UserRoles.Superuser)
                    };

                    var superuserTicket = new AuthenticationTicket(
                        new ClaimsPrincipal(new ClaimsIdentity(superuserClaims)),
                        Scheme.Name);

                    return AuthenticateResult.Success(superuserTicket);
                }


                // TODO: Get ct from context
                var isMatchApiKey = await _cameraService.IsMatchApiKey(cameraUid, apiKey, default);
                if (!isMatchApiKey)
                {
                    return AuthenticateResult.Fail("Incorrect apiKey");
                }
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, cameraUid.ToString())
            };

            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(
                    new ClaimsIdentity(claims)),
                Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
