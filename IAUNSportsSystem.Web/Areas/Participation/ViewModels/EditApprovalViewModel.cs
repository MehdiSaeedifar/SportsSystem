using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Participation.ViewModels
{
    public class EditApprovalViewModel
    {
        public int CompetitorId { get; set; }
        public bool? IsApproved { get; set; }
        [AllowHtml]
        public string Error { get; set; }
        public int? DormId { get; set; }
        public string DormNumber { get; set; }
    }
}