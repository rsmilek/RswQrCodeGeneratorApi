using QRCoder;
using SixLabors.ImageSharp;

namespace RswQrCodeGeneratorApi.QrCode.Extensions
{
    public static class PayloadExtensions
    {
        public static Image GenerateQrCode(this PayloadGenerator.Payload payload, int pixelsPerModule = 10)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrCodeData);
            
            var qrCodeAsBitmap = qrCode.GetGraphic(pixelsPerModule);
            return qrCodeAsBitmap;
        }
    }
}
