using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;
using System.Threading.Tasks;

namespace IAUNSportsSystem.Web.Areas.Card.Controllers
{
    public class UserPanelController : Controller
    {
        private readonly ICompetitionService _competitionService;

        public UserPanelController(ICompetitionService competitionService)
        {
            _competitionService = competitionService;
        }

        public async Task<ActionResult> GetCardsList()
        {
            return Json(await _competitionService.GetCompetitionsForCardPrint(Convert.ToInt32(User.Identity.Name)), JsonRequestBehavior.AllowGet);
        }
    }
}