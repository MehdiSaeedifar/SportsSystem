using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Register
{
    public class RegisterAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Register";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Register_default",
                "Register/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.Register.Controllers" }
            );
        }
    }
}