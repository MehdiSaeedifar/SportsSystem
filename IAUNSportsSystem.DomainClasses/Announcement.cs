using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SmsText { get; set; }
        public string EmailText { get; set; }
        public string WebsiteText { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool HasSms { get; set; }
        public bool HasEmail { get; set; }
        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }
    }
}
