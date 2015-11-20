using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;
using System.Threading.Tasks;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.Web.Areas.CompetitionSport.ViewModels;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.CompetitionSport.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly ISportService _sportService;
        private readonly ICompetitionSportService _competitionSportService;
        private readonly ICompetitionService _competitionService;
        private readonly IParticipationService _participationService;

        public HomeController(IDbContext dbContext, ISportService sportService, ICompetitionSportService competitionSportService, ICompetitionService competitionService,
            IParticipationService participationService)
        {
            _dbContext = dbContext;
            _sportService = sportService;
            _competitionSportService = competitionSportService;
            _competitionService = competitionService;
            _participationService = participationService;
        }

        [SiteAuthorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Add(AddCompetitionSportViewModel competitionSportModel)
        {
            var competitionSport = new DomainClasses.CompetitionSport
            {
                Gender = competitionSportModel.Gender,
                MaxCompetitors = competitionSportModel.MaxCompetitors,
                MaxTechnicalStaff = competitionSportModel.MaxTechnicalStaffs,
                SportId = competitionSportModel.SportId,
                SportDetailId = competitionSportModel.SportDetailId,
                SportCategoryId = competitionSportModel.SportCategoryId,
                CompetitionId = competitionSportModel.CompetitionId,
                Rule = competitionSportModel.Rule,
                IsIndividual = competitionSportModel.IsIndividual,
                HasRule = competitionSportModel.HasRule,
            };

            _competitionSportService.Add(competitionSport);

            await _dbContext.SaveChangesAsync();

            return Json(competitionSport.Id);
        }

        public async Task<ActionResult> GetSportsList()
        {
            return Json(await _sportService.GetAllSportsAsync(), JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> GetAddData(int competitionId)
        {
            return Json(new
            {
                SportsList = await _sportService.GetAllSportsAsync(),
                Competition = await _competitionService.GetCompetitionSportsList(competitionId)
            }, JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> GetCompetitionSportListForReadiness(int competitionId)
        {
            return Json(await _competitionSportService.GetCompetitionSportListForReadiness(competitionId), JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetApprovedCompetitionParticipationsList(int competitionId)
        {
            return Json(new
            {
                Name = await _competitionService.GetName(competitionId),
                Participations = await _participationService.GetApprovedCompetitionParticipations(competitionId, Convert.ToInt32(User.Identity.Name))
            },
                JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int competitionSportId)
        {
            _competitionSportService.Delete(competitionSportId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> GetEditData(int competitionSportId)
        {
            return Json(await _competitionSportService.GetEditData(competitionSportId), JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Edit(AddCompetitionSportViewModel competitionSportModel)
        {
            var selectedCompetitionSport = await _competitionSportService.Find(competitionSportModel.Id);

            selectedCompetitionSport.IsIndividual = competitionSportModel.IsIndividual;

            selectedCompetitionSport.MaxTechnicalStaff = competitionSportModel.MaxTechnicalStaffs;

            selectedCompetitionSport.MaxCompetitors = competitionSportModel.MaxCompetitors;

            selectedCompetitionSport.HasRule = competitionSportModel.HasRule;

            selectedCompetitionSport.Rule = competitionSportModel.Rule;

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}