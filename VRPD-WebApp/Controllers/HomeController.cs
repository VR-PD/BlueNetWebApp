using System.Web.Mvc;

namespace VRPD_WebApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Overview()
        {
            return View();
        }
    }
}
