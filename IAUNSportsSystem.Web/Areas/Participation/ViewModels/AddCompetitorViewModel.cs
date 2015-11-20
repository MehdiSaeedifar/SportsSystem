using System;

namespace IAUNSportsSystem.Web.Areas.Participation.ViewModels
{
    public class AddCompetitorViewModel
    {
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
        public string AzmoonConfirmationImage { get; set; }
        public string InsuranceImage { get; set; }
        public string StudentCertificateImage { get; set; }
        public int StudyFieldId { get; set; }
        public int StudyFieldDegreeId { get; set; }
        public int ParticipationId { get; set; }
    }
}