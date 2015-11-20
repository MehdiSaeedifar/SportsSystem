using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Areas.Register.ViewModels;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.Register.Controllers
{
    [SiteAuthorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly IRepresentativeUserService _representativeUserService;
        private readonly IParticipationService _participationService;
        private readonly IDormService _dormService;
        private readonly ICompetitorService _competitorService;
        private readonly ITechnicalStaffService _technicalStaffService;
        private readonly ICommonTechnicalStaffService _commonTechnicalStaffService;
        private readonly ICompetitionService _competitionService;
        private readonly ICompetitionRepresentativeUserService _competitionRepresentativeUserService;

        public AdminController(IDbContext dbContext, IRepresentativeUserService representativeUserService, IParticipationService participationService, IDormService dormService, ICompetitorService competitorService, ITechnicalStaffService technicalStaffService, ICommonTechnicalStaffService commonTechnicalStaffService, ICompetitionService competitionService,
            ICompetitionRepresentativeUserService competitionRepresentativeUserService)
        {
            _dbContext = dbContext;
            _representativeUserService = representativeUserService;
            _participationService = participationService;
            _dormService = dormService;
            _competitorService = competitorService;
            _technicalStaffService = technicalStaffService;
            _commonTechnicalStaffService = commonTechnicalStaffService;
            _competitionService = competitionService;
            _competitionRepresentativeUserService = competitionRepresentativeUserService;
        }

        // GET: Register/Admin

        public async Task<ActionResult> GetRepresentativeUsers(int competitionId)
        {
            return Json(await _representativeUserService.GetRegisterRepresentativeUsersDataGrid(competitionId)
            , JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetSportsList(int competitionId, int representativeUserId)
        {
            return Json(new
            {
                University = await _representativeUserService.GetUniversityName(representativeUserId),
                CompetitionName = await _competitionService.GetName(competitionId),
                UnverifiedCommonTechnicalStaffsCount = await _commonTechnicalStaffService.GetUnverifiedCommonTechnicalStaffsCount(competitionId, representativeUserId),
                SportsList = await _participationService.GetRegisterSportsDataGrid(competitionId, representativeUserId)
            }
            , JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetCompetitorEditData(int competitorId)
        {
            return Json(new
            {
                Competitor = await _competitorService.GetCompetitor(competitorId),
                DormsList = await _dormService.GetAll()
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetTechnicalStaffsList(int participationId)
        {
            return Json(await _participationService.GetTechnicalStaffsList(participationId), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetTechnicalStaffEditData(int technicalStaffId, int participationId)
        {
            return Json(new
            {
                TechnicalStaff = await _technicalStaffService.Get(technicalStaffId, participationId),
                DormsList = await _dormService.GetAll()
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditTechnicalStaffApproval(TechnicalStaffApprovalViewModel technicalStaffModel)
        {
            _technicalStaffService.EditApproval(new DomainClasses.TechnicalStaff()
           {
               Id = technicalStaffModel.Id,
               DormId = technicalStaffModel.DormId,
               DormNumber = technicalStaffModel.DormNumber,
               Error = technicalStaffModel.Error,
               IsApproved = technicalStaffModel.IsApproved
           });

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        public async Task<ActionResult> GetCommonTechnicalStaffsList(int competitionId, int representativeUserId)
        {
            return Json(new
            {
                TechnicalStaffs = await _commonTechnicalStaffService.GetCommonTechnicalStaffsList(competitionId, representativeUserId),
                Competition = await _competitionRepresentativeUserService.GetCompetition(competitionId, representativeUserId),
                RepresentativeUserId = representativeUserId,
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetCommonTechnicalStaffEditData(int technicalStaffId)
        {
            return Json(new
            {
                TechnicalStaff = await _technicalStaffService.GetCommonTechnicalStaff(technicalStaffId),
                DormsList = await _dormService.GetAll()
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetTeamColor(int participationId)
        {
            return Json(await _participationService.GetTeamColorList(participationId), JsonRequestBehavior.AllowGet);
        }
    }
}