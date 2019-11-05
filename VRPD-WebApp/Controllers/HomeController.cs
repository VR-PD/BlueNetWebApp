using System.Linq;
using System.Web.Mvc;
using VRPD_WebApp.db;

namespace VRPD_WebApp.Controllers
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
