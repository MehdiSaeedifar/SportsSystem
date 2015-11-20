using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface IParticipateService
    {
        void Add(Participate participate);
        Task<CompetitonReportModel> GetReportAsync(int competitionId);
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
        public IList<CompetitorsReportModel> Competitors { get; set; }
    }

    public class CompetitorsReportModel
    {
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string BirthDate { get; set; }
        public string IdentityNumber { get; set; }
        public string StudyField { get; set; }
        public string StudentNumber { get; set; }
        public string InsuranceNumber { get; set; }
        public string Image { get; set; }
    }
}
