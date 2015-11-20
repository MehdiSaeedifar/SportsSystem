using IAUNSportsSystem.Web.Filters;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new JsonHandlerAttribute());
            filters.Add(new ForceWww(IaunSportsSystemApp.GetSiteRootUrl()));
        }
    }
}
