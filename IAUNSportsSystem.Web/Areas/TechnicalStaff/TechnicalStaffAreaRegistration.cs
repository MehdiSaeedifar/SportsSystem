using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.TechnicalStaff
{
    public class TechnicalStaffAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TechnicalStaff";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TechnicalStaff_default",
                "TechnicalStaff/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.TechnicalStaff.Controllers" }
            );
        }
    }
}