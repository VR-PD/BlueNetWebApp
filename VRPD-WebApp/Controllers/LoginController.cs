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
            Guest g = db.Guest.Add(new Guest());
            db.SaveChanges();
            if (Session[STATICS.VISITOR_KEY] == null)
            {
                Visitor visitor = new Visitor(new Random().Next().ToString());
                Session[STATICS.VISITOR_KEY] = visitor.Key;
                Visitor.Visitors.Add(visitor);
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
            Visitor.Visitors.RemoveAll(v => v.Key == Session[STATICS.VISITOR_KEY].ToString());
            Session[STATICS.VISITOR_KEY] = null;
            return RedirectToActionPermanent("Index");
        }
    }
}
