using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Competitor
{
    public class CompetitorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Competitor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Competitor_default",
                "Competitor/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.Competitor.Controllers" }
            );
        }
    }
}