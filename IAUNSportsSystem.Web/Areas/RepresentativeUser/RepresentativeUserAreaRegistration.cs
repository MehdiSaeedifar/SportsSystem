using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.RepresentativeUser
{
    public class RepresentativeUserAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RepresentativeUser";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RepresentativeUser_default",
                "RepresentativeUser/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.RepresentativeUser.Controllers" }
            );
        }
    }
}