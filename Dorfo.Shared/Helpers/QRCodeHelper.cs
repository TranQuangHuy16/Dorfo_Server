using QRCoder;
using System;

namespace Dorfo.Shared.Helpers
{
    public static class QRCodeHelper
    {
        /// <summary>
        /// Tạo QR code từ string và trả về dưới dạng byte[]
        /// </summary>
        public static byte[] GenerateQRCodeAsBytes(string content)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);

            return qrCode.GetGraphic(20); // Kích thước điểm ảnh QR code
        }
    }
}
