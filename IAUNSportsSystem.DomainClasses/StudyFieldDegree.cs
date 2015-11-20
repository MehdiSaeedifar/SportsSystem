using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses
{
    public class StudyFieldDegree
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Competitor> Competitors { get; set; }
    }
}
