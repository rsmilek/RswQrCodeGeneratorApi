using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace RswQrCodeGeneratorApi.Domain.DTOs
{
    public class EmailDTO
    {
        [OpenApiProperty(Nullable = false, Default = "example@example.com", Description = "Enter Email address")]
        public string Email { get; set; } = null!;

        [OpenApiProperty(Nullable = false, Default = "Subject", Description = "Enter Email subject")]
        public string Subject { get; set; } = null!;

        [OpenApiProperty(Nullable = true, Default = "Message", Description = "Enter Email message")]
        public string? Message { get; set; }
    }
}
