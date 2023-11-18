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
using RswQrCodeGeneratorApi.Domain.DTO;
using RswQrCodeGeneratorApi.QrCode.Extensions;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

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
            string url = req.Query["url"];
            if (url == null)
                return new BadRequestObjectResult("Please pass capacity and hours in the request body");

            var stream = new PayloadGenerator.Url(url).GenerateQrCode().SaveAsPngToStream();

            return new FileStreamResult(stream, "image/png");
        }

        [FunctionName(nameof(QrCodeUrlAsync))]
        [OpenApiOperation(operationId: nameof(QrCodeUrlAsync))]
        //[OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody("application/json", typeof(UrlDTO),
            Description = $"JSON request body containing {nameof(UrlDTO)}")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/png", bodyType: typeof(byte[]),
            Description = "The OK response containing a image/png result.")]
        public async Task<IActionResult> QrCodeUrlAsync(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = null)] HttpRequest req, ExecutionContext context)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var urlDTO = JsonConvert.DeserializeObject<UrlDTO>(body);
            if (urlDTO == null)
                return new BadRequestObjectResult($"Please pass {nameof(UrlDTO)} in the request body");

            var stream = new PayloadGenerator.Url(urlDTO.Url).GenerateQrCode().SaveAsPngToStream();

            return new FileStreamResult(stream, "image/png");
        }

    }
}

