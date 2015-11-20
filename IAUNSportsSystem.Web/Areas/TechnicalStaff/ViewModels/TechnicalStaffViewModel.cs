using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAUNSportsSystem.Web.Areas.TechnicalStaff.ViewModels
{
    public class TechnicalStaffViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public DateTime BirthDate { get; set; }
        public string NationalCode { get; set; }
        public int TechnicalStaffRoleId { get; set; }
        public string Image { get; set; }
        public int ParticipationId { get; set; }
        public int CompetitionId { get; set; }
        public string MobileNumber { get; set; }
    }
}