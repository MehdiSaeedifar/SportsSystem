using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses
{
    public class TeamColor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParticipationId { get; set; }
        public Participation Participation { get; set; }
    }
}
