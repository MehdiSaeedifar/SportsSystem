using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Reporting;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.UI.WebControls;
using DNTProfiler.Common.Toolkit;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer.EntityFramework;
using IAUNSportsSystem.Utilities;
using IAUNSportsSystem.Web.DependencyResolution;
using IAUNSportsSystem.Web.Filters;
using IAUNSportsSystem.Web.ViewModels;
using Postal;

namespace IAUNSportsSystem.Web.Controllers
{
    [SiteAuthorize(Roles = "admin")]
    public class TestController : Controller
    {
        // GET: Test

        private readonly IParticipationService _participationService;
        private readonly ITechnicalStaffService _technicalStaffService;
        private readonly ICompetitionService _competitionService;

        public TestController(IParticipationService participationService, ITechnicalStaffService technicalStaffService, ICompetitionService competitionService)
        {
            _participationService = participationService;
            _technicalStaffService = technicalStaffService;
            _competitionService = competitionService;
        }


        public async Task<ActionResult> Index()
        {
            var model = await _competitionService.GetCompetitionsForCardPrint(Convert.ToInt32(User.Identity.Name));

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Send()
        {

            var viewsPath = Path.GetFullPath(HostingEnvironment.MapPath(@"~/Views/Emails"));

            var engines = new ViewEngineCollection
            {
                new FileSystemRazorViewEngine(viewsPath)
            };

            var emailService = new EmailService(engines);

            using (var db = new SportsSystemDbContext())
            {


                var competitionService = new CompetitionService(db);

                var readyCompetitions = await competitionService.GetReadyCompetitionsNotificationAsync();

                foreach (var readyCompetition in readyCompetitions)
                {
                    var email = new CompetitionNotificationEmail
                    {
                        ViewName = "ReadyCompetition",
                        From = "info@iaun.com",
                        StartDate = readyCompetition.StartDate.Value,
                        EndDate = readyCompetition.EndDate.Value,
                        CompetitionName = readyCompetition.CompetitionName,
                        Subject = string.Format("فراخوان برای اعلام آمادگی در {0}", readyCompetition.CompetitionName),
                        SiteUrl = IaunSportsSystemApp.GetSiteRootUrl()
                    };

                    foreach (var representativeUser in readyCompetition.RepresentativeUsers)
                    {
                        email.To = representativeUser.Email;
                        email.FirstName = representativeUser.FirstName;
                        email.LastName = representativeUser.LastName;
                        email.University = representativeUser.University;
                        email.Password = EncryptionHelper.Decrypt(representativeUser.Password, EncryptionHelper.Key);
                        await emailService.SendAsync(email);
                    }

                }


                var registerCompetitions = await competitionService.GetRegisterCompetitionsNotification();

                foreach (var registerCompetition in registerCompetitions)
                {
                    var email = new CompetitionNotificationEmail
                    {
                        ViewName = "RegisterCompetition",
                        From = "info@iaun.com",
                        StartDate = registerCompetition.StartDate.Value,
                        EndDate = registerCompetition.EndDate.Value,
                        CompetitionName = registerCompetition.CompetitionName,
                        Subject = string.Format("فراخوان برای ثبت نام در {0}", registerCompetition.CompetitionName),
                        SiteUrl = IaunSportsSystemApp.GetSiteRootUrl()
                    };

                    foreach (var representativeUser in registerCompetition.RepresentativeUsers)
                    {
                        email.To = representativeUser.Email;
                        email.FirstName = representativeUser.FirstName;
                        email.LastName = representativeUser.LastName;
                        email.University = representativeUser.University;
                        email.Password = EncryptionHelper.Decrypt(representativeUser.Password, EncryptionHelper.Key);
                        await emailService.SendAsync(email);
                    }

                }


                var printCardCompetitions = await competitionService.GetPrintCardCompetitionsNotification();

                foreach (var printCardCompetition in printCardCompetitions)
                {
                    var email = new CompetitionNotificationEmail
                    {
                        ViewName = "PrintCardCompetition",
                        From = "info@iaun.com",
                        StartDate = printCardCompetition.StartDate.Value,
                        EndDate = printCardCompetition.EndDate.Value,
                        CompetitionName = printCardCompetition.CompetitionName,
                        Subject = string.Format("فراخوان برای چاپ کارت  {0}", printCardCompetition.CompetitionName),
                        SiteUrl = IaunSportsSystemApp.GetSiteRootUrl()
                    };

                    foreach (var representativeUser in printCardCompetition.RepresentativeUsers)
                    {
                        email.To = representativeUser.Email;
                        email.FirstName = representativeUser.FirstName;
                        email.LastName = representativeUser.LastName;
                        email.University = representativeUser.University;
                        email.Password = EncryptionHelper.Decrypt(representativeUser.Password, EncryptionHelper.Key);
                        await emailService.SendAsync(email);
                    }
                }

                return Content("ok");
            }
        }
    }
}