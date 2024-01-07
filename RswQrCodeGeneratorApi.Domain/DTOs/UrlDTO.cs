using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System;

namespace RswQrCodeGeneratorApi.Domain.DTOs
{
    public class UrlDTO
    {
        [OpenApiProperty(Nullable = false, Default = "https://github.com/JPlenert/QRCoder-ImageSharp", Description = "The URL text value")]
        public string? Url { get; set; }
    }
}
