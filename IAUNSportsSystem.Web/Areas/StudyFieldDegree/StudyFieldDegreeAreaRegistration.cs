using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.StudyFieldDegree
{
    public class StudyFieldDegreeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "StudyFieldDegree";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "StudyFieldDegree_default",
                "StudyFieldDegree/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.StudyFieldDegree.Controllers" }
            );
        }
    }
}