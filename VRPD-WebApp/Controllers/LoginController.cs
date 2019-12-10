using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        [HttpGet]
        public ActionResult GetQrCode()
        {
            QRModel qrInfo = Session[STATICS.VISITORKEY] as QRModel;
            if (qrInfo == null)
                return null;

            using (QREncoder encoder = new QREncoder())
            {
                return encoder.GetQRImage(Convert.ToBase64String(Serializer.ToByteArray(qrInfo.ToArray())), Color.LightGray);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetQrDownload()
        {
            using (QREncoder encoder = new QREncoder())
            {
                Uri blob = new BlobConnector().GetScannerSAS();

                return encoder.GetQRImage(new PayloadGenerator.Url(blob.ToString()), Color.LightGray);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            // Check if session has qr data stored
            QRModel qrInfo = Session[STATICS.VISITORKEY] as QRModel;

            if (qrInfo == null || (DateTime.UtcNow - qrInfo.Created).TotalSeconds > 30)
            {
                // If qr data is missing or timed out, create a new guest
                Guest g = db.Guest.Add(new Guest());
                db.SaveChanges();
                qrInfo = new QRModel(g.Keynum, g.Visited, "");

                Session[STATICS.VISITORKEY] = qrInfo;
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
            QRModel k = Session[STATICS.VISITORKEY] as QRModel;
            IEnumerable<Guest> r = db.Guest.ToList().Where(g => g.Keynum.SequenceEqual(k.GetKeynum()));
            db.Guest.RemoveRange(r);
            db.SaveChanges();

            Session[STATICS.VISITORKEY] = null;
            return RedirectToActionPermanent("Index");
        }
    }
}
