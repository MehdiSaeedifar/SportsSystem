using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IAUNSportsSystem.DomainClasses
{
    public class SportDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SportId { get; set; }
        [JsonIgnore]
        public virtual Sport Sport { get; set; }
        [JsonIgnore]
        public virtual ICollection<CompetitionSport> PresentedSports { get; set; }
    }
}
