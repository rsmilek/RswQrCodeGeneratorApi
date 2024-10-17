using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
namespace RswQrCodeGeneratorApi.Domain.DTOs
{
    public class CzPaymentDTO
    {
        [OpenApiProperty(Nullable = true, Default = "", Description = "Enter bank account's prefix")]
        public string? Prefix { get; set; }

        [OpenApiProperty(Nullable = false, Default = "1599211019", Description = "Enter bank account's number")]
        public string Account { get; set; } = null!;

        [OpenApiProperty(Nullable = false, Default = "3030", Description = "Enter bank account's bank code")]
        public string Bank { get; set; } = null!;

        [OpenApiProperty(Nullable = false, Default = "1.00", Description = "Enter amount")]
        public string Amount { get; set; } = null!;

        [OpenApiProperty(Nullable = true, Default = "", Description = "Enter variable symbol")]
        public string? VariableSymbol { get; set; }

        [OpenApiProperty(Nullable = true, Default = "", Description = "Enter specific symbol")]
        public string? SpecificSymbol { get; set; }

        [OpenApiProperty(Nullable = true, Default = "", Description = "Enter constant symbol")]
        public string? ConstantSymbol { get; set; }
            
        [OpenApiProperty(Nullable = true, Default = "", Description = "Enter message")]
        public string? Message { get; set; }
    }
}
