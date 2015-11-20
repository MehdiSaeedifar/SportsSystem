using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using System.Threading.Tasks;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.CompetitionRepresentativeUser.Controllers
{

    [SiteAuthorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly IRepresentativeUserService _representativeUserService;
        private readonly ICompetitionService _competitionService;
        private readonly ICompetitionRepresentativeUserService _competitionRepresentativeUserService;

        public AdminController(IDbContext dbContext, IRepresentativeUserService representativeUserService, ICompetitionService competitionService, ICompetitionRepresentativeUserService competitionRepresentativeUserService)
        {
            _dbContext = dbContext;
            _representativeUserService = representativeUserService;
            _competitionService = competitionService;
            _competitionRepresentativeUserService = competitionRepresentativeUserService;
        }

        // GET: CompetitionRepresentativeUser/Admin
        public async Task<ActionResult> GetAll(int competitionId)
        {
            return Json(new
            {
                RepresentativeUsers = await _representativeUserService.GetCompetitionRepresentativeUsers(competitionId),
                CompetitionName = await _competitionService.GetName(competitionId)
            },
                JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Add(int competitionId, IList<int> representativeUsers)
        {
            await _competitionRepresentativeUserService.Add(competitionId, representativeUsers);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}