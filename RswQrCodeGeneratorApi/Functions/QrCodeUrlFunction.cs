using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using QRCoder;
using RswQrCodeGeneratorApi.QrCode.Extensions;
using System;
using System.Net;

namespace RswQrCodeGeneratorApi.Functions
{
    public class QrCodeUrlFunction
    {
        private readonly ILogger<QrCodeUrlFunction> _logger;

        public QrCodeUrlFunction(ILogger<QrCodeUrlFunction> log)
        {
            _logger = log ?? throw new ArgumentNullException();
        }

        [FunctionName(nameof(QrCodeUrl))]
        [OpenApiOperation(operationId: nameof(QrCodeUrl), tags: new[] { "url" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "url", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Url** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/png", bodyType: typeof(byte[]), Description = "The OK response")]
        public IActionResult QrCodeUrl(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ExecutionContext context)
        {
            var stream = new PayloadGenerator.Url("https://github.com/codebude/QRCoder/")
                .GenerateQrCode()
                .SaveAsPngToStream();

            return new FileStreamResult(stream, "image/png");
        }
    }
}

