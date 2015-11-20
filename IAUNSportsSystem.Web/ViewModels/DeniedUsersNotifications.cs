using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IAUNSportsSystem.ServiceLayer;
using Postal;

namespace IAUNSportsSystem.Web.ViewModels
{
    public class DeniedUsersNotifications : Email
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string CompetitionName { get; set; }
        public DateTime? EndDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string University { get; set; }
        public IList<DeniedUser> DeniedCompetitors { get; set; }
        public IList<DeniedUser> DeniedTechnicalStaffs { get; set; }
    }


}