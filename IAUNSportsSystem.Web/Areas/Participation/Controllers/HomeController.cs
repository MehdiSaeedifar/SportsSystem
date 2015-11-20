using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Reporting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IAUNSportsSystem.Web.Filters;
using IAUNSportsSystem.Web.Infrastructure;
using Microsoft.Ajax.Utilities;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace IAUNSportsSystem.Web.Areas.Participation.Controllers
{
    public partial class HomeController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly IParticipationService _participateService;
        private readonly ICompetitionService _competitionService;
        private readonly ICompetitionRepresentativeUserService _competitionRepresentativeUserService;

        public HomeController(IDbContext dbContext, ICompetitionRepresentativeUserService competitionRepresentativeUserService, IParticipationService participateService,
            ICompetitionService competitionService)
        {
            _dbContext = dbContext;
            _participateService = participateService;
            _competitionService = competitionService;
            _competitionRepresentativeUserService = competitionRepresentativeUserService;
        }

        // GET: Participate/Home
        public virtual ActionResult Index()
        {
            return PartialView();
        }



        [HttpPost]
        public virtual async Task<ActionResult> Add(int[] competitionSports, int competitionId)
        {
            int userId = Convert.ToInt32(User.Identity.Name);

            if (!await _competitionRepresentativeUserService.CanUserReadiness(competitionId, userId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            await _participateService.ClearUserParticipations(userId, competitionId);

            await _dbContext.SaveChangesAsync();

            if (competitionSports == null) return new HttpStatusCodeResult(HttpStatusCode.OK);


            var participationsList = competitionSports.Select(competitionSportId => new DomainClasses.Participation
            {
                RepresentativeUserId = userId,
                PresentedSportId = competitionSportId
            }).ToList();

            _participateService.AddRange(participationsList);

            await _dbContext.SaveChangesAsync();


            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public virtual async Task<ActionResult> GetCompetitionSportsList(int competitionId)
        {

            var competition =
                await _competitionService.GetCompetitionSportsList(competitionId, Convert.ToInt32(User.Identity.Name));

            return Json(competition, JsonRequestBehavior.AllowGet);
        }

        public virtual async Task<ActionResult> GetConfirmCompetitionSportsList(int competitionId)
        {
            var competition =
                await _competitionService.GetConfirmCompetitionSportsList(competitionId, Convert.ToInt32(User.Identity.Name));

            return Json(competition, JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Approve(int competitionSportId, int userId)
        {
            await _participateService.ApproveParticipation(userId, competitionSportId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(200);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Reject(int competitionSportId, int userId)
        {
            await _participateService.RejectParticipation(userId, competitionSportId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(200);
        }

    }
}