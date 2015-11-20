using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DomainClasses.Configuration;

namespace IAUNSportsSystem.DomainClasses
{
    public class Participation
    {
        public int Id { get; set; }
        public int RepresentativeUserId { get; set; }
        public virtual RepresentativeUser RepresentativeUser { get; set; }
        public int PresentedSportId { get; set; }
        public virtual CompetitionSport PresentedSport { get; set; }
        public virtual ICollection<Competitor> Competitors { get; set; }
        public virtual ICollection<ParticipationTechnicalStaff> ParticipationTechnicalStaves { get; set; }
        public bool? IsApproved { get; set; }
        public virtual ICollection<TeamColor> TeamColors { get; set; }
    }
}
