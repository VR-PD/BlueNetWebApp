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
            if (Session[STATICS.VISITOR_ID_KEY] == null)
            {
                Visitor visitor = new Visitor(new Random().Next().ToString());
                Session[STATICS.VISITOR_ID_KEY] = visitor.ID;
                Visitor.Visitors.Add(visitor);
            }

            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
    }
}
