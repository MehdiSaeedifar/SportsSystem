using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.TechnicalStaffRole
{
    public class TechnicalStaffRoleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TechnicalStaffRole";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TechnicalStaffRole_default",
                "TechnicalStaffRole/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.TechnicalStaffRole.Controllers" }
            );
        }
    }
}