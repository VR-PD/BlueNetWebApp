using QRCoder;
using System;
using System.Drawing;
using System.IO;
using System.Web.Mvc;

namespace BlueNetWebApp
{
    public sealed class QREncoder : IDisposable
    {
        private readonly QRCodeGenerator generator;

        public QREncoder() => generator = new QRCodeGenerator();

        public void Dispose() => generator.Dispose();

        public FileContentResult GetQRImage(PayloadGenerator.Payload payload) => GetQRImage(payload, Color.White);

        public FileContentResult GetQRImage(PayloadGenerator.Payload payload, Color backColor)
        {
            QRCodeData qRCodeData = generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.M);

            using (QRCode qrCode = new QRCode(qRCodeData))
            {
                Bitmap qrImage = qrCode.GetGraphic(8, Color.Black, backColor, true);
                byte[] bytes = BitmapToBytes(qrImage);
                return new FileContentResult(bytes, "image/jpeg");
            }
        }

        public FileContentResult GetQRImage(string text, Color backColor)
        {
            QRCodeData qRCodeData = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.M);

            using (QRCode qrCode = new QRCode(qRCodeData))
            {
                Bitmap qrImage = qrCode.GetGraphic(8, Color.Black, backColor, true);
                byte[] bytes = BitmapToBytes(qrImage);
                return new FileContentResult(bytes, "image/jpeg");
            }
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            byte[] buffer;
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                buffer = stream.ToArray();
            }
            return buffer;
        }
    }
}
