using System.Web.Mvc;
using VRPD_WebApp.Models;

namespace VRPD_WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new RequireHttpsAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Authorizer());
        }
    }
}
