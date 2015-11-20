using System;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer.EntityFramework;
using IAUNSportsSystem.Web.ViewModels;
using Postal;
using Scheduler;

namespace IAUNSportsSystem.Web.WebTasks
{
    public class DeniedUsersNotificationTask : ScheduledTaskTemplate
    {
        /// <summary>
        /// اگر چند جاب در يك زمان مشخص داشتيد، اين خاصيت ترتيب اجراي آن‌ها را مشخص خواهد كرد
        /// </summary>
        public override int Order
        {
            get { return 2; }
        }

        public override bool RunAt(DateTime utcNow)
        {
            if (this.IsShuttingDown || this.Pause)
                return false;

            var now = utcNow.AddHours(3.5);
            return now.Minute == 30 && now.Second == 1;
        }

        public async override void Run()
        {
            if (this.IsShuttingDown || this.Pause)
                return;

            var db = new SportsSystemDbContext();

            var participationService = new ParticipationService(db);

            var representativeUsers = await participationService.GetDeniedUsersNotification();



            //Prepare Postal classes to work outside of ASP.NET request
            var viewsPath = Path.GetFullPath(HostingEnvironment.MapPath(@"~/Views/Emails"));
            var engines = new ViewEngineCollection();
            engines.Add(new FileSystemRazorViewEngine(viewsPath));



            var emailService = new EmailService(engines);

            foreach (var competition in representativeUsers)
            {
                var email = new DeniedUsersNotifications
                {
                    ViewName = "DeniedUsers",
                    From = "info@iaun.com",
                    EndDate = competition.EndDate,
                    CompetitionName = competition.CompetitionName,
                    Subject = string.Format("رفع اشکالات مشخصات وارد شده {0}", competition.CompetitionName),
                    To = competition.Email,
                    FirstName = competition.FirstName,
                    LastName = competition.LastName,
                    University = competition.University,
                    DeniedCompetitors = competition.DeniedCompetitors,
                    DeniedTechnicalStaffs = competition.DeniedTechnicalStaffs
                };

                await emailService.SendAsync(email);
            }

            db.Dispose();
        }

        public override string Name
        {
            get { return "تهيه پشتيبان"; }
        }
    }
}