using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace RswQrCodeGeneratorApi.QrCode.Extensions
{
    public static class ImageExtensions
    {
        public static MemoryStream SaveAsPngToStream(this Image image, PngEncoder pngEncoder = null)
        {
            var memoryStream = new MemoryStream();
            var encoder = pngEncoder ?? new PngEncoder()
            {
                BitDepth = PngBitDepth.Bit1,
                ColorType = PngColorType.Grayscale
            };
            image.SaveAsPng(memoryStream, encoder);
            memoryStream.Position = 0; // THIS IS THE MAGIC !!! Needed for proper stream futher...

            return memoryStream;
        }
    }
}
