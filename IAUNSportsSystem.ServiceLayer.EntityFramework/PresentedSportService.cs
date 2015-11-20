using IAUNSportsSystem.DomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DataLayer;

namespace IAUNSportsSystem.ServiceLayer.EntityFramework
{
    public class PresentedSportService : IPresentedSportService
    {
        private readonly IDbSet<CompetitionSport> _competitionSports;

        public PresentedSportService(IDbContext dbContext)
        {
            _competitionSports = dbContext.Set<CompetitionSport>();
        }

        public void Add(CompetitionSport presentedSport)
        {
            _competitionSports.Add(presentedSport);
        }


        public async Task<IList<PresentedSportModel>> GetAllAsync(int competitionId, int representativeUserId)
        {
            return
                await
                    _competitionSports.Where(ps => ps.CompetitionId == competitionId)
                        .Select(cs => new PresentedSportModel
                        {
                            Id = cs.Id,
                            Gender = cs.Gender,
                            Name = cs.Sport.Name,
                            SportCategory = cs.SportCategory.Name,
                            SportDetail = cs.SportDetail.Name,
                            MaxCompetitors = cs.MaxCompetitors,
                            MaxTechnicalStaff = cs.MaxTechnicalStaff,
                            IsParticipated = cs.Participates
                            .Any(p => p.RepresentativeUserId == representativeUserId &&
                                p.PresentedSport.CompetitionId == competitionId)

                        }).ToListAsync();
        }
    }
}
