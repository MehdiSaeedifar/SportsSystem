using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Register.ViewModels
{
    public class TechnicalStaffApprovalViewModel
    {
        public int Id { get; set; }
        public bool? IsApproved { get; set; }
        [AllowHtml]
        public string Error { get; set; }
        public int? DormId { get; set; }
        public string DormNumber { get; set; }
    }
}