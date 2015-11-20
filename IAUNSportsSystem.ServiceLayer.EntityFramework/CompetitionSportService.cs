using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer.EntityFramework
{
    public class CompetitionSportService : ICompetitionSportService
    {
        private readonly IDbSet<CompetitionSport> _competitionSports;

        public CompetitionSportService(IDbContext dbContext)
        {
            _competitionSports = dbContext.Set<CompetitionSport>();
        }

        public void Add(CompetitionSport competitionSport)
        {
            _competitionSports.Add(competitionSport);
        }


        public async Task<IList<CompetitionSportReadinessModel>> GetCompetitionSportListForReadiness(int competitionId)
        {
            return
                await
                    _competitionSports.AsNoTracking().Where(cs => cs.CompetitionId == competitionId)
                        .Select(cs => new CompetitionSportReadinessModel
                        {
                            Id = cs.Id,
                            Name = cs.Sport.Name + " " + cs.SportCategory.Name + " " + cs.SportDetail.Name,
                            ReadinessNumber = cs.Participates.Count(),
                            ApprovedNumber = cs.Participates.Count(p => p.IsApproved == true),
                            RejectedNumber = cs.Participates.Count(p => p.IsApproved == false),
                            Gender = cs.Gender
                        })
                        .ToListAsync();

        }


        public async Task<IList<ApprovedParticipationModel>> GetApprovedCompetitionParticipations(int competitionId, int representativeUserId)
        {
            return
                await _competitionSports.Where(cs => cs.CompetitionId == competitionId && cs.Participates.Any(p => p.RepresentativeUserId == representativeUserId && p.IsApproved == true))
                    .Select(cs => new ApprovedParticipationModel
                    {
                        Id = cs.Participates.Where(p => p.PresentedSportId == cs.Id).Select(p => p.Id).FirstOrDefault(),
                        SportName = cs.Sport.Name,
                        SportCategory = cs.SportCategory.Name,
                        SportDetail = cs.SportDetail.Name,
                        Gender = cs.Gender,
                        MaxCompetitors = cs.MaxCompetitors,
                        MaxTechnicalStaff = cs.MaxTechnicalStaff
                    }).ToListAsync();


        }

        public void Delete(int competitionId)
        {
            var competitionSport = new CompetitionSport()
            {
                Id = competitionId
            };

            _competitionSports.Attach(competitionSport);
            _competitionSports.Remove(competitionSport);
        }

        public async Task<SignleCompetitionSportModel> GetEditData(int competitionSportId)
        {
            return
              await
                  _competitionSports.Where(cs => cs.Id == competitionSportId)
                      .Select(cs => new SignleCompetitionSportModel
                      {
                          HasRule = cs.HasRule,
                          IsIndividual = cs.IsIndividual,
                          Rule = cs.Rule
                      })
                      .FirstOrDefaultAsync();
        }

        public async Task<CompetitionSport> Find(int competitionSportId)
        {
            return await _competitionSports.FirstOrDefaultAsync(cs => cs.Id == competitionSportId);
        }


        public async Task<CompetitionSportRepresentativeUsersModel> GetCompetitionSportRepresentativeUsersList(int competitionSportId)
        {
            return
                await _competitionSports.Where(c => c.Id == competitionSportId)
                    .Select(c => new CompetitionSportRepresentativeUsersModel
                    {
                        CompetitionId = c.Competition.Id,
                        CompetitionName = c.Competition.Name,
                        SportName = c.Sport.Name + " " + c.SportCategory.Name + " " + c.SportDetail.Name,
                        Gender = c.Gender,
                        RepresentativeUsers = c.Participates.Select(p => new RepresentativeUserModel()
                        {
                            Id = p.RepresentativeUser.Id,
                            Email = p.RepresentativeUser.Email,
                            FirstName = p.RepresentativeUser.FirstName,
                            LastName = p.RepresentativeUser.LastName,
                            FatherName = p.RepresentativeUser.FatherName,
                            MobileNumber = p.RepresentativeUser.MobileNumber,
                            NationalCode = p.RepresentativeUser.NationalCode,
                            UniversityName = p.RepresentativeUser.University.Name,
                            IsApproved = p.IsApproved,
                            ParticipationId = p.Id
                        }).ToList()
                    }).FirstOrDefaultAsync();
        }
    }
}
