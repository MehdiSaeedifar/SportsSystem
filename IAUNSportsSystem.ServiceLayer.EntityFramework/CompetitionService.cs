using IAUNSportsSystem.DomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using EFSecondLevelCache;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.Models;

namespace IAUNSportsSystem.ServiceLayer.EntityFramework
{
    public class CompetitionService : ICompetitionService
    {
        private readonly IDbSet<Competition> _competitions;

        public CompetitionService(IDbContext dbContext)
        {
            _competitions = dbContext.Set<Competition>();
        }

        public void Add(Competition competition)
        {
            _competitions.Add(competition);
        }



        public async Task<IList<CompetitionDataGridModel>> GetAll()
        {
            return await _competitions.Select(c => new CompetitionDataGridModel
              {
                  Id = c.Id,
                  Name = c.Name,
                  IsReadyActive = c.IsReadyActive,
                  IsRegisterActive = c.IsRegisterActive,
                  ReadyEndDate = c.ReadyEndDate,
                  RegisterStartDate = c.RegisterStartDate,
                  ReadyStartDate = c.ReadyStartDate,
                  RegisterEndDate = c.RegisterEndDate,
                  PrintCardEndDate = c.PrintCardEndDate,
                  PrintCardStartDate = c.PrintCardStartDate,
                  IsPrintCardActive = c.IsPrintCardActive
              }).ToListAsync();
        }


        public async Task<bool> IsCometitionNameExist(string name)
        {
            return await _competitions.AnyAsync(c => c.Name == name);
        }


        public IList<RunningCompetitionModel> GetRunningCompetitions()
        {
            var currentDate = DateTime.Now;


            var runningCompetitions = _competitions.AsNoTracking()
                .Where(
                    c =>
                        (c.IsReadyActive &&
                         c.ReadyStartDate.Value <= currentDate && currentDate <= c.ReadyEndDate.Value) ||
                        (c.IsRegisterActive &&
                         c.RegisterStartDate.Value <= currentDate && currentDate <= c.RegisterEndDate.Value) ||
                         (c.IsPrintCardActive &&
                         c.PrintCardStartDate.Value <= currentDate && currentDate <= c.PrintCardEndDate.Value)
                         )
                .Select(c => new RunningCompetitionModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsReadyActive = (c.IsReadyActive &&
                         c.ReadyStartDate.Value <= currentDate && currentDate <= c.ReadyEndDate.Value),
                    IsRegisterActive = (c.IsRegisterActive &&
                    c.RegisterStartDate.Value <= currentDate && currentDate <= c.RegisterEndDate.Value),
                    IsPrintCardActive = (c.IsPrintCardActive &&
                   c.PrintCardStartDate.Value <= currentDate && currentDate <= c.PrintCardEndDate.Value)
                }).Cacheable().ToList();

            return runningCompetitions;
        }


        public async Task<bool> IsReadyRunning(int competitionId)
        {
            var currentDate = DateTime.Now;
            return await _competitions.AnyAsync(c => c.Id == competitionId && c.IsReadyActive &&
                                                     c.ReadyStartDate.Value <= currentDate &&
                                                     currentDate <= c.ReadyEndDate.Value);
        }


        public async Task<IList<CompetitionModel>> GetCompetitionListForReadiness()
        {
            return await _competitions.OrderByDescending(c => c.ReadyEndDate).ThenByDescending(c => c.ReadyStartDate)
                .ThenBy(c => c.Name).Select(c => new CompetitionModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync();
        }


        public async Task<IList<CompetitionModel>> GetReadyCompetitionsList(int representativeUserId)
        {
            var currentDate = DateTime.Now;

            return await _competitions.AsNoTracking()
                .Where(
                    c => c.CompetitionRepresentativeUsers.Any(cru => cru.RepresentativeUserId == representativeUserId) &&
                        (c.IsReadyActive &&
                         c.ReadyStartDate.Value <= currentDate && currentDate <= c.ReadyEndDate.Value))
                .Select(c => new CompetitionModel()
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync();
        }




        public async Task<CompetitionSportsListModel> GetCompetitionSportsList(int competitionId, int representativeUserId)
        {
            return await _competitions.Where(c => c.Id == competitionId).Select(c => new CompetitionSportsListModel
            {
                Id = c.Id,
                Name = c.Name,
                IsReadyActive = c.IsReadyActive,
                IsRegisterActive = c.IsRegisterActive,
                ReadyEndDate = c.ReadyEndDate,
                RegisterStartDate = c.RegisterStartDate,
                ReadyStartDate = c.ReadyStartDate,
                RegisterEndDate = c.ReadyEndDate,
                IsPrintCardActive = c.IsPrintCardActive,
                PrintCardStartDate = c.PrintCardStartDate,
                PrintCardEndDate = c.PrintCardEndDate,
                CompetitionSports = c.PresentedSports.Select(cs => new PresentedSportModel
                {
                    Id = cs.Id,
                    Gender = cs.Gender,
                    Name = cs.Sport.Name + " " + cs.SportCategory.Name + " " + cs.SportDetail.Name,
                    MaxCompetitors = cs.MaxCompetitors,
                    MaxTechnicalStaff = cs.MaxTechnicalStaff,
                    IsParticipated = cs.Participates
                        .Any(p => p.RepresentativeUserId == representativeUserId &&
                                  p.PresentedSport.CompetitionId == competitionId),
                    HasRule = cs.HasRule,
                    Rule = cs.Rule

                }).OrderBy(cs => cs.Name).ToList()
            }).FirstOrDefaultAsync();
        }

        public async Task<CompetitionSportsListModel> GetConfirmCompetitionSportsList(int competitionId, int representativeUserId)
        {
            return await _competitions.Where(c => c.Id == competitionId).Select(c => new CompetitionSportsListModel
            {
                Id = c.Id,
                Name = c.Name,
                IsReadyActive = c.IsReadyActive,
                IsRegisterActive = c.IsRegisterActive,
                ReadyEndDate = c.ReadyEndDate,
                RegisterStartDate = c.RegisterStartDate,
                ReadyStartDate = c.ReadyStartDate,
                RegisterEndDate = c.ReadyEndDate,
                IsPrintCardActive = c.IsPrintCardActive,
                PrintCardStartDate = c.PrintCardStartDate,
                PrintCardEndDate = c.PrintCardEndDate,
                CompetitionSports = c.PresentedSports.Where(ps => ps.Participates.Any(p => p.RepresentativeUserId == representativeUserId && p.PresentedSport.CompetitionId == competitionId)).Select(cs => new PresentedSportModel
                {
                    Id = cs.Id,
                    Gender = cs.Gender,
                    Name = cs.Sport.Name + " " + cs.SportCategory.Name + " " + cs.SportDetail.Name,
                    MaxCompetitors = cs.MaxCompetitors,
                    MaxTechnicalStaff = cs.MaxTechnicalStaff,
                    HasRule = cs.HasRule,
                    Rule = cs.Rule
                }).OrderBy(cs => cs.Name).ToList()
            }).FirstOrDefaultAsync();
        }


        public async Task<IList<CompetitionModel>> GetUserRgisterCompetitionsList(int representativeUserId)
        {
            var currentDate = DateTime.Now;
            return
                await
                    _competitions.Where(
                        c => (c.IsRegisterActive &&
                         c.RegisterStartDate.Value <= currentDate && currentDate <= c.RegisterEndDate.Value) &&
                            c.PresentedSports.Any(
                                ps => ps.Participates.Any(p => p.RepresentativeUserId == representativeUserId && p.IsApproved == true)))
                        .Select(c => new CompetitionModel()
                        {
                            Id = c.Id,
                            Name = c.Name
                        }).ToListAsync();
        }


        public Task<string> GetName(int competitionId)
        {

            return _competitions.Where(c => c.Id == competitionId).Select(c => c.Name).FirstOrDefaultAsync();
        }

        public async Task<CommonTechnicalStaffListModel> GetTechnicalStaffsList(int competitionId, int representativeUserId)
        {
            return await _competitions.Where(c => c.Id == competitionId)
                  .Select(c => new CommonTechnicalStaffListModel
                  {
                      CompetitionName = c.Name,
                      MaxTechnicalStaffs = c.MaxCommonTechnicalStaffs,
                      TechnicalStaffs = c.CompetitionCommonTechnicalStaffs.Where(t => t.RepresentativeUserId == representativeUserId).Select(t => new SingleTechnicalStaffModel
                      {
                          NationalCode = t.TechnicalStaff.NationalCode,
                          Id = t.TechnicalStaff.Id,
                          BirthDate = t.TechnicalStaff.BirthDate,
                          FirstName = t.TechnicalStaff.FirstName,
                          LastName = t.TechnicalStaff.LastName,
                          FatherName = t.TechnicalStaff.FatherName,
                          IsApproved = t.TechnicalStaff.IsApproved,
                          Image = t.TechnicalStaff.Image,
                          RoleName = t.TechnicalStaffRole.Name
                      }).ToList()
                  }).FirstOrDefaultAsync();
        }




        public async Task<CompetitionSportsList> GetCompetitionSportsList(int competitionId)
        {
            return
                await _competitions.Where(c => c.Id == competitionId)
                    .Select(c => new CompetitionSportsList
                    {
                        CompetitionId = c.Id,
                        CompetitionName = c.Name,
                        CompetitionSports = c.PresentedSports.Select(cs => new SignleCompetitionSportModel()
                        {
                            Id = cs.Id,
                            SportName = cs.Sport.Name + " " + cs.SportCategory.Name + " " + cs.SportDetail.Name,
                            Sport = cs.Sport,
                            SportCategory = cs.SportCategory,
                            SportDetail = cs.SportDetail,
                            Gender = cs.Gender.ToString(),
                            MaxCompetitors = cs.MaxCompetitors,
                            MaxTechnicalStaffs = cs.MaxTechnicalStaff
                        }).ToList()

                    }).FirstOrDefaultAsync();
        }


        public async Task<SingleCompetitionModel> Get(int competitionId)
        {
            return await _competitions.Where(c => c.Id == competitionId).Select(c => new SingleCompetitionModel
            {
                Id = c.Id,
                Name = c.Name,
                IsReadyActive = c.IsReadyActive,
                IsRegisterActive = c.IsRegisterActive,
                ReadyStartDate = c.ReadyStartDate,
                ReadyEndDate = c.ReadyEndDate,
                RegisterStartDate = c.RegisterStartDate,
                RegisterEndDate = c.RegisterEndDate,
                IsPrintCardActive = c.IsPrintCardActive,
                PrintCardStartDate = c.PrintCardStartDate,
                PrintCardEndDate = c.PrintCardEndDate,
                Rule = c.Rule,
                LogoImage = c.LogoImage,
                MaxCommonTechnicalStaffs = c.MaxCommonTechnicalStaffs
            }).FirstOrDefaultAsync();
        }



        public async Task<Competition> Find(int competitionId)
        {
            return await _competitions.Where(c => c.Id == competitionId).FirstOrDefaultAsync();
        }






        public async Task<CommonTechnicalStaffCompetition> GetCompetition(int competitionId, int representativeUserId)
        {
            return
                await
                    _competitions.Where(
                        c => c.Id == competitionId)
                        .Select(c => new CommonTechnicalStaffCompetition
                        {
                            CompetitionName = c.Name,
                            MaxTechnicalStaffs = c.MaxCommonTechnicalStaffs,
                            TechnicalStaffsCount = c.CompetitionCommonTechnicalStaffs.Count(cts => cts.RepresentativeUserId == representativeUserId)
                        }).FirstOrDefaultAsync();
        }


        public async Task<bool> CanReportCards(int competitionId, int representativeUserId)
        {
            var currentDate = DateTime.Now;

            return await _competitions.Where(c => c.Id == competitionId).AnyAsync(c =>
                (c.IsPrintCardActive &&
                 c.PrintCardStartDate.Value <= currentDate && currentDate <= c.PrintCardEndDate.Value) &&
                c.CompetitionRepresentativeUsers.Any(
                    cru => cru.CompetitionId == competitionId && cru.RepresentativeUserId == representativeUserId));
        }


        public async Task<CompetitionLogoModel> GetLogoImage(int competitionId)
        {
            return await _competitions.Where(c => c.Id == competitionId).Select(c => new CompetitionLogoModel()
            {
                CompetitionName = c.Name,
                Logo = c.LogoImage
            }).FirstOrDefaultAsync();
        }




        public IList<ReadyCompetitionEmailNotificationModel> GetReadyCompetitionsNotification()
        {
            var currentDate = DateTime.Now;

            return _competitions.Where(c => c.IsReadyActive &&
                                                  DbFunctions.DiffDays(c.ReadyStartDate, currentDate) <= 2)
                .Select(c => new ReadyCompetitionEmailNotificationModel
                {
                    CompetitionName = c.Name,
                    EndDate = c.ReadyEndDate,
                    StartDate = c.ReadyStartDate,
                    RepresentativeUsers =
                        c.CompetitionRepresentativeUsers.Select(cru => new RepresentativeUserNotificationModel
                        {
                            Email = cru.RepresentativeUser.Email,
                            FirstName = cru.RepresentativeUser.FirstName,
                            LastName = cru.RepresentativeUser.LastName,
                            MobileNumber = cru.RepresentativeUser.MobileNumber,
                            University = cru.RepresentativeUser.University.Name,
                            Password = cru.RepresentativeUser.Password
                        }).ToList()
                }).ToList();
        }


        public async Task<IList<ReadyCompetitionEmailNotificationModel>> GetReadyCompetitionsNotificationAsync()
        {
            var currentDate = DateTime.Now;

            return await _competitions.AsNoTracking().Where(c => c.IsReadyActive &&
                                                  DbFunctions.DiffDays(c.ReadyStartDate, currentDate) <= 2 && DbFunctions.DiffDays(c.ReadyStartDate, currentDate) >= 0)
                .Select(c => new ReadyCompetitionEmailNotificationModel
                {
                    CompetitionName = c.Name,
                    EndDate = c.ReadyEndDate,
                    StartDate = c.ReadyStartDate,
                    RepresentativeUsers =
                        c.CompetitionRepresentativeUsers.Select(cru => new RepresentativeUserNotificationModel
                        {
                            Email = cru.RepresentativeUser.Email,
                            FirstName = cru.RepresentativeUser.FirstName,
                            LastName = cru.RepresentativeUser.LastName,
                            MobileNumber = cru.RepresentativeUser.MobileNumber,
                            University = cru.RepresentativeUser.University.Name,
                            Password = cru.RepresentativeUser.Password
                        }).ToList()
                }).ToListAsync();
        }

        public async Task<IList<ReadyCompetitionEmailNotificationModel>> GetRegisterCompetitionsNotification()
        {
            var currentDate = DateTime.Now;

            return await _competitions.AsNoTracking().Where(c => c.IsRegisterActive &&
                                                  DbFunctions.DiffDays(c.RegisterStartDate, currentDate) <= 2 && DbFunctions.DiffDays(c.RegisterStartDate, currentDate) >= 0)
                .Select(c => new ReadyCompetitionEmailNotificationModel
                {
                    CompetitionName = c.Name,
                    EndDate = c.RegisterEndDate,
                    StartDate = c.RegisterStartDate,
                    RepresentativeUsers =
                        c.CompetitionRepresentativeUsers.Where(cru => cru.RepresentativeUser.Participates.Any(p => p.PresentedSport.CompetitionId == c.Id && p.IsApproved == true)).Select(cru => new RepresentativeUserNotificationModel
                        {
                            Email = cru.RepresentativeUser.Email,
                            FirstName = cru.RepresentativeUser.FirstName,
                            LastName = cru.RepresentativeUser.LastName,
                            MobileNumber = cru.RepresentativeUser.MobileNumber,
                            University = cru.RepresentativeUser.University.Name,
                            Password = cru.RepresentativeUser.Password
                        }).ToList()
                }).ToListAsync();
        }


        public async Task<IList<ReadyCompetitionEmailNotificationModel>> GetPrintCardCompetitionsNotification()
        {
            var currentDate = DateTime.Now;

            return await _competitions.AsNoTracking().Where(c => c.IsPrintCardActive &&
                                                  DbFunctions.DiffDays(c.PrintCardStartDate, currentDate) <= 2 && DbFunctions.DiffDays(c.PrintCardStartDate, currentDate) >= 0)
                .Select(c => new ReadyCompetitionEmailNotificationModel
                {
                    CompetitionName = c.Name,
                    EndDate = c.PrintCardEndDate,
                    StartDate = c.PrintCardStartDate,
                    RepresentativeUsers =
                        c.CompetitionRepresentativeUsers.Where(cru => cru.RepresentativeUser.Participates.Any(p => p.PresentedSport.CompetitionId == c.Id && p.IsApproved == true && p.Competitors.Any(competitor => competitor.IsApproved == true))).Select(cru => new RepresentativeUserNotificationModel
                        {
                            Email = cru.RepresentativeUser.Email,
                            FirstName = cru.RepresentativeUser.FirstName,
                            LastName = cru.RepresentativeUser.LastName,
                            MobileNumber = cru.RepresentativeUser.MobileNumber,
                            University = cru.RepresentativeUser.University.Name,
                            Password = cru.RepresentativeUser.Password
                        }).ToList()
                }).ToListAsync();
        }


        public async Task<IList<CompetitionModel>> GetCompetitionsForCardPrint(int representativeUserId)
        {
            var currentDate = DateTime.Now;

            return await _competitions.Where(c =>
                (c.IsPrintCardActive &&
                 c.PrintCardStartDate.Value <= currentDate && currentDate <= c.PrintCardEndDate.Value) &&
                c.CompetitionRepresentativeUsers.Any(
                    cru => cru.RepresentativeUser.Id == representativeUserId && cru.RepresentativeUser.Participates.Any(p => p.PresentedSport.CompetitionId == c.Id && p.IsApproved == true && p.Competitors.Any(competitor => competitor.IsApproved == true)))).Select(c => new CompetitionModel
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).OrderBy(c => c.Name).ToListAsync();
        }


        public async Task<CompetitionAnnouncementsListModel> GetCompetitionAnnouncementsList(int competitionId)
        {
            return await _competitions.Where(c => c.Id == competitionId)
                .Select(c => new CompetitionAnnouncementsListModel
                {
                    CompetitionName = c.Name,
                    Announcements = c.Announcements.Select(a => new AnnouncementModel()
                    {
                        Id = a.Id,
                        CreatedDate = a.CreatedDate,
                        EmailText = a.EmailText,
                        SmsText = a.SmsText,
                        Title = a.Title,
                        WebsiteText = a.WebsiteText,
                    }).ToList()
                }).FirstOrDefaultAsync();
        }


        public void Delete(int competitionId)
        {
            var competition = new Competition() { Id = competitionId };

            _competitions.Attach(competition);

            _competitions.Remove(competition);
        }

        public async Task<CompetitionRuleAnnouncementsModel> GetCompetitionRuleAnnouncements(int competitionId)
        {
            return await _competitions.Where(c => c.Id == competitionId)
                  .Select(c => new CompetitionRuleAnnouncementsModel
                  {
                      CompetitionName = c.Name,
                      Rule = c.Rule,
                      Announcements = c.Announcements.Select(a => new AnnouncementModel()
                      {
                          Id = a.Id,
                          CreatedDate = a.CreatedDate,
                          //EmailText = a.EmailText,
                          //SmsText = a.SmsText,
                          Title = a.Title,
                          WebsiteText = a.WebsiteText,
                      }).OrderByDescending(a => a.CreatedDate).ToList()
                  }).FirstOrDefaultAsync();
        }

        public async Task<bool> CanAddCommonTechnicalStaff(int competitionId, int representativeUserId)
        {
            var currentDate = DateTime.Now;

            return
                await
                    _competitions.Where(c => c.Id == competitionId)
                    .AnyAsync(c =>
                             c.CompetitionCommonTechnicalStaffs.Count(cts => cts.RepresentativeUserId == representativeUserId) < c.MaxCommonTechnicalStaffs &&
                                    (c.IsRegisterActive && c.RegisterStartDate.Value <=
                                                                                          currentDate &&
                                                                                          currentDate <=
                                                                                         c.RegisterEndDate.Value));
        }

        public async Task<bool> CanEditCommonTechnicalStaff(int competitionId, int representativeUserId)
        {

            var currentDate = DateTime.Now;

            return
                await
                    _competitions.Where(c => c.CompetitionCommonTechnicalStaffs.Any(cts => cts.CompetitonId == competitionId && cts.RepresentativeUserId == representativeUserId)).AnyAsync(c =>
                                    (c.IsRegisterActive && c.RegisterStartDate.Value <=
                                                                                          currentDate &&
                                                                                          currentDate <=
                                                                                         c.RegisterEndDate.Value));
        }

        public async Task<bool> CanEditRejectedCommonTechnicalStaff(int competitionId, int representativeUserId)
        {

            var currentDate = DateTime.Now;

            return
                await
                    _competitions.Where(c => c.CompetitionCommonTechnicalStaffs.Any(cts => cts.CompetitonId == competitionId && cts.RepresentativeUserId == representativeUserId)).AnyAsync(c =>
                                    (c.RegisterStartDate.Value <= currentDate && currentDate <= c.PrintCardStartDate.Value));
        }


        public async Task<bool> CanDeleteCommonTechncialStaff(int competitionId, int representativeUserId)
        {
            var currentDate = DateTime.Now;

            return
                await
                    _competitions.Where(c => c.CompetitionCommonTechnicalStaffs.Any(cts => cts.CompetitonId == competitionId && cts.RepresentativeUserId == representativeUserId)).AnyAsync(c =>

                                    (c.IsRegisterActive && c.RegisterStartDate.Value <=
                                                                                          currentDate &&
                                                                                          currentDate <=
                                                                                         c.RegisterEndDate.Value));
        }

    }
}
