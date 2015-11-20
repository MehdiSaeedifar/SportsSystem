using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses
{
    public class CompetitionRepresentativeUser
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }
        public int RepresentativeUserId { get; set; }
        public RepresentativeUser RepresentativeUser { get; set; }
    }
}
