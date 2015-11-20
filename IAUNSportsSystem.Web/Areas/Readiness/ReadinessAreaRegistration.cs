using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Readiness
{
    public class ReadinessAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Readiness";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Readiness_default",
                "Readiness/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.Readiness.Controllers" }
            );
        }
    }
}