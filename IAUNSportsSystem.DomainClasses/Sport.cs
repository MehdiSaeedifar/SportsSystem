using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IAUNSportsSystem.DomainClasses
{
    public class Sport
    {
        public Sport()
        {
            SportCategories = new HashSet<SportCategory>();
            SportDetails = new HashSet<SportDetail>();
        }


        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<CompetitionSport> PresentedSports { get; set; }
        [JsonIgnore]
        public virtual ICollection<SportCategory> SportCategories { get; set; }
        [JsonIgnore]
        public virtual ICollection<SportDetail> SportDetails { get; set; }
    }
}
