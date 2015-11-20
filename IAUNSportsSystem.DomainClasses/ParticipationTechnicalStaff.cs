using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses
{
    public class ParticipationTechnicalStaff
    {
        public int Id { get; set; }
        public int ParticipationId { get; set; }
        public Participation Participation { get; set; }
        public int TechnicalStaffId { get; set; }
        public TechnicalStaff TechnicalStaff { get; set; }
        public int TechnicalStaffRoleId { get; set; }
        public TechnicalStaffRole TechnicalStaffRole { get; set; }
    }
}
