using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.News
{
    public class NewsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "News";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "News_default",
                "News/{controller}/{action}/{id}/{title}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional, title = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.News.Controllers" }
            );
        }
    }
}