using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Areas.Competition.ViewModels;
using System.Threading.Tasks;
using IAUNSportsSystem.Utilities.HtmlCleaner;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.Competition.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly ICompetitionService _competitionService;

        public HomeController(IDbContext dbContext, ICompetitionService competitionService)
        {
            _dbContext = dbContext;
            _competitionService = competitionService;
        }

        public async Task<ActionResult> GetAll()
        {
            return Json(await _competitionService.GetAll(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult Add()
        {
            return PartialView();
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> GetEditData(int competitionId)
        {
            return Json(await _competitionService.Get(competitionId), JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Add(CompetitionViewModel competitionModel)
        {
            if (!string.IsNullOrEmpty(competitionModel.LogoImage))
            {
                var tmpPath = Server.MapPath("~/App_Data/tmp/");
                var logoImagePath = Server.MapPath("~/App_Data/Logo_Image/");

                await
                    CopyFileAsync(tmpPath + competitionModel.LogoImage,
                        logoImagePath + competitionModel.LogoImage);
            }

            if (competitionModel.IsRegisterActive && competitionModel.IsPrintCardActive == false)
            {
                competitionModel.PrintCardStartDate = competitionModel.RegisterEndDate.Value.AddDays(3);
            }

            var competition = new DomainClasses.Competition
            {
                Name = competitionModel.Name,
                ReadyStartDate = competitionModel.ReadyStartDate,
                ReadyEndDate = competitionModel.ReadyEndDate,
                RegisterStartDate = competitionModel.RegisterStartDate,
                RegisterEndDate = competitionModel.RegisterEndDate,
                IsReadyActive = competitionModel.IsReadyActive,
                IsRegisterActive = competitionModel.IsRegisterActive,
                IsPrintCardActive = competitionModel.IsPrintCardActive,
                LogoImage = competitionModel.LogoImage,
                PrintCardEndDate = competitionModel.PrintCardEndDate,
                PrintCardStartDate = competitionModel.PrintCardStartDate,
                Rule = competitionModel.Rule.ToSafeHtml(),
                MaxCommonTechnicalStaffs = competitionModel.MaxCommonTechnicalStaffs
            };

            _competitionService.Add(competition);

            await _dbContext.SaveChangesAsync();

            return Json(new { id = competition.Id });
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Edit(CompetitionViewModel competitionModel)
        {
            //if (!ModelState.IsValid) throw null;


            var selectedCompetition = await _competitionService.Find(competitionModel.Id);

            if (selectedCompetition.LogoImage != competitionModel.LogoImage)
            {
                var tmpPath = Server.MapPath("~/App_Data/tmp/");
                var logoImagePath = Server.MapPath("~/App_Data/Logo_Image/");
                await
                    CopyFileAsync(tmpPath + competitionModel.LogoImage,
                        logoImagePath + competitionModel.LogoImage);

                try
                {
                    System.IO.File.Delete(logoImagePath + selectedCompetition.LogoImage);
                }
                catch (Exception)
                {

                }
            }

            selectedCompetition.IsPrintCardActive = competitionModel.IsPrintCardActive;
            selectedCompetition.IsReadyActive = competitionModel.IsReadyActive;
            selectedCompetition.IsRegisterActive = competitionModel.IsRegisterActive;

            selectedCompetition.LogoImage = competitionModel.LogoImage;

            selectedCompetition.Name = competitionModel.Name;
            selectedCompetition.PrintCardEndDate = competitionModel.PrintCardEndDate;
            selectedCompetition.PrintCardStartDate = competitionModel.PrintCardStartDate;
            selectedCompetition.ReadyEndDate = competitionModel.ReadyEndDate;
            selectedCompetition.ReadyStartDate = competitionModel.ReadyStartDate;

            selectedCompetition.RegisterStartDate = competitionModel.RegisterStartDate;
            selectedCompetition.RegisterEndDate = competitionModel.RegisterEndDate;
            selectedCompetition.Rule = competitionModel.Rule.ToSafeHtml();

            selectedCompetition.MaxCommonTechnicalStaffs = competitionModel.MaxCommonTechnicalStaffs;


            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> CheckUniqueName(string term)
        {
            return Json(await _competitionService.IsCometitionNameExist(term), JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> GetCompetitionListForReadiness()
        {
            return Json(await _competitionService.GetCompetitionListForReadiness(), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetReadyCompetitionsList()
        {
            return Json(await _competitionService.GetReadyCompetitionsList(Convert.ToInt32(User.Identity.Name)), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetUserReadyCompetitionsList()
        {
            return Json(await _competitionService.GetUserRgisterCompetitionsList(Convert.ToInt32(User.Identity.Name)), JsonRequestBehavior.AllowGet);
        }



        public async Task CopyFileAsync(string sourcePath, string destinationPath)
        {
            using (Stream source = System.IO.File.Open(sourcePath, FileMode.Open))
            {
                using (Stream destination = System.IO.File.Create(destinationPath))
                {
                    await source.CopyToAsync(destination);
                }
            }
        }

        public async Task<ActionResult> GetCompetitionData(int competitionId)
        {
            return Json(await _competitionService.GetCompetitionRuleAnnouncements(competitionId), JsonRequestBehavior.AllowGet);
        }

    }
}