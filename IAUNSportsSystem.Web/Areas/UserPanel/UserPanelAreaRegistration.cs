using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.UserPanel
{
    public class UserPanelAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UserPanel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "UserPanel_default",
                "UserPanel/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.UserPanel.Controllers" }
            );
        }
    }
}