using System.Web.Mvc;
using VRPDWebApp.Models;

namespace VRPDWebApp
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new RequireHttpsAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Authorizer());
        }
    }
}
