using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface IPresentedSportService
    {
        void Add(CompetitionSport presentedSport);
        Task<IList<PresentedSportModel>> GetAllAsync(int competitionId,int representativeUserId);
    }

    public class PresentedSportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public string SportCategory { get; set; }
        public string SportDetail { get; set; }
        public int MaxCompetitors { get; set; }
        public int MaxTechnicalStaff { get; set; }
        public bool IsParticipated { get; set; }
        public bool HasRule { get; set; }
        public string Rule { get; set; }
    }
}
