using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.Readiness.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRepresentativeUserService _representativeUserService;
        private readonly ICompetitionSportService _competitionSportService;

        public AdminController(IRepresentativeUserService representativeUserService, ICompetitionSportService competitionSportService)
        {
            _representativeUserService = representativeUserService;
            _competitionSportService = competitionSportService;
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> GetCompetitionSportUsersList(int competitionSportId)
        {
            return Json(await _competitionSportService.GetCompetitionSportRepresentativeUsersList(competitionSportId),
                JsonRequestBehavior.AllowGet);
        }
    }
}