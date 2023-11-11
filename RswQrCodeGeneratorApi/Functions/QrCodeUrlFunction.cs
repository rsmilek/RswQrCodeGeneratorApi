using System;
using System.IO;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace RswQrCodeGeneratorApi.Functions
{
    public class QrCodeUrlFunction
    {
        private readonly ILogger<QrCodeUrlFunction> _logger;

        public QrCodeUrlFunction(ILogger<QrCodeUrlFunction> log)
        {
            _logger = log;
        }

        [FunctionName("QrCodeUrl")]
        [OpenApiOperation(operationId: "QrCodeUrl", tags: new[] { "url" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "url", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Url** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/jpeg", bodyType: typeof(byte[]), Description = "The OK response")]
        public IActionResult QrCodeUrl(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var fs = File.OpenRead(@$"C:\Projects\JavaScript\OpenHabApi\OpenHabApi\Images\R_Right.jpg");
            return new FileStreamResult(fs, "image/jpeg");
        }
    }
}

