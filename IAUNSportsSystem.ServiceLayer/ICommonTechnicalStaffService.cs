using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface ICommonTechnicalStaffService
    {
        void Add(TechnicalStaff technicalStaff,int representativeUserId,int competitionId,int roleId);
        void Edit(TechnicalStaff technicalStaff);
        void Delete(int technicalStaffId);
        //Task<SingleTechnicalStaffModel> GetByNationalCode(string nationalCode,int representativeUserId);
        Task<TechnicalStaff> Get(int technicalStaffId);
        Task<CompetitionCommonTechnicalStaff> GetCompetitionTechnicalStaff( int technicalStaffId,int representativeUserId);
        Task<SingleTechnicalStaffModel> GetTechnicalStaff(int technicalStaffId,int representativeUserId);
        Task<IList<SingleTechnicalStaffModel>> GetCommonTechnicalStaffsList(int competitionId, int representativeUserId);
        Task<CommonTechnicalStaffModel> Get(int competitionId, int representativeUserId);
        Task<int> GetUnverifiedCommonTechnicalStaffsCount(int competitionId, int representativeUserId);
        Task<IList<CommonTechnicalStaffReportModel>> GetCommonTechnicalStaffsReportAsync(int competitionId);
        Task<IList<RejectedTechnicalStaff>> GetRejectedTechnicalStaffsList(int representativeUserId);
        Task<int> GetCompetitionId(int technicalStaffId);
    }


    public class CommonTechnicalStaffModel
    {
        public string CompetitionName { get; set; }
        public string University { get; set; }
    }

    public class CommonTechnicalStaffCompetition
    {
        public int CompetitionId { get; set; }
        public string CompetitionName { get; set; }
        public int MaxTechnicalStaffs { get; set; }
        public int TechnicalStaffsCount { get; set; }
    }

}
