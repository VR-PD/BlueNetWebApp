using System;
using System.Web.Mvc;
using VRPD_WebApp.db;
using VRPD_WebApp.Models;
using QRCoder;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace VRPD_WebApp.Controllers
{
    public class LoginController : Controller
    {
        private VrpdContext db = new VrpdContext();

        [AllowAnonymous]
        public ActionResult GetQrCode()
        {
            Keynum key = Session[STATICS.VISITOR_KEY] as Keynum;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(Convert.ToBase64String(key.Key), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(10);
            var bitmapBytes = BitmapToBytes(qrCodeImage); //Convert bitmap into a byte array
            return new FileContentResult(bitmapBytes, "image/jpeg"); //Return as file result
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            #region Check key code

            Keynum key = Session[STATICS.VISITOR_KEY] as Keynum;
            if (key == null || (DateTime.UtcNow - key.Created).TotalSeconds > 30)
            {
                Guest g = db.Guest.Add(new Guest());
                db.SaveChanges();
                key = new Keynum(g.Keynum, g.Visited);
                Session[STATICS.VISITOR_KEY] = key;
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
            Keynum k = Session[STATICS.VISITOR_KEY] as Keynum;
            IEnumerable<Guest> r = db.Guest.ToList().Where(g => g.Keynum.SequenceEqual(k.Key));
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
