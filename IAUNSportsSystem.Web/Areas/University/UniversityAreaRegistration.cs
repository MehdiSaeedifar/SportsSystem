using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.University
{
    public class UniversityAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "University";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "University_default",
                "University/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 new[] { "IAUNSportsSystem.Web.Areas.University.Controllers" }
            );
        }
    }
}