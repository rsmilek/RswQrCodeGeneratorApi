using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System;

namespace RswQrCodeGeneratorApi.Domain.DTOs
{
    public class EmailDTO
    {
        [OpenApiProperty(Nullable = true, Default = "example@example.com", Description = "Enter Email address")]
        public string Email { get; set; }

        [OpenApiProperty(Nullable = true, Default = "Subject", Description = "Enter Email subject")]
        public string Subject { get; set; }

        [OpenApiProperty(Nullable = true, Default = "Message", Description = "Enter Email message")]
        public string Message { get; set; }

    }
}
