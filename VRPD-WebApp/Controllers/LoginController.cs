using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using VRPD_WebApp.db;
using VRPD_WebApp.Utils;

namespace VRPD_WebApp.Controllers
{
    public class LoginController : Controller
    {
        private VrpdContext db = new VrpdContext();

        [AllowAnonymous]
        public ActionResult GetQrCode()
        {
            object[] key = Session[STATICS.VISITOR_KEY] as object[];
            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            byte[] b = Serializer.ToByteArray(key);

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(Convert.ToBase64String(b), QRCodeGenerator.ECCLevel.L);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(10, Color.Black, Color.LightGray, true);
            var bitmapBytes = BitmapToBytes(qrCodeImage); //Convert bitmap into a byte array
            return new FileContentResult(bitmapBytes, "image/jpeg"); //Return as file result
        }

        [AllowAnonymous]
        public ActionResult GetQrDownload()
        {
            QRCodeGenerator gen = new QRCodeGenerator();
            QRCodeData qrData = gen.CreateQrCode(new PayloadGenerator.Url("https://google.com/"), QRCodeGenerator.ECCLevel.M);
            QRCode qrCode = new QRCode(qrData);
            Bitmap qrImage = qrCode.GetGraphic(8, Color.Black, Color.LightGray, true);
            byte[] bytes = BitmapToBytes(qrImage);
            return new FileContentResult(bytes, "image/jpeg");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            #region Check key code

            object[] qRData = Session[STATICS.VISITOR_KEY] as object[];
            if (qRData == null || (DateTime.UtcNow - (DateTime)qRData[1]).TotalSeconds > 30)
            {
                Guest g = db.Guest.Add(new Guest());
                db.SaveChanges();
                qRData = new object[] { g.Keynum, DateTime.UtcNow };

                Session[STATICS.VISITOR_KEY] = qRData;
            }

            #endregion Check key code

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
            IEnumerable<Guest> r = db.Guest.ToList().Where(g => g.Keynum.SequenceEqual(k[0] as byte[]));
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
