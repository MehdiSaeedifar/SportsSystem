using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface IParticipationService
    {
        void Add(Participation participate);
        void AddRange(IList<Participation> participations);
        Task<IList<CompetitonReportModel>> GetReportAsync(int competitionId);

        Task<bool> ClearUserParticipations(int userId, int competitionId);
        Task<int> GetMaxCompetitorNumber(int participationId);

        Task ApproveParticipation(int userId, int competitionSportId);
        Task RejectParticipation(int userId, int competitionSportId);
        Task<ParticipationModel> GetPartricipationForCompetitiorsList(int participationId);
        Task<IList<ApprovedParticipationModel>> GetApprovedCompetitionParticipations(int competitionId, int representativeUserId);
        Task<IList<IndividualSportCompetitonReportModel>> GetIndividualSportCompetitonReport(int competitionId);
        Task<int> GetCompetitorsCount(int participationId);
        Task<CompetitionSportModel> GetCompetitionSport(int participationId, int representativeUserId);

        Task<CompetitorsListModel> GetCompetitorsList(int participationId, int representativeUserId);
        Task<TechnicalStaffListModel> GetTechnicalStaffsList(int participationId);
        Task<TechnicalStaffListModel> GetTechnicalStaffsList(int participationId, int representativeUserId);
        Task AddTeamColor(int participationId, string[] colors);
        Task<TeamColorModel> GetTeamColors(int participationId, int representativeUserId);

        Task<IList<RegisterParticipationDataGridModel>> GetRegisterSportsDataGrid(int competitionId,
            int representativeUserId);

        Task<IList<DeniedUserNotification>> GetDeniedUsersNotification();
        Task<IList<int>> GetSimilarParticipationSport(int participationId, int representativeUserId);
        Task<IList<int>> GetTechnicalStaffSimilarParticipationSport(int participationId, int technicalStaffId);
        Task<int> GetCompetitionId(int participationId);
        Task<bool> CanAddCompetitor(int participationId, int representativeUserId);
        Task<bool> CanEditPerson(int participationId, int representativeUserId);
        Task<bool> CanEditRejectedPerson(int participationId, int representativeUserId);
        Task<bool> CanDeleteCompetitor(int competitiorId, int representativeUserId);
        Task<bool> CanAddTechnicalStaff(int participationId, int representativeUserId);
        Task<bool> CanDeleteTechnicalStaff(int technicalStaffId, int representativeUserId);
        Task<IList<string>> GetTeamColorList(int participationId);
    }


    public class RegisterParticipationDataGridModel
    {
        public int ParticipationId { get; set; }
        public string Sport { get; set; }
        public Gender Gender { get; set; }
        public int MaxTechnicalStaffsCount { get; set; }
        public int MaxCompetitorsCount { get; set; }
        public int TotalTechnicalStaffsCount { get; set; }
        public int TotalCompetitorsCount { get; set; }
        public int UnverifiedCompetitorsCount { get; set; }
        public int UnverifiedTechnicalStaffsCount { get; set; }
    }


    public class ParticipationModel
    {
        public int Id { get; set; }
        public int MaxCompetitors { get; set; }
        public string CompetitionName { get; set; }
        public int CompetitionId { get; set; }
        public int RepresentativeUserId { get; set; }
        public string University { get; set; }
        public string Sport { get; set; }
        public Gender Gender { get; set; }
        public bool IsIndividual { get; set; }
        public List<ParicipationCompetitorModel> Competitors { get; set; }

    }

    public class TeamColorModel
    {
        public CompetitionSportModel CompetitionSport { get; set; }

        public IList<string> Colors { get; set; }
    }

    public class CompetitiorModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string StudentNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string StudyFieldDegree { get; set; }
        public string StudyField { get; set; }
        public string UserImage { get; set; }
        public bool? IsApproved { get; set; }
    }


    public class CompetitionSportModel
    {
        public int Id { get; set; }
        public string SportName { get; set; }
        public string SportCategory { get; set; }
        public string SportDetail { get; set; }
        public string Gender { get; set; }
        public int MaxCompetitors { get; set; }
        public int CompetitorsCount { get; set; }
        public int MaxTechnicalStaffs { get; set; }
        public int TechnicalStaffsCount { get; set; }
        public int CompetitionId { get; set; }
        public bool IsIndividual { get; set; }
    }

    public class CompetitorsListModel
    {
        public CompetitionSportModel CompetitionSport { get; set; }
        public IList<CompetitiorModel> Competitiors { get; set; }
    }

    public class TechnicalStaffListModel
    {
        public CompetitionSportModel CompetitionSport { get; set; }
        public string University { get; set; }
        public string CompetitionName { get; set; }
        public int CompetitionId { get; set; }
        public int RepresentativeUserId { get; set; }
        public IList<SingleTechnicalStaffModel> TechnicalStaffs { get; set; }
    }




    public class ParicipationCompetitorModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string UserImage { get; set; }
        public bool? IsApproved { get; set; }

    }

    public class CompetitonReportModel
    {
        public int ParticipationId { get; set; }
        public string CompetitionName { get; set; }
        public string UniversityName { get; set; }
        public string SportName { get; set; }
        public string SportCategory { get; set; }
        public string SportDetail { get; set; }
        public string Gender { get; set; }
        public int MaxCompetitors { get; set; }
        public IList<CompetitorsReportModel> Competitors { get; set; }
        public IList<TechnicalStaffReportModel> TechnicalStaves { get; set; }
    }

    public class CommonTechnicalStaffReportModel
    {
        public string CompetitionName { get; set; }
        public string UniversityName { get; set; }
        public IList<TechnicalStaffReportModel> TechnicalStaves { get; set; }
    }


    public class IndividualSportCompetitonReportModel
    {
        public int ParticipationId { get; set; }
        public string CompetitionName { get; set; }
        public string UniversityName { get; set; }
        public string SportName { get; set; }
        public string SportCategory { get; set; }
        public string SportDetail { get; set; }
        public string Gender { get; set; }
        public int MaxCompetitors { get; set; }
        public IList<IndividualSportCompetitorReportModel> Competitors { get; set; }
        public IList<IndividualSportTechnicalStaffReportModel> TechnicalStaves { get; set; }
    }

    public class CompetitorsReportModel
    {
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public DateTime BirthDate { get; set; }
        public string NationalCode { get; set; }
        public string StudyField { get; set; }
        public string StudentNumber { get; set; }
        public string InsuranceNumber { get; set; }
        public string Image { get; set; }
    }

    public class IndividualSportCompetitorReportModel : CompetitorsReportModel
    {
        public string University { get; set; }
    }

    public class TechnicalStaffReportModel
    {
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string NationalCode { get; set; }
        public string Image { get; set; }
        public string Role { get; set; }
    }

    public class IndividualSportTechnicalStaffReportModel : TechnicalStaffReportModel
    {
        public string University { get; set; }
    }

    public class ApprovedCompetitionParticipationsModel
    {
        public string CompetitionName { get; set; }
        public IList<ApprovedParticipationModel> Participations { get; set; }
    }


    public class ApprovedParticipationModel
    {
        public int Id { get; set; }
        public string SportName { get; set; }
        public string SportCategory { get; set; }
        public string SportDetail { get; set; }
        public int MaxCompetitors { get; set; }
        public int MaxTechnicalStaff { get; set; }
        public int CompetitorsCount { get; set; }
        public int TechnicalStaffsCount { get; set; }
        public Gender Gender { get; set; }
    }

    public class DeniedUserNotification
    {
        public string CompetitionName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string University { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public DateTime? EndDate { get; set; }
        public IList<DeniedUser> DeniedCompetitors { get; set; }
        public IList<DeniedUser> DeniedTechnicalStaffs { get; set; }

    }


    public class DeniedUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Detail { get; set; }
        public string Error { get; set; }
    }

}
