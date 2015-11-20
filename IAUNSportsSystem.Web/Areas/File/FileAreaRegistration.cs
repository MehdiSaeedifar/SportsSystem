using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.File
{
    public class FileAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "File";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "File_default",
                "File/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                 new[] { "IAUNSportsSystem.Web.Areas.File.Controllers" }
            );
        }
    }
}