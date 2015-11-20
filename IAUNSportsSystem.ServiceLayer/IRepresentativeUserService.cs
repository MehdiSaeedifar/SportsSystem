using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DomainClasses;
using IAUNSportsSystem.Models;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface IRepresentativeUserService
    {
        void Add(RepresentativeUser representativeUser);
        void Edit(RepresentativeUser representativeUser);
        Task<LoginResult> Login(string email, string password);
        Task<IList<RepresentativeUserModel>> GetCompetitionSportUsersList(int competitionSportId);
        Task<bool> IsExistByEmail(string email);
        Task<IList<RepresentativeUserModel>> GetAll();
        void Delete(int representativeUserId);
        Task<IList<CompetitionRepresentativeUserModel>> GetCompetitionRepresentativeUsers(int competitionId);
        Task<IList<RegisterListDataGridModel>> GetRegisterRepresentativeUsersDataGrid(int competitionId);
        Task<string> GetFullName(int representativeUserId);
        Task<RepresentativeUserModel> Get(int representativeUserId);
        Task<RepresentativeUser> FindById(int representativeUserId);
        Task<bool> CanEditEmail(int representaiveUserId, string email);
        Task<string> GetPassword(int representativeUserId);
        void ChangePassword(int representativeUserId, string password);
        Task<string> GetUniversityName(int representativeUserId);
    }


    public class RepresentativeUserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string NationalCode { get; set; }
        public string MobileNumber { get; set; }
        public string UniversityName { get; set; }
        public int ParticipationId { get; set; }
        public bool? IsApproved { get; set; }
    }

    public class CompetitionRepresentativeUserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string UniversityName { get; set; }
        public bool IsSelected { get; set; }
    }


    public class RegisterListDataGridModel
    {
        public int RepresentativeUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string University { get; set; }
        public int UnverifiedCompetitorsCount { get; set; }
        public int UnverifiedTechnicalStaffsCount { get; set; }
    }

}
