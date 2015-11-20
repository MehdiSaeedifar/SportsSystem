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
    public class TeamColorService : ITeamColorService
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<TeamColor> _teamColors;

        public TeamColorService(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _teamColors = dbContext.Set<TeamColor>();
        }

        public async Task Add(int participationId, int representativeUserId, string[] colors)
        {
            _teamColors.RemoveRange(await _teamColors.Where(c => c.ParticipationId == participationId && c.Participation.RepresentativeUserId == representativeUserId).ToListAsync());

            foreach (var color in colors)
            {
                _teamColors.Add(new TeamColor()
                {
                    Name = color,
                    ParticipationId = participationId
                });
            }

        }

        public async Task<bool> CanAddTemColor(int participationId, int representativeUserId)
        {
            return
                await
                    _teamColors.AnyAsync(
                        t =>
                            t.ParticipationId == participationId &&
                            t.Participation.RepresentativeUserId == representativeUserId);
        }

    }
}
