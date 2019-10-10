using System;
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

            string key = filterContext.HttpContext.Session[STATICS.VISITOR_KEY] as string;
            if (!IsValid(key))
            {
                // Unauthorized!
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        private bool IsValid(string key)
        {
            var q = db.Guest.SqlQuery("SELECT * FROM Guest WHERE Keynum=@p0", key);
            return q.AnyAsync(g => g.IsConfirmed).Result;
        }
    }
}
