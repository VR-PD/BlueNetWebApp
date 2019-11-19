using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using VRPDWebApp.db;
using VRPDWebApp.Models;
using VRPDWebApp.Utils;

namespace VRPDWebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly Entities db = new Entities();

        [AllowAnonymous]
        [OutputCache(Duration = 0)]
        public ActionResult GetQrCode()
        {
            QRModel qrInfo = Session[STATICS.VISITOR_KEY] as QRModel;
            if (qrInfo == null)
                return null;

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                // Create a qr code from the base 64 string of the serialized data
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(Convert.ToBase64String(Serializer.ToByteArray(qrInfo.ToArray())), QRCodeGenerator.ECCLevel.L);
                return GenerateQrCode(qrCodeData);
            }
        }

        [AllowAnonymous]
        public ActionResult GetQrDownload()
        {
            using (QRCodeGenerator gen = new QRCodeGenerator())
            {
                // Create a qr code with a url to the download of the scanner app
                QRCodeData qrData = gen.CreateQrCode(new PayloadGenerator.Url("https://google.com/"), QRCodeGenerator.ECCLevel.M);
                return GenerateQrCode(qrData);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            // Check if session has qr data stored
            QRModel qrInfo = Session[STATICS.VISITOR_KEY] as QRModel;

            if (qrInfo == null || (DateTime.UtcNow - qrInfo.Created).TotalSeconds > 30)
            {
                // If qr data is missing or timed out, create a new guest
                Guest g = db.Guest.Add(new Guest());
                db.SaveChanges();
                qrInfo = new QRModel(g.Keynum, g.Visited, "");

                Session[STATICS.VISITOR_KEY] = qrInfo;
            }

            return View();
        }

        /// <summary>
        /// Logout client by removing them from the database and deleting session data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult Logout()
        {
            QRModel k = Session[STATICS.VISITOR_KEY] as QRModel;
            IEnumerable<Guest> r = db.Guest.ToList().Where(g => g.Keynum.SequenceEqual(k.GetKeynum()));
            db.Guest.RemoveRange(r);
            db.SaveChanges();

            Session[STATICS.VISITOR_KEY] = null;
            return RedirectToActionPermanent("Index");
        }

        /// <summary>
        /// This method is for converting bitmap into a byte array
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
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
                Bitmap qrImage = qrCode.GetGraphic(8, Color.Black, Color.LightGray, true);
                byte[] bytes = BitmapToBytes(qrImage);
                return new FileContentResult(bytes, "image/jpeg");
            }
        }
    }
}
