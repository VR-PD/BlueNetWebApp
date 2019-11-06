using System;
using System.Linq;
using System.Web.Mvc;
using VRPD_WebApp.db;

namespace VRPD_WebApp.Models
{
    [OutputCache(Duration = 0)]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class Authorizer : FilterAttribute, IAuthorizationFilter
    {
        private Entities db = new Entities();

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            if (skipAuthorization)
                return;

            QRModel key = filterContext.HttpContext.Session[STATICS.VISITOR_KEY] as QRModel;
            Guest found = null;
            if (!IsValid(key, ref found) && false)
            {
                // Unauthorized!
                filterContext.Result = new HttpUnauthorizedResult();
                if (found != null)
                {
                    // No second chances, remove invalid record
                    db.Guest.Remove(found);
                    filterContext.HttpContext.Session[STATICS.VISITOR_KEY] = null;
                    db.SaveChanges();
                }
            }
        }

        private bool IsValid(QRModel key, ref Guest found)
        {
            if (key == null)
                return false;

            found = db.Guest.ToList().FirstOrDefault(g => g.Keynum.SequenceEqual(key.Keynum));

            return found?.IsConfirmed ?? false;
        }
    }
}
