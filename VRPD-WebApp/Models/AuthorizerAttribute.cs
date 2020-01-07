using BlueNetWebApp.db;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BlueNetWebApp
{
    [OutputCache(Duration = 0)]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class AuthorizerAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = (filterContext?.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ?? false)
                || (filterContext?.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ?? false);

            if (skipAuthorization)
                return;

            QRModel key = filterContext.HttpContext.Session[Statics.Visitorkey] as QRModel;
            Guest found = null;

            using (Entities db = new Entities())
            {
                if (!IsValid(key, ref found, db))
                {
                    // Unauthorized!
                    filterContext.Result = new HttpUnauthorizedResult();
                    if (found != null)
                    {
                        // No second chances, remove invalid record
                        db.Guest.Remove(found);
                        filterContext.HttpContext.Session[Statics.Visitorkey] = null;
                        db.SaveChanges();
                    }
                }
            }
        }

        private bool IsValid(QRModel key, ref Guest found, Entities db)
        {
            if (key == null)
                return false;

            found = db.Guest.ToList().FirstOrDefault(g => g.Keynum.SequenceEqual(key.GetKeynum()));

            return found?.IsConfirmed ?? false;
        }
    }
}
