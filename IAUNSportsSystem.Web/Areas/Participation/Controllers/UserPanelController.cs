using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using System.Threading.Tasks;
using IAUNSportsSystem.Web.Infrastructure;

namespace IAUNSportsSystem.Web.Areas.Participation.Controllers
{
    public class UserPanelController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly IParticipationService _participationService;
        private readonly ITeamColorService _teamColorService;

        public UserPanelController(IDbContext dbContext, IParticipationService participateService, ITeamColorService teamColorService)
        {
            _dbContext = dbContext;
            _participationService = participateService;
            _teamColorService = teamColorService;
        }
        public async Task<ActionResult> AddTeamColor(int participationId, string[] colors)
        {
            var userId = Convert.ToInt32(User.Identity.Name);
            if (!await _teamColorService.CanAddTemColor(participationId, userId))
            {
                return this.JsonValidationErrors();
            }

            await _teamColorService.Add(participationId, userId, colors);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> GetTeamColors(int participationId)
        {
            return Json(await _participationService.GetTeamColors(participationId, Convert.ToInt32(User.Identity.Name)), JsonRequestBehavior.AllowGet);
        }
    }
}