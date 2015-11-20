using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer.EntityFramework
{
    public class CompetitionRepresentativeUserService : ICompetitionRepresentativeUserService
    {
        private readonly IDbSet<CompetitionRepresentativeUser> _competitionRepresentativeUsers;
        private readonly IDbContext _dbContext;

        public CompetitionRepresentativeUserService(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _competitionRepresentativeUsers = dbContext.Set<CompetitionRepresentativeUser>();
        }

        public async Task Add(int competitionId, IList<int> representativeUsers)
        {
            _competitionRepresentativeUsers.RemoveRange(await
                _competitionRepresentativeUsers.Where(cru => cru.CompetitionId == competitionId).ToListAsync());

            if (representativeUsers != null && representativeUsers.Any())
            {
                foreach (var representativeUserId in representativeUsers)
                {
                    _competitionRepresentativeUsers.Add(new CompetitionRepresentativeUser()
                    {
                        CompetitionId = competitionId,
                        RepresentativeUserId = representativeUserId
                    });
                }
            }

        }


        public async Task<bool> CanUserReadiness(int competitionId, int representativeUserId)
        {
            var currentDate = DateTime.Now;

            return
                await
                    _competitionRepresentativeUsers.AnyAsync(
                        cru =>
                            cru.CompetitionId == competitionId && cru.RepresentativeUserId == representativeUserId &&
                            (cru.Competition.IsReadyActive &&
                             cru.Competition.ReadyStartDate.Value <= currentDate &&
                             currentDate <= cru.Competition.ReadyEndDate.Value));
        }






        public async Task<CompetitionRepresentativeUserUniversity> GetCompetition(int competitionId, int representativeUserId)
        {
            return
                await
                    _competitionRepresentativeUsers.Where(
                        cru => cru.CompetitionId == competitionId && cru.RepresentativeUserId == representativeUserId)
                        .Select(cru => new CompetitionRepresentativeUserUniversity
                        {
                            CompetitionName = cru.Competition.Name,
                            University = cru.RepresentativeUser.University.Name
                        }).FirstOrDefaultAsync();
        }
    }
}
