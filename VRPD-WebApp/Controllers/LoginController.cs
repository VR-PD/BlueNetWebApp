using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using VRPD_WebApp.db;
using VRPD_WebApp.Models;
using VRPD_WebApp.Utils;

namespace VRPD_WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly VrpdContext db = new VrpdContext();

        [AllowAnonymous]
        public ActionResult GetQrCode()
        {
            if (!(Session[STATICS.VISITOR_KEY] is object[] qrInfo))
                return null;

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                // Create a qr code from the base 64 string of the serialized data
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(Convert.ToBase64String(Serializer.ToByteArray(qrInfo)), QRCodeGenerator.ECCLevel.L);
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
            object[] qrInfo = Session[STATICS.VISITOR_KEY] as object[];
            if (qrInfo == null || (DateTime.UtcNow - (DateTime)qrInfo[QRData.Created]).TotalSeconds > 30)
            {
                // If qr data is missing or timed out, create a new guest
                Guest g = db.Guest.Add(new Guest());
                db.SaveChanges();
                qrInfo = new object[3];
                qrInfo[QRData.Keynum] = g.Keynum;
                qrInfo[QRData.Created] = g.Visited;

                Session[STATICS.VISITOR_KEY] = qrInfo;
            }

            ViewBag.ReturnUrl = "/";

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
            object[] k = Session[STATICS.VISITOR_KEY] as object[];
            IEnumerable<Guest> r = db.Guest.ToList().Where(g => g.Keynum.SequenceEqual(k[QRData.Keynum] as byte[]));
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
