using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Card
{
    public class CardAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Card";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Card_default",
                "Card/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "IAUNSportsSystem.Web.Areas.Card.Controllers" }
            );
        }
    }
}