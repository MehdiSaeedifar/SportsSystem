using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;
using System.Threading.Tasks;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.Utilities.HtmlCleaner;
using IAUNSportsSystem.Web.Areas.Announcement.ViewModels;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.Announcement.Controllers
{
    [SiteAuthorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly ICompetitionService _competitionService;
        private readonly IAnnouncementService _announcementService;
        public AdminController(IDbContext dbContext, ICompetitionService competitionService, IAnnouncementService announcementService)
        {
            _dbContext = dbContext;
            _competitionService = competitionService;
            _announcementService = announcementService;
        }

        public async Task<ActionResult> GetAll(int competitionId)
        {
            return Json(await _competitionService.GetCompetitionAnnouncementsList(competitionId),
                JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Add(AnnouncementViewModel announcementModel)
        {
            var announcement = new DomainClasses.Announcement()
            {
                EmailText = announcementModel.EmailText,
                SmsText = announcementModel.SmsText,
                Title = announcementModel.Title,
                WebsiteText = announcementModel.WebsiteText.ToSafeHtml(),
                CompetitionId = announcementModel.CompetitionId,
                CreatedDate = DateTime.Now,
                HasEmail = announcementModel.HasEmail,
                HasSms = announcementModel.HasSms
            };

            _announcementService.Add(announcement);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> Edit(AnnouncementViewModel announcementModel)
        {
            var selectedAnnouncement = await _announcementService.Find(announcementModel.Id);

            selectedAnnouncement.SmsText = announcementModel.SmsText;
            selectedAnnouncement.Title = announcementModel.Title;
            selectedAnnouncement.WebsiteText = announcementModel.WebsiteText;
            selectedAnnouncement.EmailText = announcementModel.EmailText;
            selectedAnnouncement.CreatedDate = DateTime.Now;
            selectedAnnouncement.HasSms = announcementModel.HasSms;
            selectedAnnouncement.HasEmail = announcementModel.HasEmail;

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> GetEditData(int announcementId)
        {
            return Json(await _announcementService.Get(announcementId), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Delete(int announcementId)
        {
            _announcementService.Delete(announcementId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> GetAddData(int competitionId)
        {
            return Json(await _competitionService.GetName(competitionId), JsonRequestBehavior.AllowGet);
        }

    }
}