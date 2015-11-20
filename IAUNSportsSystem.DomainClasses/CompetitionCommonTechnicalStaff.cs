using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses
{
    public class CompetitionCommonTechnicalStaff
    {
        public int Id { get; set; }
        public int RepresentativeUserId { get; set; }
        public RepresentativeUser RepresentativeUser { get; set; }
        public int CompetitonId { get; set; }
        public Competition Competition { get; set; }
        public int TechnicalStaffId { get; set; }
        public TechnicalStaff TechnicalStaff { get; set; }
        public int TechnicalStaffRoleId { get; set; }
        public TechnicalStaffRole TechnicalStaffRole { get; set; }
    }
}
