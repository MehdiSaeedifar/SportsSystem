using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer.EntityFramework
{
    public class TechnicalStaffService : ITechnicalStaffService
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<TechnicalStaff> _technicalStaves;
        private readonly IDbSet<ParticipationTechnicalStaff> _participationTechnicalStaves;

        public TechnicalStaffService(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _technicalStaves = dbContext.Set<TechnicalStaff>();
            _participationTechnicalStaves = dbContext.Set<ParticipationTechnicalStaff>();
        }

        public void Add(TechnicalStaff technicalStaff, int participationId, int technicalStaffRoleId)
        {
            if (technicalStaff.Id == 0)
            {

                _technicalStaves.Add(technicalStaff);
                _participationTechnicalStaves.Add(new ParticipationTechnicalStaff()
                {
                    ParticipationId = participationId,
                    TechnicalStaffId = technicalStaff.Id,
                    TechnicalStaffRoleId = technicalStaffRoleId,
                });
            }
            else
            {
                _participationTechnicalStaves.Add(new ParticipationTechnicalStaff()
                {
                    ParticipationId = participationId,
                    TechnicalStaffId = technicalStaff.Id,
                    TechnicalStaffRoleId = technicalStaffRoleId,
                });
            }
        }

        public void Edit(TechnicalStaff technicalStaff)
        {
            _technicalStaves.Attach(technicalStaff);

            _dbContext.Entry(technicalStaff).State = EntityState.Modified;

        }

        public async Task Delete(int technicalStaffId, int participationId)
        {

            var participationCount =
                await
                    _participationTechnicalStaves.CountAsync(
                        pt => pt.TechnicalStaffId == technicalStaffId);
            if (participationCount <= 1)
            {

                _participationTechnicalStaves.Remove(
                    await
                        _participationTechnicalStaves.FirstOrDefaultAsync(
                            pt => pt.TechnicalStaffId == technicalStaffId && pt.ParticipationId == participationId));

                var technicalStaff = new TechnicalStaff()
                {
                    Id = technicalStaffId,
                };

                _technicalStaves.Attach(technicalStaff);
                _technicalStaves.Remove(technicalStaff);
            }
            else
            {
                _participationTechnicalStaves.Remove(
                    await
                        _participationTechnicalStaves.FirstOrDefaultAsync(
                            pt => pt.TechnicalStaffId == technicalStaffId && pt.ParticipationId == participationId));
            }

        }

        public async Task Delete(int technicalStaffId, int[] similarParticipationIds)
        {
            var paricipationsCount = await
                _participationTechnicalStaves.Where(pts => pts.TechnicalStaffId == technicalStaffId).CountAsync();

            foreach (var similarParticipationId in similarParticipationIds)
            {
                _participationTechnicalStaves.Remove(
                    _participationTechnicalStaves.FirstOrDefault(
                        pts => pts.ParticipationId == similarParticipationId && pts.TechnicalStaffId == technicalStaffId));
            }

            if (paricipationsCount == similarParticipationIds.Length)
            {
                var technicalStaff = new TechnicalStaff()
                {
                    Id = technicalStaffId,
                };

                _technicalStaves.Attach(technicalStaff);
                _technicalStaves.Remove(technicalStaff);
            }

        }


        public async Task<SingleTechnicalStaffModel> GetByNationalCode(string nationalCode, int representativeUserId, int competitionId)
        {
            return
                await
                    _technicalStaves.Where(
                        t =>
                            t.NationalCode == nationalCode &&
                            t.Participations.Any(p => p.Participation.RepresentativeUserId == representativeUserId && p.Participation.PresentedSport.CompetitionId == competitionId))
                        .Select(t => new SingleTechnicalStaffModel()
                        {
                            BirthDate = t.BirthDate,
                            FatherName = t.FatherName,
                            FirstName = t.FirstName,
                            Id = t.Id,
                            Image = t.Image,
                            LastName = t.LastName,
                            NationalCode = t.NationalCode,
                            RoleId = t.Participations.Select(p => p.TechnicalStaffRole.Id).FirstOrDefault()
                        }).FirstOrDefaultAsync();
        }


        public async Task<TechnicalStaff> Find(int technicalStaffId)
        {
            return await _technicalStaves.FirstOrDefaultAsync(t => t.Id == technicalStaffId);
        }


        public async Task<ParticipationTechnicalStaff> GetParticipationTechnicalStaff(int participationId, int technicalStaffId)
        {
            return
                await
                    _participationTechnicalStaves.Where(
                        pt => pt.ParticipationId == participationId && pt.TechnicalStaffId == technicalStaffId)
                        .FirstOrDefaultAsync();
        }


        public async Task<SingleTechnicalStaffModel> GetTechnicalStaff(int technicalStaffId, int participationId, int representativeUserId)
        {
            return await _technicalStaves.Where(t => t.Id == technicalStaffId && t.Participations.Any(p => p.Participation.Id == participationId && p.Participation.RepresentativeUserId == representativeUserId))
                .Select(t => new SingleTechnicalStaffModel()
                {
                    Id = t.Id,
                    NationalCode = t.NationalCode,
                    FatherName = t.FatherName,
                    Image = t.Image,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    BirthDate = t.BirthDate,
                    RoleId =
                        t.Participations.Where(p => p.ParticipationId == participationId)
                            .Select(p => p.TechnicalStaffRoleId)
                            .FirstOrDefault(),
                    IsApproved = t.IsApproved,
                    Error = t.Error
                }).FirstOrDefaultAsync();
        }


        public async Task<IList<TechnicalStaffCardModel>> GetTechnicalStaffsCards(int competitionId, int representativeUserId)
        {
            return await _technicalStaves.AsNoTracking().Where(t => t.IsApproved == true && t.Participations.Any(p => p.Participation.PresentedSport.CompetitionId == competitionId && p.Participation.RepresentativeUserId == representativeUserId))
                 .Select(t => new TechnicalStaffCardModel()
                 {
                     NationalCode = t.NationalCode,
                     Image = t.Image,
                     FirstName = t.FirstName,
                     LastName = t.LastName,
                     University = t.Participations.Select(p => p.Participation.RepresentativeUser.University.Name).FirstOrDefault(),
                     Roles =
                         t.Participations
                             .Select(p => new
                             {
                                 RoleName = p.TechnicalStaffRole.Name,
                                 SportName = p.Participation.PresentedSport.Sport.Name,
                                 SportCategoryName = p.Participation.PresentedSport.SportCategory.Name,
                                 SportDetailName = p.Participation.PresentedSport.SportDetail.Name,
                                 Gender = p.Participation.PresentedSport.Gender
                             }).Distinct().Select(x => x.RoleName + " " + x.SportName + " " + x.SportCategoryName)
                             .ToList(),
                     Dorm = t.Dorm.Name,
                     DormNumber = t.DormNumber
                 }).ToListAsync();
        }


        public async Task<IList<TechnicalStaffCardModel>> GetCommonTechnicalStaffsCards(int competitionId, int representativeUserId)
        {
            return await _technicalStaves.AsNoTracking().Where(t => t.IsApproved == true && t.CompetitionCommonTechnicalStaffs.Any(p => p.CompetitonId == competitionId && p.RepresentativeUserId == representativeUserId))
                 .Select(t => new TechnicalStaffCardModel()
                 {
                     NationalCode = t.NationalCode,
                     Image = t.Image,
                     FirstName = t.FirstName,
                     LastName = t.LastName,
                     University = t.CompetitionCommonTechnicalStaffs.Select(p => p.RepresentativeUser.University.Name).FirstOrDefault(),
                     Roles =
                         t.CompetitionCommonTechnicalStaffs.Select(ctf => ctf.TechnicalStaffRole.Name).ToList(),
                     Dorm = t.Dorm.Name,
                     DormNumber = t.DormNumber
                 }).ToListAsync();
        }


        public async Task<SingleTechnicalStaffModel> Get(int technicalStaffId, int participationId)
        {
            return await _technicalStaves.Where(t => t.Id == technicalStaffId)
                .Select(t => new SingleTechnicalStaffModel()
                {
                    Id = t.Id,
                    NationalCode = t.NationalCode,
                    FatherName = t.FatherName,
                    Image = t.Image,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    BirthDate = t.BirthDate,
                    Error = t.Error,
                    DormId = t.DormId,
                    DormNumber = t.DormNumber,
                    //RoleId = t.Participations.
                    RoleName = t.Participations.FirstOrDefault(p => p.ParticipationId == participationId).TechnicalStaffRole.Name,
                    IsApproved = t.IsApproved,
                }).FirstOrDefaultAsync();
        }


        public void EditApproval(TechnicalStaff technicalStaff)
        {
            _technicalStaves.Attach(technicalStaff);

            _dbContext.Entry(technicalStaff).Property(t => t.IsApproved).IsModified = true;
            _dbContext.Entry(technicalStaff).Property(t => t.Error).IsModified = true;
            _dbContext.Entry(technicalStaff).Property(t => t.DormId).IsModified = true;
            _dbContext.Entry(technicalStaff).Property(t => t.DormNumber).IsModified = true;
        }


        public async Task<SingleTechnicalStaffModel> GetCommonTechnicalStaff(int technicalStaffId)
        {
            return await _technicalStaves.Where(t => t.Id == technicalStaffId)
                 .Select(t => new SingleTechnicalStaffModel()
                 {
                     Id = t.Id,
                     NationalCode = t.NationalCode,
                     FatherName = t.FatherName,
                     Image = t.Image,
                     FirstName = t.FirstName,
                     LastName = t.LastName,
                     BirthDate = t.BirthDate,
                     Error = t.Error,
                     DormId = t.DormId,
                     DormNumber = t.DormNumber,
                     //RoleId = t.Participations.
                     RoleName = t.CompetitionCommonTechnicalStaffs.FirstOrDefault().TechnicalStaffRole.Name,
                     IsApproved = t.IsApproved,
                 }).FirstOrDefaultAsync();
        }


        public async Task<IList<RejectedTechnicalStaff>> GetRejectedTechnicalStaffsList(int representativeUserId)
        {
            var currentDate = DateTime.Now;

            return await _technicalStaves.Where(
                  c =>
                      c.Participations.Any(p => p.Participation.PresentedSport.Competition.RegisterStartDate <= currentDate &&
                      currentDate < p.Participation.PresentedSport.Competition.PrintCardStartDate && c.IsApproved == false && p.Participation.RepresentativeUserId == representativeUserId)).
                  Select(t => new RejectedTechnicalStaff()
                  {
                      Id = t.Id,
                      NationalCode = t.NationalCode,
                      LastName = t.LastName,
                      FirstName = t.FirstName,
                      CompetitionName = t.Participations.Select(p => p.Participation.PresentedSport.Competition.Name).FirstOrDefault(),
                      Image = t.Image,
                      ParticipationId = t.Participations.Select(p => p.ParticipationId).FirstOrDefault(),
                      Role = t.Participations.Select(p => p.TechnicalStaffRole.Name + " " + p.Participation.PresentedSport.Sport.Name).FirstOrDefault()
                  }).ToListAsync();
        }

    }
}
