using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses
{
    public class Competitor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public DateTime BirthDate { get; set; }
        public string NationalCode { get; set; }
        public string StudentNumber { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public string InsuranceNumber { get; set; }
        public DateTime InsuranceEndDate { get; set; }
        public string UserImage { get; set; }
        public string InsuranceImage { get; set; }
        public string StudentCertificateImage { get; set; }
        public string AzmoonConfirmationImage { get; set; }
        public int StudyFieldId { get; set; }
        public virtual StudyField StudyField { get; set; }
        public int StudyFieldDegreeId { get; set; }
        public virtual StudyFieldDegree StudyFieldDegree { get; set; }
        public bool? IsApproved { get; set; }
        public int ParticipateId { get; set; }
        public string Error { get; set; }
        public virtual Participation Participate { get; set; }
        public int? DormId { get; set; }
        public virtual Dorm Dorm { get; set; }
        public string DormNumber { get; set; }
    }
}
