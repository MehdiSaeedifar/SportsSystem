using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.SlideShow
{
    public class SlideShowAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SlideShow";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SlideShow_default",
                "SlideShow/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 new[] { "IAUNSportsSystem.Web.Areas.SlideShow.Controllers" }
            );
        }
    }
}