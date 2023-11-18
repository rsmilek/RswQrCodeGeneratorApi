using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System;

namespace RswQrCodeGeneratorApi.Domain.DTO
{
    public class UrlDTO
    {
        [OpenApiProperty(Nullable = true, Default = "https://github.com/JPlenert/QRCoder-ImageSharp", Description = "The URL text value")]
        public string Url { get; set; }
    }
}
