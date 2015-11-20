using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface ICompetitionSportService
    {
        void Add(CompetitionSport competitionSport);
        Task<IList<CompetitionSportReadinessModel>> GetCompetitionSportListForReadiness(int competitionId);
        void Delete(int competitionId);
        //Task<SignleCompetitionSportModel> GetAddData(int competitionId);
        Task<SignleCompetitionSportModel> GetEditData(int competitionSportId);
        Task<CompetitionSport> Find(int competitionSportId);
        Task<CompetitionSportRepresentativeUsersModel> GetCompetitionSportRepresentativeUsersList(int competitionSportId);
    }


    public class CompetitionSportsList
    {
        public int CompetitionId { get; set; }
        public string CompetitionName { get; set; }
        public IList<SignleCompetitionSportModel> CompetitionSports { get; set; }
    }

    public class SignleCompetitionSportModel
    {
        public int Id { get; set; }
        public string SportName { get; set; }
        public Sport Sport { get; set; }
        public SportCategory SportCategory { get; set; }
        public SportDetail SportDetail { get; set; }
        public string Gender { get; set; }
        public int MaxCompetitors { get; set; }
        public int MaxTechnicalStaffs { get; set; }
        public string Rule { get; set; }
        public bool HasRule { get; set; }
        public bool IsIndividual { get; set; }
    }

    public class CompetitionSportReadinessModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public int ReadinessNumber { get; set; }
        public int ApprovedNumber { get; set; }
        public int RejectedNumber { get; set; }

    }

    public class CompetitionSportRepresentativeUsersModel
    {
        public int CompetitionId { get; set; }
        public string CompetitionName { get; set; }
        public string SportName { get; set; }
        public Gender Gender { get; set; }
        public IList<RepresentativeUserModel> RepresentativeUsers { get; set; }
    }


}
