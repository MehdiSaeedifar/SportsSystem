using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.CompetitionSport
{
    public class CompetitionSportAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CompetitionSport";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CompetitionSport_default",
                "CompetitionSport/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.CompetitionSport.Controllers" }
            );
        }
    }
}