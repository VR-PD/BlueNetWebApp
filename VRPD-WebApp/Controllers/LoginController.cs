using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VRPD_WebApp.Models;

namespace VRPD_WebApp.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(string ReturnUrl)
        {
            if (Session[STATICS.VISITOR_KEY] == null)
            {
                Visitor visitor = new Visitor(new Random().Next().ToString());
                Session[STATICS.VISITOR_KEY] = visitor.Key;
                Visitor.Visitors.Add(visitor);
            }
            ViewBag.ReturnUrl = ReturnUrl;
            if (ReturnUrl == null)
                ViewBag.ReturnUrl = "/Home";
            return View();
        }

        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult Logout()
        {
            Visitor.Visitors.RemoveAll(v => v.Key == Session[STATICS.VISITOR_KEY].ToString() && v.IsConfirmed);
            Session[STATICS.VISITOR_KEY] = null;
            return RedirectToActionPermanent("Index");
        }
    }
}
