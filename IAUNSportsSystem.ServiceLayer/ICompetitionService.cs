using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DomainClasses;
using IAUNSportsSystem.Models;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface ICompetitionService
    {
        void Add(Competition competition);
        void Delete(int competitionId);
        Task<IList<CompetitionDataGridModel>> GetAll();
        Task<bool> IsCometitionNameExist(string name);
        IList<RunningCompetitionModel> GetRunningCompetitions();
        Task<bool> IsReadyRunning(int competitionId);
        Task<IList<CompetitionModel>> GetCompetitionListForReadiness();
        Task<IList<CompetitionModel>> GetReadyCompetitionsList(int representativeUserId);
        Task<CompetitionSportsListModel> GetCompetitionSportsList(int competitionId, int representativeUserId);
        Task<CompetitionSportsListModel> GetConfirmCompetitionSportsList(int competitionId, int representativeUserId);
        Task<IList<CompetitionModel>> GetUserRgisterCompetitionsList(int representativeUserId);
        Task<string> GetName(int competitionId);
        Task<CommonTechnicalStaffListModel> GetTechnicalStaffsList(int competitionId, int representativeUserId);
        Task<CompetitionSportsList> GetCompetitionSportsList(int competitionId);
        Task<SingleCompetitionModel> Get(int competitionId);
        Task<Competition> Find(int competitionId);
        Task<CommonTechnicalStaffCompetition> GetCompetition(int competitionId, int representativeUserId);
        Task<bool> CanReportCards(int competitionId, int representativeUserId);
        Task<CompetitionLogoModel> GetLogoImage(int competitionId);

        Task<IList<ReadyCompetitionEmailNotificationModel>> GetReadyCompetitionsNotificationAsync();
        IList<ReadyCompetitionEmailNotificationModel> GetReadyCompetitionsNotification();
        Task<IList<ReadyCompetitionEmailNotificationModel>> GetRegisterCompetitionsNotification();
        Task<IList<ReadyCompetitionEmailNotificationModel>> GetPrintCardCompetitionsNotification();
        Task<IList<CompetitionModel>> GetCompetitionsForCardPrint(int representativeUserId);
        Task<CompetitionAnnouncementsListModel> GetCompetitionAnnouncementsList(int competitionId);
        Task<CompetitionRuleAnnouncementsModel> GetCompetitionRuleAnnouncements(int competitionId);
        Task<bool> CanAddCommonTechnicalStaff(int competitionId, int representativeUserId);
        Task<bool> CanEditCommonTechnicalStaff(int competitionId, int representativeUserId);
        Task<bool> CanEditRejectedCommonTechnicalStaff(int competitionId, int representativeUserId);
        Task<bool> CanDeleteCommonTechncialStaff(int competitionId, int representativeUserId);
    }


    public class CompetitionLogoModel
    {
        public string Logo { get; set; }
        public string  CompetitionName { get; set; }
    }

    public class CommonTechnicalStaffListModel
    {
        public string CompetitionName { get; set; }
        public int MaxTechnicalStaffs { get; set; }
        public IList<SingleTechnicalStaffModel> TechnicalStaffs { get; set; }
    }
    //public class CompetitionGridData
    //{
    //    public int TotalItems { get; set; }
    //    public IList<CompetitionGridRow> Records { get; set; }
    //}

    public class CompetitionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CompetitionRuleAnnouncementsModel
    {
        public string CompetitionName { get; set; }
        public string Rule { get; set; }
        public IList<AnnouncementModel> Announcements { get; set; }
    }

    public class CompetitionDataGridModel
    {
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

    }


    public class CompetitionSportsListModel
    {
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
        public IList<PresentedSportModel> CompetitionSports { get; set; }
    }

    public class SingleCompetitionModel
    {
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
    }

    public class ReadyCompetitionEmailNotificationModel
    {
        public string CompetitionName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IList<RepresentativeUserNotificationModel> RepresentativeUsers { get; set; }
    }

    public class RepresentativeUserNotificationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string University { get; set; }
        public string Password { get; set; }
    }

    public class CompetitionAnnouncementsListModel
    {
        public string CompetitionName { get; set; }
        public IList<AnnouncementModel> Announcements { get; set; }
    }
}
