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
using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using static System.Net.Mime.MediaTypeNames;
using static QRCoder.PayloadGenerator;

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
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/png", bodyType: typeof(byte[]), Description = "The OK response")]
        public IActionResult QrCodeUrl(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ExecutionContext context)
        {
            //var fs = File.OpenRead(@$"C:\Projects\QrCode\R_Right.jpg");
            //return new FileStreamResult(fs, "image/jpeg");


            Url generator = new Url("https://github.com/codebude/QRCoder/");
            string payload = generator.ToString();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeAsBitmap = qrCode.GetGraphic(10);

            var memoryStream = new MemoryStream();
            qrCodeAsBitmap.SaveAsPng(memoryStream, new PngEncoder()
            {
                BitDepth = PngBitDepth.Bit1,
                ColorType = PngColorType.Grayscale
            });
            memoryStream.Position = 0; // THIS IS THE MAGIC !!!
            return new FileStreamResult(memoryStream, "image/png");

            //var fullFileName = @$"{context.FunctionAppDirectory}\___QrCodeImageTmp.jpg";
            //qrCodeAsBitmap.SaveAsPng(fullFileName, new PngEncoder()
            //{
            //    BitDepth = PngBitDepth.Bit1,
            //    ColorType = PngColorType.Grayscale
            //});

            //var memoryStream = new MemoryStream();
            //qrCodeAsBitmap.SaveAsJpeg(memoryStream);
            //return new FileStreamResult(memoryStream, "image/jpeg");

            //var memoryStream = new MemoryStream();
            //return new FileStreamResult(memoryStream, "image/jpeg");

            //var fullFileName = @$"{context.FunctionAppDirectory}\_QrCodeImageTmp.jpg";
            //if (File.Exists(fullFileName))
            //    File.Delete(fullFileName);
            //qrCodeAsBitmap.SaveAsJpeg(fullFileName);
            //var fileStream = File.OpenRead(fullFileName);
            //return new FileStreamResult(fileStream, "image/jpeg");
        }

    }
}

