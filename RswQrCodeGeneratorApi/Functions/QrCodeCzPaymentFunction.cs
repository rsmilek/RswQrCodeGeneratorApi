using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RswQrCodeGeneratorApi.Domain.DTOs;
using RswQrCodeGeneratorApi.Domain.Payloads;
using RswQrCodeGeneratorApi.QrCode.Extensions;
using System.Net;

namespace RswQrCodeGeneratorApi.Functions
{
    public class QrCodeCzPaymentFunction
    {
        private readonly ILogger<QrCodeCzPaymentFunction> _logger;

        public QrCodeCzPaymentFunction(ILogger<QrCodeCzPaymentFunction> log)
        {
            _logger = log;
        }

        /// <summary>http://localhost:7069/api/swagger/ui#/default/QrCodeCzPaymentAsync</summary>
        [Function(nameof(QrCodeCzPaymentAsync))]
        [OpenApiOperation(operationId: nameof(QrCodeCzPaymentAsync))]
        //[OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody("application/json", typeof(CzPaymentDTO),
            Description = $"JSON request body containing {nameof(CzPaymentDTO)}")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/png", bodyType: typeof(byte[]),
            Description = "The OK response containing a image/png result.")]
        public async Task<IActionResult> QrCodeCzPaymentAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = null)] HttpRequest request, ExecutionContext context)
        {
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            var czPaymentDTO = JsonConvert.DeserializeObject<CzPaymentDTO>(body);
            if (czPaymentDTO == null)
                return new BadRequestObjectResult($"Please pass {nameof(CzPaymentDTO)} in the request body!");

            var stream = new CzPayment(czPaymentDTO.Prefix, czPaymentDTO.Account, czPaymentDTO.Bank, czPaymentDTO.Amount,
                czPaymentDTO.VariableSymbol, czPaymentDTO.SpecificSymbol, czPaymentDTO.SpecificSymbol, czPaymentDTO.Message)
                .GenerateQrCode()
                .SaveAsPngToStream();

            return new FileStreamResult(stream, "image/png");
        }
    }
}

