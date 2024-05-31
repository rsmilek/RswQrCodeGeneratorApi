using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using QRCoder;
using RswQrCodeGeneratorApi.Domain.DTOs;
using RswQrCodeGeneratorApi.QrCode.Extensions;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace RswQrCodeGeneratorApi.Functions
{
    public class QrCodeEmailFunction
    {
        private readonly ILogger<QrCodeUrlFunction> _logger;

        public QrCodeEmailFunction(ILogger<QrCodeUrlFunction> log)
        {
            _logger = log ?? throw new ArgumentNullException();
        }

        /// <summary>http://localhost:7069/api/swagger/ui#/default/QrCodeEmailAsync</summary>
        [Function(nameof(QrCodeEmailAsync))]
        [OpenApiOperation(operationId: nameof(QrCodeEmailAsync))]
        //[OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody("application/json", typeof(EmailDTO),
            Description = $"JSON request body containing {nameof(EmailDTO)}")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/png", bodyType: typeof(byte[]),
            Description = "The OK response containing a image/png result.")]
        public async Task<IActionResult> QrCodeEmailAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = null)] HttpRequest request, ExecutionContext context)
        {
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            var emailDTO = JsonConvert.DeserializeObject<EmailDTO>(body);
            if (emailDTO == null)
                return new BadRequestObjectResult($"Please pass {nameof(UrlDTO)} in the request body!");

            var stream = new PayloadGenerator.Mail(emailDTO.Email, emailDTO.Subject, emailDTO.Message).GenerateQrCode().SaveAsPngToStream();

            return new FileStreamResult(stream, "image/png");
        }
    }
}

