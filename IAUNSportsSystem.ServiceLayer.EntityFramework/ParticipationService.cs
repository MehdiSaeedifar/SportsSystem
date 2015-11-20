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
    public class ParticipationService : IParticipationService
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<Participation> _participates;

        public ParticipationService(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _participates = dbContext.Set<Participation>();
        }

        public void Add(Participation participate)
        {
            _participates.Add(participate);
        }




        public async Task<IList<CompetitonReportModel>> GetReportAsync(int competitionId)
        {
            return
                await
                    _participates.Where(p => p.PresentedSport.CompetitionId == competitionId && p.PresentedSport.IsIndividual == false)
                        .Select(p => new CompetitonReportModel()
                        {
                            CompetitionName = p.PresentedSport.Competition.Name,
                            Gender = p.PresentedSport.Gender.ToString(),
                            SportDetail = p.PresentedSport.SportDetail.Name,
                            SportName = p.PresentedSport.Sport.Name,
                            SportCategory = p.PresentedSport.SportCategory.Name,
                            UniversityName = p.RepresentativeUser.University.Name,
                            MaxCompetitors = p.PresentedSport.MaxCompetitors,
                            Competitors = p.Competitors.Where(c => c.IsApproved == true).Select(c => new CompetitorsReportModel
                            {

                                FullName = c.FirstName + " " + c.LastName,
                                FatherName = c.FatherName,
                                BirthDate = c.BirthDate,
                                StudentNumber = c.StudentNumber,
                                StudyField = c.StudyField.Name,
                                NationalCode = c.NationalCode,
                                InsuranceNumber = c.InsuranceNumber,
                                Image = c.UserImage
                            }).ToList(),
                            TechnicalStaves = p.ParticipationTechnicalStaves.Where(pts => pts.TechnicalStaff.IsApproved == true).Select(pts => new TechnicalStaffReportModel
                            {
                                Image = pts.TechnicalStaff.Image,
                                Role = pts.TechnicalStaffRole.Name,
                                FatherName = pts.TechnicalStaff.FatherName,
                                FullName = pts.TechnicalStaff.FirstName + " " + pts.TechnicalStaff.LastName,
                                NationalCode = pts.TechnicalStaff.NationalCode
                            }).ToList()
                        }).ToListAsync();
        }



        public async Task<IList<IndividualSportCompetitonReportModel>> GetIndividualSportCompetitonReport(int competitionId)
        {
            return
                await
                    _participates.Where(p => p.PresentedSport.CompetitionId == competitionId && p.PresentedSport.IsIndividual == true)
                        .GroupBy(x => new { x.PresentedSport })
                        .Select(p => new IndividualSportCompetitonReportModel()
                        {
                            CompetitionName = p.Key.PresentedSport.Competition.Name,
                            Gender = p.Key.PresentedSport.Gender.ToString(),
                            SportDetail = p.Key.PresentedSport.SportDetail.Name,
                            SportName = p.Key.PresentedSport.Sport.Name,
                            SportCategory = p.Key.PresentedSport.SportCategory.Name,
                            MaxCompetitors = p.Key.PresentedSport.MaxCompetitors,
                            Competitors = p.Key.PresentedSport.Participates.SelectMany(p2 => p2.Competitors.Where(c => c.IsApproved == true).Select(c => new IndividualSportCompetitorReportModel
                            {
                                FullName = c.FirstName + " " + c.LastName,
                                FatherName = c.FatherName,
                                BirthDate = c.BirthDate,
                                StudentNumber = c.StudentNumber,
                                StudyField = c.StudyField.Name,
                                NationalCode = c.NationalCode,
                                InsuranceNumber = c.InsuranceNumber,
                                Image = c.UserImage,
                                University = c.Participate.RepresentativeUser.University.Name

                            }).ToList()).ToList(),
                            TechnicalStaves = p.Key.PresentedSport.Participates.SelectMany(p2 => p2.ParticipationTechnicalStaves.Where(pts => pts.TechnicalStaff.IsApproved == true).Select(pts => new IndividualSportTechnicalStaffReportModel
                            {
                                Image = pts.TechnicalStaff.Image,
                                Role = pts.TechnicalStaffRole.Name,
                                FatherName = pts.TechnicalStaff.FatherName,
                                FullName = pts.TechnicalStaff.FirstName + " " + pts.TechnicalStaff.LastName,
                                NationalCode = pts.TechnicalStaff.NationalCode,
                                University = pts.Participation.RepresentativeUser.University.Name
                            }).ToList()).ToList()
                        }).ToListAsync();
        }



        public async Task<bool> ClearUserParticipations(int userId, int competitionId)
        {
            var participationsList = await _participates.Where(p => p.RepresentativeUserId == userId && p.PresentedSport.CompetitionId == competitionId)
                .ToListAsync();

            _participates.RemoveRange(participationsList);

            return true;
        }


        public void AddRange(IList<Participation> participations)
        {
            _participates.AddRange(participations);
        }


        public async Task<int> GetMaxCompetitorNumber(int participationId)
        {
            return
                await
                    _participates.Where(p => p.Id == participationId)
                        .Select(p => p.PresentedSport.MaxCompetitors)
                        .FirstOrDefaultAsync();
        }


        public async Task ApproveParticipation(int userId, int competitionSportId)
        {
            var participationId =
                await
                    _participates.Where(
                        p => p.PresentedSportId == competitionSportId && p.RepresentativeUserId == userId)
                        .Select(p => p.Id)
                        .SingleAsync();

            var participation = new Participation() { Id = participationId, IsApproved = true };

            _participates.Attach(participation);

            _dbContext.Entry(participation).Property(p => p.IsApproved).IsModified = true;


        }


        public async Task RejectParticipation(int userId, int competitionSportId)
        {
            var participationId =
                await
                    _participates.Where(
                        p => p.PresentedSportId == competitionSportId && p.RepresentativeUserId == userId)
                        .Select(p => p.Id)
                        .SingleAsync();

            var participation = new Participation() { Id = participationId, IsApproved = false };

            _participates.Attach(participation);

            _dbContext.Entry(participation).Property(p => p.IsApproved).IsModified = true;
        }


        public async Task<ParticipationModel> GetPartricipationForCompetitiorsList(int participationId)
        {
            return await _participates.Where(p => p.Id == participationId)
                .Select(p => new ParticipationModel()
                {
                    Id = p.Id,
                    MaxCompetitors = p.PresentedSport.MaxCompetitors,
                    CompetitionName = p.PresentedSport.Competition.Name,
                    University = p.RepresentativeUser.University.Name,
                    Sport = p.PresentedSport.Sport.Name + " " + p.PresentedSport.SportCategory.Name + " " + p.PresentedSport.SportDetail.Name,
                    Gender = p.PresentedSport.Gender,
                    CompetitionId = p.PresentedSport.CompetitionId,
                    RepresentativeUserId = p.RepresentativeUserId,
                    IsIndividual = p.PresentedSport.IsIndividual,
                    Competitors = p.Competitors.Where(c => c.ParticipateId == p.Id).Select(c => new ParicipationCompetitorModel()
                    {
                        Id = c.Id,
                        FirstName = c.FirstName,
                        IsApproved = c.IsApproved,
                        NationalCode = c.NationalCode,
                        LastName = c.LastName,
                        UserImage = c.UserImage
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<IList<ApprovedParticipationModel>> GetApprovedCompetitionParticipations(int competitionId, int representativeUserId)
        {
            return
                await _participates.Where(p => p.PresentedSport.CompetitionId == competitionId && p.RepresentativeUserId == representativeUserId && p.IsApproved == true)
                    .Select(p => new ApprovedParticipationModel
                    {
                        Id = p.Id,
                        SportName = p.PresentedSport.Sport.Name,
                        SportCategory = p.PresentedSport.SportCategory.Name,
                        SportDetail = p.PresentedSport.SportDetail.Name,
                        Gender = p.PresentedSport.Gender,
                        MaxCompetitors = p.PresentedSport.MaxCompetitors,
                        MaxTechnicalStaff = p.PresentedSport.MaxTechnicalStaff,
                        CompetitorsCount = p.Competitors.Count,
                        TechnicalStaffsCount = p.ParticipationTechnicalStaves.Count
                    }).ToListAsync();


        }







        public async Task<int> GetCompetitorsCount(int participationId)
        {
            return await _participates.Where(p => p.Id == participationId)
                .Select(p => p.Competitors.Count).FirstOrDefaultAsync();
        }


        public async Task<CompetitionSportModel> GetCompetitionSport(int participationId, int representativeUserId)
        {
            return await _participates.Where(p => p.Id == participationId && p.RepresentativeUserId == representativeUserId)
                .Select(p => new CompetitionSportModel()
                {
                    SportName = p.PresentedSport.Sport.Name,
                    SportDetail = p.PresentedSport.SportDetail.Name,
                    SportCategory = p.PresentedSport.SportCategory.Name,
                    Gender = p.PresentedSport.Gender.ToString(),
                    Id = p.Id,
                    MaxCompetitors = p.PresentedSport.MaxCompetitors,
                    CompetitorsCount = p.Competitors.Count,
                    MaxTechnicalStaffs = p.PresentedSport.MaxTechnicalStaff,
                    TechnicalStaffsCount = p.ParticipationTechnicalStaves.Count
                }).FirstOrDefaultAsync();
        }


        public async Task<CompetitorsListModel> GetCompetitorsList(int participationId, int representativeUserId)
        {
            return await _participates.Where(p => p.Id == participationId && p.RepresentativeUserId == representativeUserId)
                  .Select(p => new CompetitorsListModel
                  {
                      CompetitionSport = new CompetitionSportModel()
                      {
                          SportName = p.PresentedSport.Sport.Name,
                          Gender = p.PresentedSport.Gender.ToString(),
                          MaxCompetitors = p.PresentedSport.MaxCompetitors,
                          SportCategory = p.PresentedSport.SportCategory.Name,
                          SportDetail = p.PresentedSport.SportDetail.Name,
                          CompetitionId = p.PresentedSport.CompetitionId,
                          IsIndividual = p.PresentedSport.IsIndividual
                      },
                      Competitiors = p.Competitors.Select(c => new CompetitiorModel
                      {
                          NationalCode = c.NationalCode,
                          Id = c.Id,
                          BirthDate = c.BirthDate,
                          FirstName = c.FirstName,
                          LastName = c.LastName,
                          StudentNumber = c.StudentNumber,
                          StudyField = c.StudyField.Name,
                          StudyFieldDegree = c.StudyFieldDegree.Name,
                          UserImage = c.UserImage,
                          IsApproved = c.IsApproved
                      }).ToList()
                  }).FirstOrDefaultAsync();
        }


        public async Task<TechnicalStaffListModel> GetTechnicalStaffsList(int participationId)
        {
            return await _participates.Where(p => p.Id == participationId)
                  .Select(p => new TechnicalStaffListModel
                  {
                      CompetitionSport = new CompetitionSportModel()
                      {
                          SportName = p.PresentedSport.Sport.Name + " " + p.PresentedSport.SportCategory.Name + " " + p.PresentedSport.SportDetail.Name,
                          Gender = p.PresentedSport.Gender.ToString(),
                          MaxTechnicalStaffs = p.PresentedSport.MaxTechnicalStaff,
                          //SportCategory = p.PresentedSport.SportCategory.Name,
                          //SportDetail = p.PresentedSport.SportDetail.Name,
                      },
                      CompetitionId = p.PresentedSport.CompetitionId,
                      CompetitionName = p.PresentedSport.Competition.Name,
                      University = p.RepresentativeUser.University.Name,
                      RepresentativeUserId = p.RepresentativeUserId,
                      TechnicalStaffs = p.ParticipationTechnicalStaves.Select(c => new SingleTechnicalStaffModel
                      {
                          NationalCode = c.TechnicalStaff.NationalCode,
                          Id = c.TechnicalStaff.Id,
                          BirthDate = c.TechnicalStaff.BirthDate,
                          FirstName = c.TechnicalStaff.FirstName,
                          LastName = c.TechnicalStaff.LastName,
                          FatherName = c.TechnicalStaff.FatherName,
                          IsApproved = c.TechnicalStaff.IsApproved,
                          Image = c.TechnicalStaff.Image,
                          RoleName = c.TechnicalStaffRole.Name
                      }).ToList()
                  }).FirstOrDefaultAsync();
        }

        public async Task<TechnicalStaffListModel> GetTechnicalStaffsList(int participationId, int representativeUserId)
        {
            return await _participates.Where(p => p.Id == participationId && p.RepresentativeUserId == representativeUserId)
                  .Select(p => new TechnicalStaffListModel
                  {
                      CompetitionSport = new CompetitionSportModel()
                      {
                          SportName = p.PresentedSport.Sport.Name,
                          Gender = p.PresentedSport.Gender.ToString(),
                          MaxTechnicalStaffs = p.PresentedSport.MaxTechnicalStaff,
                          SportCategory = p.PresentedSport.SportCategory.Name,
                          SportDetail = p.PresentedSport.SportDetail.Name,
                          Id = p.Id,
                          CompetitionId = p.PresentedSport.CompetitionId,
                          TechnicalStaffsCount = p.ParticipationTechnicalStaves.Count
                      },
                      CompetitionName = p.PresentedSport.Competition.Name,
                      University = p.RepresentativeUser.University.Name,
                      TechnicalStaffs = p.ParticipationTechnicalStaves.Select(c => new SingleTechnicalStaffModel
                      {
                          NationalCode = c.TechnicalStaff.NationalCode,
                          Id = c.TechnicalStaff.Id,
                          BirthDate = c.TechnicalStaff.BirthDate,
                          FirstName = c.TechnicalStaff.FirstName,
                          LastName = c.TechnicalStaff.LastName,
                          FatherName = c.TechnicalStaff.FatherName,
                          IsApproved = c.TechnicalStaff.IsApproved,
                          Image = c.TechnicalStaff.Image,
                          RoleName = c.TechnicalStaffRole.Name
                      }).ToList()
                  }).FirstOrDefaultAsync();
        }


        public async Task AddTeamColor(int participationId, string[] colors)
        {
            var selectedParticipation = await _participates.Where(p => p.Id == participationId).FirstOrDefaultAsync();

            selectedParticipation.TeamColors.Clear();

            foreach (var color in colors)
            {
                selectedParticipation.TeamColors.Add(new TeamColor()
                {
                    Name = color
                });
            }
        }


        public async Task<TeamColorModel> GetTeamColors(int participationId, int representativeUserId)
        {
            return await _participates.Where(p => p.Id == participationId && p.RepresentativeUserId == representativeUserId)
                .Select(p => new TeamColorModel()
                {
                    CompetitionSport = new CompetitionSportModel()
                    {
                        SportName = p.PresentedSport.Sport.Name,
                        SportDetail = p.PresentedSport.SportDetail.Name,
                        SportCategory = p.PresentedSport.SportCategory.Name,
                        Gender = p.PresentedSport.Gender.ToString()
                    },
                    Colors = p.TeamColors.Select(c => c.Name).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<IList<RegisterParticipationDataGridModel>> GetRegisterSportsDataGrid(int competitionId, int representativeUserId)
        {
            return await _participates.Where(
                p => p.PresentedSport.CompetitionId == competitionId && p.RepresentativeUserId == representativeUserId && p.IsApproved == true)
                .Select(p => new RegisterParticipationDataGridModel
                {
                    ParticipationId = p.Id,
                    Gender = p.PresentedSport.Gender,
                    Sport =
                        p.PresentedSport.Sport.Name + " " + p.PresentedSport.SportCategory.Name + " " +
                        p.PresentedSport.SportDetail.Name,
                    MaxCompetitorsCount = p.PresentedSport.MaxCompetitors,
                    MaxTechnicalStaffsCount = p.PresentedSport.MaxTechnicalStaff,
                    TotalCompetitorsCount = p.Competitors.Count(),
                    TotalTechnicalStaffsCount = p.ParticipationTechnicalStaves.Count(),
                    UnverifiedCompetitorsCount = p.Competitors.Count(c => c.IsApproved == null),
                    UnverifiedTechnicalStaffsCount = p.ParticipationTechnicalStaves.Count(pts => pts.TechnicalStaff.IsApproved == null)
                }).ToListAsync();
        }

        public async Task<IList<DeniedUserNotification>> GetDeniedUsersNotification()
        {
            return await _participates.Where(p => p.IsApproved == true).GroupBy(p => new { p.PresentedSport.Competition, p.RepresentativeUser, })
                  .Select(x => new DeniedUserNotification
                  {

                      University = x.Key.RepresentativeUser.University.Name,
                      CompetitionName = x.Key.Competition.Name,
                      FirstName = x.Key.RepresentativeUser.FirstName,
                      LastName = x.Key.RepresentativeUser.LastName,
                      Email = x.Key.RepresentativeUser.Email,
                      MobileNumber = x.Key.RepresentativeUser.MobileNumber,
                      EndDate = x.Key.Competition.PrintCardStartDate,
                      DeniedCompetitors = x.Key.RepresentativeUser.Participates.Where(participate => participate.PresentedSport.CompetitionId == x.Key.Competition.Id).SelectMany(participate => participate.Competitors.Where(competitor => competitor.IsApproved == false).Select(competitor => new DeniedUser
                      {
                          FirstName = competitor.FirstName,
                          LastName = competitor.LastName,
                          Error = competitor.Error,
                          Detail = competitor.Participate.PresentedSport.Sport.Name + " " + competitor.Participate.PresentedSport.SportCategory.Name + " " + competitor.Participate.PresentedSport.SportDetail.Name
                      })).ToList(),
                      DeniedTechnicalStaffs = x.Key.RepresentativeUser.Participates.Where(participate => participate.PresentedSport.CompetitionId == x.Key.Competition.Id).SelectMany(participate => participate.ParticipationTechnicalStaves.Where(pts => pts.TechnicalStaff.IsApproved == false).Select(pts => new DeniedUser
                      {
                          FirstName = pts.TechnicalStaff.FirstName,
                          LastName = pts.TechnicalStaff.LastName,
                          Error = pts.TechnicalStaff.Error,
                          Detail = pts.Participation.PresentedSport.Sport.Name +
                          " " + pts.Participation.PresentedSport.SportCategory.Name + " " + pts.Participation.PresentedSport.SportDetail.Name
                      }).ToList()).ToList()

                  }).ToListAsync();
        }

        public async Task<IList<int>> GetSimilarParticipationSport(int participationId, int representativeUserId)
        {
            var selectedParticipation = await _participates.Where(p => p.Id == participationId)
                .Select(p => new { SportId = p.PresentedSport.SportId, p.PresentedSport.Gender, CompetitionId = p.PresentedSport.CompetitionId }).FirstOrDefaultAsync();

            return await _participates.Where(p => p.RepresentativeUserId == representativeUserId && p.PresentedSport.CompetitionId == selectedParticipation.CompetitionId && p.PresentedSport.SportId == selectedParticipation.SportId &&
              p.PresentedSport.Gender == selectedParticipation.Gender)
                .Select(p => p.Id)
                .ToListAsync();
        }

        public async Task<IList<int>> GetTechnicalStaffSimilarParticipationSport(int participationId, int technicalStaffId)
        {
            var selectedParticipation = await _participates.Where(p => p.Id == participationId)
                .Select(p => new { SportId = p.PresentedSport.SportId, p.PresentedSport.Gender, CompetitionId = p.PresentedSport.CompetitionId }).FirstOrDefaultAsync();

            return await _participates.Where(p => p.PresentedSport.CompetitionId == selectedParticipation.CompetitionId && p.PresentedSport.SportId == selectedParticipation.SportId &&
              p.PresentedSport.Gender == selectedParticipation.Gender && p.ParticipationTechnicalStaves.Any(pts => pts.TechnicalStaffId == technicalStaffId))
                .Select(p => p.Id)
                .ToListAsync();
        }



        public async Task<int> GetCompetitionId(int participationId)
        {
            return
                await
                    _participates.Where(p => p.Id == participationId)
                        .Select(p => p.PresentedSport.CompetitionId)
                        .FirstOrDefaultAsync();
        }

        public async Task<bool> CanAddCompetitor(int participationId, int representativeUserId)
        {

            var currentDate = DateTime.Now;

            return
                await
                    _participates.Where(p => p.Id == participationId).AnyAsync(
                        p => p.IsApproved == true &&
                            p.RepresentativeUserId == representativeUserId && p.Competitors.Count < p.PresentedSport.MaxCompetitors &&
                                    (p.PresentedSport.Competition.IsRegisterActive && p.PresentedSport.Competition.RegisterStartDate.Value <=
                                                                                          currentDate &&
                                                                                          currentDate <=
                                                                                         p.PresentedSport.Competition.RegisterEndDate.Value));
        }


        public async Task<bool> CanEditPerson(int participationId, int representativeUserId)
        {

            var currentDate = DateTime.Now;

            return
                await
                    _participates.Where(p => p.Id == participationId).AnyAsync(
                        p => p.IsApproved == true &&
                            p.RepresentativeUserId == representativeUserId &&
                                    (p.PresentedSport.Competition.IsRegisterActive && p.PresentedSport.Competition.RegisterStartDate.Value <=
                                                                                          currentDate &&
                                                                                          currentDate <=
                                                                                         p.PresentedSport.Competition.RegisterEndDate.Value));
        }

        public async Task<bool> CanEditRejectedPerson(int participationId, int representativeUserId)
        {

            var currentDate = DateTime.Now;

            return
                await
                    _participates.Where(p => p.Id == participationId).AnyAsync(
                        p => p.IsApproved == true &&
                            p.RepresentativeUserId == representativeUserId &&
                                    (p.PresentedSport.Competition.RegisterStartDate.Value <=
                                                                                          currentDate &&
                                                                                          currentDate <=
                                                                                      p.PresentedSport.Competition.PrintCardStartDate.Value));
        }


        public async Task<bool> CanDeleteCompetitor(int competitiorId, int representativeUserId)
        {
            var currentDate = DateTime.Now;

            return
                await
                    _participates.Where(p => p.Competitors.Any(c => c.Id == competitiorId)).AnyAsync(
                        p => p.IsApproved == true &&
                            p.RepresentativeUserId == representativeUserId &&
                                    (p.PresentedSport.Competition.IsRegisterActive && p.PresentedSport.Competition.RegisterStartDate.Value <=
                                                                                          currentDate &&
                                                                                          currentDate <=
                                                                                         p.PresentedSport.Competition.RegisterEndDate.Value));
        }

        public async Task<bool> CanDeleteTechnicalStaff(int technicalStaffId, int representativeUserId)
        {
            var currentDate = DateTime.Now;

            return
                await
                    _participates.Where(p => p.ParticipationTechnicalStaves.Any(pts => pts.TechnicalStaffId == technicalStaffId)).AnyAsync(
                        p => p.IsApproved == true &&
                            p.RepresentativeUserId == representativeUserId &&
                                    (p.PresentedSport.Competition.IsRegisterActive && p.PresentedSport.Competition.RegisterStartDate.Value <=
                                                                                          currentDate &&
                                                                                          currentDate <=
                                                                                         p.PresentedSport.Competition.RegisterEndDate.Value));
        }


        public async Task<bool> CanAddTechnicalStaff(int participationId, int representativeUserId)
        {

            var currentDate = DateTime.Now;

            return
                await
                    _participates.Where(p => p.Id == participationId).AnyAsync(
                        p => p.IsApproved == true &&
                            p.RepresentativeUserId == representativeUserId && p.ParticipationTechnicalStaves.Count < p.PresentedSport.MaxTechnicalStaff &&
                                    (p.PresentedSport.Competition.IsRegisterActive && p.PresentedSport.Competition.RegisterStartDate.Value <=
                                                                                          currentDate &&
                                                                                          currentDate <=
                                                                                         p.PresentedSport.Competition.RegisterEndDate.Value));
        }





        public async Task<IList<string>> GetTeamColorList(int participationId)
        {
            return
                await
                    _participates.Where(p => p.Id == participationId)
                        .SelectMany(p => p.TeamColors.Select(tc => tc.Name))
                        .ToListAsync();
        }
    }
}
