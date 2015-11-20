using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface ICompetitionRepresentativeUserService
    {
        Task Add(int competitionId, IList<int> representativeUsers);
        Task<bool> CanUserReadiness(int competitionId, int representativeUserId);
        Task<CompetitionRepresentativeUserUniversity> GetCompetition(int competitionId, int representativeUserId);

    }

    public class CompetitionRepresentativeUserUniversity
    {
        public string CompetitionName { get; set; }
        public string University { get; set; }
    }
  
}
