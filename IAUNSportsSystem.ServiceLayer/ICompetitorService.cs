using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface ICompetitorService
    {
        void Add(Competitor competitor);
        Task<IList<CompetitorModel>> GetAll(int participationId);
        Task<SingleCompetitorModel> GetCompetitor(int competitorId);
        Task<SingleCompetitorModel> GetCompetitor(int competitorId,int representativeUserId);
        void EditApproval(Competitor competitor);
        void Delete(int competitiorId);
        Task<Competitor> Find(int competitorId);
        Task<IList<CompetitorCardModel>> GetCompetitorsCard(int competitionId, int representativeId);
        Task<IList<RejectedCompetitor>> GetRejectedCompetitorsList(int representativeUserId);

    }

    public class CompetitorModel
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
        public int Weight { get; set; }
        public int Height { get; set; }
        public string InsuranceNumber { get; set; }
        public DateTime InsuranceEndDate { get; set; }
        public string UserImage { get; set; }
        public string InsuranceImage { get; set; }
        public string StudentCertificateImage { get; set; }
        public string StudyField { get; set; }

        public string StudyFieldDegree { get; set; }
        public int ParticipateId { get; set; }

    }

    public class SingleCompetitorModel
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
        public string AzmoonConfirmationImage { get; set; }
        public string StudentCertificateImage { get; set; }
        public string StudyField { get; set; }
        public string StudyFieldDegree { get; set; }
        public int StudyFieldId { get; set; }
        public int StudyFieldDegreeId { get; set; }
        public int ParticipateId { get; set; }
        public bool? IsApproved { get; set; }
        public string Error { get; set; }
        public int? DormId { get; set; }
        public string DormNumber { get; set; }
    }

    public class CompetitorCardModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentNumber { get; set; }
        public string NationalCode { get; set; }
        public string University { get; set; }
        public string Dorm { get; set; }
        public string DormNumber { get; set; }
        public string Sport { get; set; }
        public string Image { get; set; }
    }

    public class RejectedCompetitor
    {
        public int Id { get; set; }
        public string CompetitionName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string Sport { get; set; }
        public string  UserImage { get; set; }
        public int ParticipationId { get; set; }
    }

   

}
