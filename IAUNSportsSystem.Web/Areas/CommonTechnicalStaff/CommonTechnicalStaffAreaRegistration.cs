using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.CommonTechnicalStaff
{
    public class CommonTechnicalStaffAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CommonTechnicalStaff";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CommonTechnicalStaff_default",
                "CommonTechnicalStaff/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 new[] { "IAUNSportsSystem.Web.Areas.CommonTechnicalStaff.Controllers" }
            );
        }
    }
}