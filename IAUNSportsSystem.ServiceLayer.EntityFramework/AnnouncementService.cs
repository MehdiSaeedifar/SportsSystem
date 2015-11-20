using IAUNSportsSystem.DomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DataLayer;

namespace IAUNSportsSystem.ServiceLayer.EntityFramework
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IDbSet<Announcement> _announcements;

        public AnnouncementService(IDbContext dbContext)
        {
            _announcements = dbContext.Set<Announcement>();
        }

        public void Add(DomainClasses.Announcement announcement)
        {
            _announcements.Add(announcement);
        }

        public async Task<Announcement> Find(int annoncementId)
        {
            return await _announcements.Where(a => a.Id == annoncementId).FirstOrDefaultAsync();
        }

        public async Task<AnnouncementModel> Get(int annoncementId)
        {
            return await _announcements.Where(a => a.Id == annoncementId)
                .Select(a => new AnnouncementModel
                {
                    Id = a.Id,
                    CompetitionName = a.Competition.Name,
                    CompetitionId = a.CompetitionId,
                    EmailText = a.EmailText,
                    HasEmail = a.HasEmail,
                    HasSms = a.HasSms,
                    SmsText = a.SmsText,
                    Title = a.Title,
                    WebsiteText = a.WebsiteText
                }).FirstOrDefaultAsync();
        }

        public void Delete(int annoncementId)
        {
            var announcment = new Announcement() {Id = annoncementId};

            _announcements.Attach(announcment);

            _announcements.Remove(announcment);
        }
    }
}
