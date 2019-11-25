using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using VRPDWebApp.db;
using VRPDWebApp.Utils;

namespace VRPDWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly Entities db = new Entities();

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetQrDownload(string gameName)
        {
            using (QRCodeGenerator gen = new QRCodeGenerator())
            {
                Uri blob = new BlobConnector().GetGameSAS(gameName);

                // Create a qr code with a url to the download of the scanner app
                QRCodeData qrData = gen.CreateQrCode(new PayloadGenerator.Url(blob.ToString()), QRCodeGenerator.ECCLevel.M);
                return GenerateQrCode(qrData);
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Info(int id)
        {
            return View(db.GameOverview.Find(id));
        }

        [HttpGet]
        public ActionResult Overview()
        {
            return View(db.GameOverview.ToList());
        }

        public FileContentResult RenderImage(int id)
        {
            GameOverview go = db.GameOverview.Find(id);
            if (go != null)
                return File(go.Image, "image/png");
            else
                return null;
        }

        private byte[] BitmapToBytes(Bitmap img)
        {
            byte[] buffer;
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                buffer = stream.ToArray();
            }
            return buffer;
        }

        private FileContentResult GenerateQrCode(QRCodeData qrData)
        {
            using (QRCode qrCode = new QRCode(qrData))
            {
                Bitmap qrImage = qrCode.GetGraphic(8, Color.Black, Color.White, true);
                byte[] bytes = BitmapToBytes(qrImage);
                return new FileContentResult(bytes, "image/jpeg");
            }
        }
    }
}
