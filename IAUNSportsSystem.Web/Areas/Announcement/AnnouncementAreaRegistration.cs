using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Announcement
{
    public class AnnouncementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Announcement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Announcement_default",
                "Announcement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.Announcement.Controllers" }
            );
        }
    }
}