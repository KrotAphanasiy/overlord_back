using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Flash.Central.Api.Authorization;
using Flash.Central.Api.Middleware;
using Flash.Central.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Flash.Central.Api.Extensions
{
    /// <summary>
    /// Class. Implements authorization middleware. Configures all authorization methods for application
    /// </summary>
    public static class AuthorizationConfigurationExtension
    {

        /// <summary>
        /// Sets authorization up
        /// </summary>
        /// <param name="services">Extension of IServiceCollection. Specifies the contract for a collection of service descriptors</param>
        public static void SetupAuthorization(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, CameraAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, CameraRegionAuthorizationHandler>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            IConfigurationService config = serviceProvider.GetRequiredService<IConfigurationService>();

            ConfigureCameraAuthorization(services, config);
        }

        /// <summary>
        /// Configurates camera's authorization
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors</param>
        /// <param name="config">Defines configuration settings</param>
        private static void ConfigureCameraAuthorization(IServiceCollection services, IConfigurationService config)
        {
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes("BasicAuthentication")
                    .RequireClaim(ClaimTypes.NameIdentifier)
                    .Build();
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }
    }
}
