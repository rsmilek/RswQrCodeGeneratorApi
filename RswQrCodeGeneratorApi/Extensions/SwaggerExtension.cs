using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace RswQrCodeGeneratorApi.Extensions
{
    internal static class SwaggerExtension
    {
        internal static void AddSwagger(this IServiceCollection services)
        {
            const string ApiGithubUrl = "https://github.com/rsmilek/RswQrCodeGeneratorApi";
            const string ApiUrl = "https://RswQrCodeGeneratorApi.azurewebsites.com";

            services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
            {
                var options = new OpenApiConfigurationOptions()
                {
                    Info = new OpenApiInfo()
                    {
                        Version = "1.0.0",
                        Title = "Swagger RswQrCodeGeneratorApi",
                        Description = $"This is a QR code generaor API designed by [{ApiUrl}]({ApiUrl}).",
                        TermsOfService = new Uri(ApiGithubUrl),
                        Contact = new OpenApiContact()
                        {
                            Name = "Radim Smilek",
                            Email = "rsmilek@seznam.cz",
                            Url = new Uri(ApiGithubUrl),
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "MIT",
                            Url = new Uri("http://opensource.org/licenses/MIT"),
                        }
                    },
                    Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
                    OpenApiVersion = OpenApiVersionType.V3,
                    IncludeRequestingHostName = true,
                    ForceHttps = false,
                    ForceHttp = false,
                };

                return options;
            });
        }
    }
}
