using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.StudyField
{
    public class StudyFieldAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "StudyField";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "StudyField_default",
                "StudyField/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 new[] { "IAUNSportsSystem.Web.Areas.StudyField.Controllers" }
            );
        }
    }
}