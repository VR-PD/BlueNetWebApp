using System.Web.Mvc;

namespace BlueNetWebApp
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters?.Add(new RequireHttpsAttribute());
            filters?.Add(new HandleErrorAttribute());
            filters?.Add(new AuthorizerAttribute());
        }
    }
}
