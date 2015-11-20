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

namespace IAUNSportsSystem.Web.Areas.Competition.Controllers
{
    [SiteAuthorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly ICompetitionService _competitionService;

        public AdminController(IDbContext dbContext, ICompetitionService competitionService)
        {
            _dbContext = dbContext;
            _competitionService = competitionService;
        }

        public async Task<ActionResult> Delete(int competitionId)
        {
            _competitionService.Delete(competitionId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}