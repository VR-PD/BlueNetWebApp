using System;
using System.Web.Mvc;
using VRPD_WebApp.Models;

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
