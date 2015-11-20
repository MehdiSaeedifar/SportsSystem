using IAUNSportsSystem.DomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.Models;
using Iris.Web.IrisMembership.System.Web.Helpers;

namespace IAUNSportsSystem.ServiceLayer.EntityFramework
{
    public class RepresentativeUserService : IRepresentativeUserService
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<RepresentativeUser> _representativeUsers;

        public RepresentativeUserService(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _representativeUsers = dbContext.Set<RepresentativeUser>();
        }

        public void Add(RepresentativeUser representativeUser)
        {
            _representativeUsers.Add(representativeUser);
        }

        public async Task<LoginResult> Login(string email, string password)
        {
            var result = new LoginResult
            {
                IsValid = false,
                UserId = 0
            };

            var user =
                await
                    _representativeUsers.Where(u => u.Email == email).Select(u => new { u.Id, u.Password })
                        .SingleOrDefaultAsync();

            if (user == null)
            {
                return result;
            }

            if (user.Password == password)
            {
                result.IsValid = true;
                result.UserId = user.Id;
            }
            else
            {
                return result;
            }

            return result;
        }



        public async Task<IList<RepresentativeUserModel>> GetCompetitionSportUsersList(int competitionSportId)
        {
            return
                await _representativeUsers.Where(u => u.Participates.Any(p => p.PresentedSportId == competitionSportId))
                    .Select(u => new RepresentativeUserModel
                    {
                        Id = u.Id,
                        Email = u.Email,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        FatherName = u.FatherName,
                        MobileNumber = u.MobileNumber,
                        NationalCode = u.NationalCode,
                        UniversityName = u.University.Name,
                        IsApproved = u.Participates.Where(p => p.PresentedSportId == competitionSportId && p.RepresentativeUserId == u.Id).Select(p => p.IsApproved).FirstOrDefault(),
                        ParticipationId = u.Participates.Where(p => p.PresentedSportId == competitionSportId && p.RepresentativeUserId == u.Id).Select(p => p.Id).FirstOrDefault()
                    }).ToListAsync();
        }


        public async Task<bool> IsExistByEmail(string email)
        {
            return await _representativeUsers.AnyAsync(u => u.Email == email);
        }



        public async Task<IList<RepresentativeUserModel>> GetAll()
        {
            return
                await _representativeUsers
                    .Select(u => new RepresentativeUserModel
                    {
                        Id = u.Id,
                        Email = u.Email,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        FatherName = u.FatherName,
                        MobileNumber = u.MobileNumber,
                        NationalCode = u.NationalCode,
                        UniversityName = u.University.Name,
                    }).ToListAsync();
        }


        public void Delete(int representativeUserId)
        {
            var representativeUser = new RepresentativeUser() { Id = representativeUserId };

            _representativeUsers.Attach(representativeUser);
            _representativeUsers.Remove(representativeUser);
        }


        public void Edit(RepresentativeUser representativeUser)
        {
            _dbContext.Entry(representativeUser).State = EntityState.Modified;

        }


        public async Task<IList<CompetitionRepresentativeUserModel>> GetCompetitionRepresentativeUsers(int competitionId)
        {
            return await _representativeUsers
                .Select(r => new CompetitionRepresentativeUserModel
                {
                    Id = r.Id,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    Email = r.Email,
                    MobileNumber = r.MobileNumber,
                    UniversityName = r.University.Name,
                    IsSelected = r.CompetitionRepresentativeUsers.Any(cru => cru.CompetitionId == competitionId)
                }).ToListAsync();
        }

        public async Task<IList<RegisterListDataGridModel>> GetRegisterRepresentativeUsersDataGrid(int competitionId)
        {
            return await _representativeUsers.Where(
                 r => r.CompetitionRepresentativeUsers.Any(cru => cru.CompetitionId == competitionId))
                 .Select(r => new RegisterListDataGridModel
                 {
                     RepresentativeUserId = r.Id,
                     FirstName = r.FirstName,
                     LastName = r.LastName,
                     University = r.University.Name,
                     UnverifiedCompetitorsCount = r.Participates.Where(p => p.PresentedSport.CompetitionId == competitionId).Sum(p => (Int32?)p.Competitors.Count(c => c.IsApproved == null)) ?? 0,
                     UnverifiedTechnicalStaffsCount =
                         (r.Participates.Where(p => p.PresentedSport.CompetitionId == competitionId).Sum(
                             p => (Int32?)p.ParticipationTechnicalStaves.Count(pts => pts.TechnicalStaff.IsApproved == null)) ?? 0)
                             + (r.CompetitionCommonTechnicalStaffs.Count(cts => cts.CompetitonId == competitionId && cts.TechnicalStaff.IsApproved == null))
                 }).ToListAsync();
        }




        public async Task<string> GetFullName(int representativeUserId)
        {
            return await _representativeUsers.Where(r => r.Id == representativeUserId)
                .Select(r => r.FirstName + " " + r.LastName).FirstOrDefaultAsync();
        }


        public async Task<RepresentativeUserModel> Get(int representativeUserId)
        {
            return await _representativeUsers.Where(r => r.Id == representativeUserId)
                .Select(u => new RepresentativeUserModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    FatherName = u.FatherName,
                    MobileNumber = u.MobileNumber,
                    NationalCode = u.NationalCode,
                    UniversityName = u.University.Name,
                }).FirstOrDefaultAsync();
        }


        public Task<RepresentativeUser> FindById(int representativeUserId)
        {
            return _representativeUsers.Where(r => r.Id == representativeUserId).FirstOrDefaultAsync();
        }


        public async Task<bool> CanEditEmail(int representaiveUserId, string email)
        {
            return !await _representativeUsers.AnyAsync(r => r.Id != representaiveUserId && r.Email == email);
        }


        public async Task<string> GetPassword(int representativeUserId)
        {
            return
                await
                    _representativeUsers.Where(r => r.Id == representativeUserId)
                        .Select(r => r.Password)
                        .FirstOrDefaultAsync();
        }

        public void ChangePassword(int representativeUserId, string password)
        {
            //var representativeUser = new RepresentativeUser {Id = representativeUserId, Password = password};

            var representativeUser = _representativeUsers.Find(representativeUserId);

            representativeUser.Password = password;
            //_representativeUsers.Attach(representativeUser);

            //_dbContext.Entry(representativeUser).Property(r => r.Password).IsModified = true;
            //_dbContext.Entry(representativeUser).Property(r => r.Email).IsModified = false;

            //var x= _dbContext.Entry(representativeUser).GetValidationResult();
        }



        public async Task<string> GetUniversityName(int representativeUserId)
        {
            return
                await
                    _representativeUsers.Where(r => r.Id == representativeUserId)
                        .Select(r => r.University.Name)
                        .FirstOrDefaultAsync();
        }
    }
}
