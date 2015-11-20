using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses
{
    public class Competition
    {
        public Competition()
        {
            PresentedSports = new HashSet<CompetitionSport>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsReadyActive { get; set; }
        public bool IsRegisterActive { get; set; }
        public bool IsPrintCardActive { get; set; }
        public DateTime? ReadyStartDate { get; set; }
        public DateTime? ReadyEndDate { get; set; }
        public DateTime? RegisterStartDate { get; set; }
        public DateTime? RegisterEndDate { get; set; }
        public DateTime? PrintCardStartDate { get; set; }
        public DateTime? PrintCardEndDate { get; set; }
        public string LogoImage { get; set; }
        public string Rule { get; set; }
        public int MaxCommonTechnicalStaffs { get; set; }
        public virtual ICollection<CompetitionSport> PresentedSports { get; set; }
        public virtual ICollection<CompetitionCommonTechnicalStaff> CompetitionCommonTechnicalStaffs { get; set; }
        public virtual ICollection<CompetitionRepresentativeUser> CompetitionRepresentativeUsers { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }
    }
}
