using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface ITechnicalStaffService
    {
        void Add(TechnicalStaff technicalStaff, int participationId, int technicalStaffRoleId);
        void Edit(TechnicalStaff technicalStaff);
        Task Delete(int technicalStaffId, int participationId);
        Task Delete(int technicalStaffId, int[] similarParticipationIds);
        Task<SingleTechnicalStaffModel> GetByNationalCode(string nationalCode, int representativeUserId, int competitionId);
        Task<TechnicalStaff> Find(int technicalStaffId);
        Task<SingleTechnicalStaffModel> Get(int technicalStaffId, int participationId);
        Task<ParticipationTechnicalStaff> GetParticipationTechnicalStaff(int participationId, int technicalStaffId);
        Task<SingleTechnicalStaffModel> GetTechnicalStaff(int technicalStaffId, int participationId, int representativeUserId);
        Task<IList<TechnicalStaffCardModel>> GetTechnicalStaffsCards(int competitionId, int representativeUserId);
        Task<IList<TechnicalStaffCardModel>> GetCommonTechnicalStaffsCards(int competitionId, int representativeUserId);
        void EditApproval(TechnicalStaff technicalStaff);
        Task<SingleTechnicalStaffModel> GetCommonTechnicalStaff(int technicalStaffId);
        Task<IList<RejectedTechnicalStaff>> GetRejectedTechnicalStaffsList(int representativeUserId);

    }

    public class RejectedTechnicalStaff
    {
        public int Id { get; set; }
        public string CompetitionName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string Role { get; set; }
        public string Image { get; set; }
        public int ParticipationId { get; set; }
        public int CompetitionId { get; set; }
    }

    public class SingleTechnicalStaffModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public DateTime BirthDate { get; set; }
        public string NationalCode { get; set; }
        public string Image { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool? IsApproved { get; set; }
        public string Error { get; set; }
        public int? DormId { get; set; }
        public string DormName { get; set; }
        public string DormNumber { get; set; }
        public string MobileNumber { get; set; }
    }

    public class TechnicalStaffCardModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string Image { get; set; }
        public string University { get; set; }
        public string Dorm { get; set; }
        public string DormNumber { get; set; }
        public IList<string> Roles { get; set; }
    }
}
