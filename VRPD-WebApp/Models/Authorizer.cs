using System;
using System.Linq;
using System.Web.Mvc;
using VRPD_WebApp.db;

namespace VRPD_WebApp.Models
{
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
            if (!IsValid(key))
            {
                // Unauthorized!
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        private bool IsValid(QRModel key)
        {
            if (key == null)
                return false;

            return db.Guest.ToList().FirstOrDefault(g => g.Keynum.SequenceEqual(key.Keynum))?.IsConfirmed ?? false;
        }
    }
}
