using QRCoder;
using System;
using System.Linq;
using System.Web.Mvc;
using VRPDWebApp.db;
using VRPDWebApp.Utils;

namespace VRPDWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly Entities db = new Entities();

        [HttpGet]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public ActionResult GetQrDownload(string gameName)
        {
            using (QREncoder encoder = new QREncoder())
            {
                Uri blob = new BlobConnector().GetGameSAS(gameName);

                return encoder.GetQRImage(new PayloadGenerator.Url(blob.ToString()));
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(db.GameOverview.ToList());
        }

        [HttpGet]
        public ActionResult Info(int id)
        {
            return View(db.GameOverview.Find(id));
        }

        [HttpGet]
        public FileContentResult RenderImage(int id)
        {
            GameOverview go = db.GameOverview.Find(id);
            if (go != null)
                return File(go.Image, "image/png");
            else
                return null;
        }
    }
}
