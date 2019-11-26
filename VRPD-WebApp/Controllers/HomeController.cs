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

        [AllowAnonymous]
        [HttpGet]
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
