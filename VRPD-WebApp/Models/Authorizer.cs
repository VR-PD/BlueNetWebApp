using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using VRPD_WebApp.db;

namespace VRPD_WebApp.Models
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class Authorizer : FilterAttribute, IAuthorizationFilter
    {
        private VrpdContext db = new VrpdContext();

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            if (skipAuthorization)
                return;

            Keynum key = filterContext.HttpContext.Session[STATICS.VISITOR_KEY] as Keynum;
            if (!IsValid(key))
            {
                // Unauthorized!
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        private bool IsValid(Keynum key)
        {
            if (key == null)
                return false;

            return db.Guest.ToList().FirstOrDefault(g => g.Keynum.SequenceEqual(key.Key))?.IsConfirmed ?? false;
            //return db.Guest.ToList().FirstOrDefault(g => g.Keynum.Count() == key.Key.Count() && g.Keynum.Intersect(key.Key).Count() == g.Keynum.Count())?.IsConfirmed ?? false;
        }
    }
}
