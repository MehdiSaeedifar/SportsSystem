using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Announcement.ViewModels
{
    public class AnnouncementViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SmsText { get; set; }
        public string EmailText { get; set; }
        [AllowHtml]
        public string WebsiteText { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CompetitionId { get; set; }
        public bool HasSms { get; set; }
        public bool HasEmail { get; set; }
    }
}