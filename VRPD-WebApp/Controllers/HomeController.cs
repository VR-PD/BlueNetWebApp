using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VRPDWebApp.db;

namespace VRPDWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly Entities db = new Entities();

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
    }
}
