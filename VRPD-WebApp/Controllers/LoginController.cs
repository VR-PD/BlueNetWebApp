using System;
using System.Web.Mvc;
using VRPD_WebApp.db;
using VRPD_WebApp.Models;

namespace VRPD_WebApp.Controllers
{
    public class LoginController : Controller
    {
        private VrpdContext db = new VrpdContext();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(string ReturnUrl)
        {
            Keynum key = Session[STATICS.VISITOR_KEY] as Keynum;
            if (key == null || (DateTime.Now - key.Created).TotalSeconds < 30)
            {
                Guest g = db.Guest.Add(new Guest());
                db.SaveChanges();

                Session[STATICS.VISITOR_KEY] = new Keynum(g.Keynum, g.Visited);
            }
            ViewBag.ReturnUrl = ReturnUrl;
            if (ReturnUrl == null)
                ViewBag.ReturnUrl = "/";
            return View();
        }

        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult Logout()
        {
            Session[STATICS.VISITOR_KEY] = null;
            return RedirectToActionPermanent("Index");
        }
    }
}
