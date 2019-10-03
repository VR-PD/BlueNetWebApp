using System.Web.Mvc;

namespace VRPD_WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return Content("Index");
        }

        [HttpGet]
        public ActionResult Overview()
        {
            return Content("Overview");
        }
    }
}
