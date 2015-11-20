using System.Web.Mvc;
using IAUNSportsSystem.Web.Infrastructure;

namespace IAUNSportsSystem.Web.PersianCaptcha
{
    public class NoBrowserCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.DisableBrowserCache();
            base.OnResultExecuting(filterContext);
        }
    }

}