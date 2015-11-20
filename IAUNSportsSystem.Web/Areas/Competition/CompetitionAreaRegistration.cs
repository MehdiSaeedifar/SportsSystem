using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Competition
{
    public class CompetitionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Competition";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Competition_default",
                "Competition/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.Competition.Controllers" }
            );
        }
    }
}