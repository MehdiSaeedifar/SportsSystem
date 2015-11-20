using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.ServiceLayer.EntityFramework;
using IAUNSportsSystem.Utilities;
using IAUNSportsSystem.Web.DependencyResolution;
using IAUNSportsSystem.Web.ViewModels;
using Postal;
using Scheduler;
using StructureMap;

namespace IAUNSportsSystem.Web.WebTasks
{
    public class CompetitionsNotificationsTask : ScheduledTaskTemplate
    {
        /// <summary>
        /// اگر چند جاب در يك زمان مشخص داشتيد، اين خاصيت ترتيب اجراي آن‌ها را مشخص خواهد كرد
        /// </summary>
        public override int Order
        {
            get { return 1; }
        }

        public override bool RunAt(DateTime utcNow)
        {
            if (this.IsShuttingDown || this.Pause)
                return false;

            var now = utcNow.AddHours(3.5);

            return now.Hour == 10 && now.Minute == 5 && now.Second == 1;
        }

        public async override void Run()
        {
            if (this.IsShuttingDown || this.Pause)
                return;


            using (var db = new SportsSystemDbContext())
            {

                //Prepare Postal classes to work outside of ASP.NET request
                var viewsPath = Path.GetFullPath(HostingEnvironment.MapPath(@"~/Views/Emails"));

                var engines = new ViewEngineCollection
                {
                    new FileSystemRazorViewEngine(viewsPath)
                };

                var emailService = new EmailService(engines);

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

            }

        }

        public override string Name
        {
            get { return "Competition Notifications"; }
        }
    }

}