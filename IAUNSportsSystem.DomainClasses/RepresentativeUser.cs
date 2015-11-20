using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses
{
    public class RepresentativeUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string NationalCode { get; set; }
        public string MobileNumber { get; set; }
        public int UniversityId { get; set; }
        public virtual University University { get; set; }
        public virtual ICollection<CompetitionRepresentativeUser> CompetitionRepresentativeUsers { get; set; }
        public virtual ICollection<CompetitionCommonTechnicalStaff> CompetitionCommonTechnicalStaffs { get; set; }
        public virtual ICollection<Participation> Participates { get; set; }
    }
}
