using System.Collections.Generic;
using Jellyfin.Server.Middleware;
using MediaBrowser.Controller.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

namespace Jellyfin.Server.Extensions
{
    /// <summary>
    /// Extensions for adding API specific functionality to the application pipeline.
    /// </summary>
    public static class ApiApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds swagger and swagger UI to the application pipeline.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <param name="serverConfigurationManager">The server configuration.</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseJellyfinApiSwagger(
            this IApplicationBuilder applicationBuilder,
            IServerConfigurationManager serverConfigurationManager)
        {
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.

            var baseUrl = serverConfigurationManager.Configuration.BaseUrl;
            var apiDocBaseUrl = serverConfigurationManager.Configuration.BaseUrl;
            if (!string.IsNullOrEmpty(baseUrl))
            {
                baseUrl = baseUrl.Trim('/') + '/';
            }

            return applicationBuilder
                .UseSwagger(c =>
                {
                    // Custom path requires {documentName}, SwaggerDoc documentName is 'api-docs'
                    c.RouteTemplate = "{documentName}/openapi.json";
                    c.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{apiDocBaseUrl}" } };
                    });
                })
                .UseSwaggerUI(c =>
                {
                    c.DocumentTitle = "Jellyfin API";
                    c.SwaggerEndpoint($"/{baseUrl}api-docs/openapi.json", "Jellyfin API");
                    c.InjectStylesheet($"/{baseUrl}api-docs/swagger/custom.css");
                    c.RoutePrefix = "api-docs/swagger";
                })
                .UseReDoc(c =>
                {
                    c.DocumentTitle = "Jellyfin API";
                    c.SpecUrl($"/{baseUrl}api-docs/openapi.json");
                    c.InjectStylesheet($"/{baseUrl}api-docs/redoc/custom.css");
                    c.RoutePrefix = "api-docs/redoc";
                });
        }

        /// <summary>
        /// Adds IP based access validation to the application pipeline.
        /// </summary>
        /// <param name="appBuilder">The application builder.</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseIpBasedAccessValidation(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<IpBasedAccessValidationMiddleware>();
        }

        /// <summary>
        /// Adds LAN based access filtering to the application pipeline.
        /// </summary>
        /// <param name="appBuilder">The application builder.</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseLanFiltering(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<LanFilteringMiddleware>();
        }

        /// <summary>
        /// Adds base url redirection to the application pipeline.
        /// </summary>
        /// <param name="appBuilder">The application builder.</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseBaseUrlRedirection(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<BaseUrlRedirectionMiddleware>();
        }

        /// <summary>
        /// Adds a custom message during server startup to the application pipeline.
        /// </summary>
        /// <param name="appBuilder">The application builder.</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseServerStartupMessage(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<ServerStartupMessageMiddleware>();
        }

        /// <summary>
        /// Adds a WebSocket request handler to the application pipeline.
        /// </summary>
        /// <param name="appBuilder">The application builder.</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseWebSocketHandler(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<WebSocketHandlerMiddleware>();
        }
    }
}
