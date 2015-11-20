using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.CompetitionRepresentativeUser
{
    public class CompetitionRepresentativeUserAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CompetitionRepresentativeUser";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CompetitionRepresentativeUser_default",
                "CompetitionRepresentativeUser/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.CompetitionRepresentativeUser.Controllers" }
            );
        }
    }
}