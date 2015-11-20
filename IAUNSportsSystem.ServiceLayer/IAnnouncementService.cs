using IAUNSportsSystem.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface IAnnouncementService
    {
        void Add(Announcement announcement);
        Task<Announcement> Find(int annoncementId);
        Task<AnnouncementModel> Get(int annoncementId);
        void Delete(int annoncementId);
    }

    public class AnnouncementModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SmsText { get; set; }
        public string EmailText { get; set; }
        public string WebsiteText { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CompetitionId { get; set; }
        public string CompetitionName { get; set; }
        public bool HasSms { get; set; }
        public bool HasEmail { get; set; }
    }
}
