using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses
{
    public class TechnicalStaff
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public DateTime BirthDate { get; set; }
        public string NationalCode { get; set; }
        public string Image { get; set; }
        public string MobileNumber { get; set; }
        public bool? IsApproved { get; set; }
        public string Error { get; set; }
        public virtual ICollection<ParticipationTechnicalStaff> Participations { get; set; }
        public virtual ICollection<CompetitionCommonTechnicalStaff> CompetitionCommonTechnicalStaffs { get; set; }
        public int? DormId { get; set; }
        public virtual Dorm Dorm { get; set; }
        public string DormNumber { get; set; }
    }
}
