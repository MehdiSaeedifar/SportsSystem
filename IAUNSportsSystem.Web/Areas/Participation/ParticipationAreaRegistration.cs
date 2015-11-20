using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Participation
{
    public class ParticipationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Participation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Participation_default",
                "Participation/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.Participation.Controllers" }
            );
        }
    }
}