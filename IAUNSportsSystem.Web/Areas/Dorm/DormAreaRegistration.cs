using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Dorm
{
    public class DormAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Dorm";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Dorm_default",
                "Dorm/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.Dorm.Controllers" }
            );
        }
    }
}