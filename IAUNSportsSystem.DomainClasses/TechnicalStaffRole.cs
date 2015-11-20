using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses
{
    public class TechnicalStaffRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCommon { get; set; }
        public virtual ICollection<ParticipationTechnicalStaff> ParticipationTechnicalStaves { get; set; }
    }
}
