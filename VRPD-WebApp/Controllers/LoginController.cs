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
            Guest g = null;
            if (Session[STATICS.VISITOR_KEY] == null)
            {
                g = db.Guest.Add(new Guest());
                db.SaveChanges();

                Session[STATICS.VISITOR_KEY] = g.Keynum;
                Session[STATICS.VISITOR_KEY_STR] = Convert.ToBase64String(g.Keynum);
            }
            ViewBag.ReturnUrl = ReturnUrl;
            if (ReturnUrl == null)
                ViewBag.ReturnUrl = "/";
            return View(g);
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
