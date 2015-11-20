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
    public class CommonTechnicalStaffService : ICommonTechnicalStaffService
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<CompetitionCommonTechnicalStaff> _competitionCommonTechnicalStaffs;
        private readonly IDbSet<TechnicalStaff> _technicalStaffs;

        public CommonTechnicalStaffService(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _competitionCommonTechnicalStaffs = dbContext.Set<CompetitionCommonTechnicalStaff>();
            _technicalStaffs = dbContext.Set<TechnicalStaff>();
        }

        public void Add(TechnicalStaff technicalStaff, int representativeUserId, int competitionId, int roleId)
        {
            _competitionCommonTechnicalStaffs.Add(new CompetitionCommonTechnicalStaff
            {
                CompetitonId = competitionId,
                RepresentativeUserId = representativeUserId,
                TechnicalStaffRoleId = roleId,
                TechnicalStaff = technicalStaff
            });
        }

        public void Edit(DomainClasses.TechnicalStaff technicalStaff)
        {
            _technicalStaffs.Attach(technicalStaff);
            _dbContext.Entry(technicalStaff).State = EntityState.Modified;
        }

        public void Delete(int technicalStaffId)
        {

            var technicalStaff = new TechnicalStaff()
            {
                Id = technicalStaffId,
            };

            _technicalStaffs.Attach(technicalStaff);
            _technicalStaffs.Remove(technicalStaff);
        }





        public async Task<TechnicalStaff> Get(int technicalStaffId)
        {
            return await _technicalStaffs.FirstOrDefaultAsync(t => t.Id == technicalStaffId);
        }




        public async Task<CompetitionCommonTechnicalStaff> GetCompetitionTechnicalStaff(int technicalStaffId, int representativeUserId)
        {
            return
                await _competitionCommonTechnicalStaffs.FirstOrDefaultAsync(
                    c => c.RepresentativeUserId == representativeUserId && c.TechnicalStaffId == technicalStaffId);
        }





        public async Task<SingleTechnicalStaffModel> GetTechnicalStaff(int technicalStaffId, int representativeUserId)
        {
            return await _technicalStaffs.Where(t => t.Id == technicalStaffId && t.CompetitionCommonTechnicalStaffs.Any(cts => cts.RepresentativeUserId == representativeUserId))
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
                        t.CompetitionCommonTechnicalStaffs
                            .Select(p => p.TechnicalStaffRoleId)
                            .FirstOrDefault(),
                    IsApproved = t.IsApproved,
                    Error = t.Error,
                    MobileNumber = t.MobileNumber
                }).FirstOrDefaultAsync();
        }


        public async Task<IList<SingleTechnicalStaffModel>> GetCommonTechnicalStaffsList(int competitionId, int representativeUserId)
        {
            return await _competitionCommonTechnicalStaffs.Where(cmt => cmt.CompetitonId == competitionId && cmt.RepresentativeUserId == representativeUserId)
                .Select(cmt => new SingleTechnicalStaffModel()
                {
                    Id = cmt.TechnicalStaff.Id,
                    NationalCode = cmt.TechnicalStaff.NationalCode,
                    FatherName = cmt.TechnicalStaff.FatherName,
                    Image = cmt.TechnicalStaff.Image,
                    FirstName = cmt.TechnicalStaff.FirstName,
                    LastName = cmt.TechnicalStaff.LastName,
                    BirthDate = cmt.TechnicalStaff.BirthDate,
                    RoleName = cmt.TechnicalStaffRole.Name,
                    IsApproved = cmt.TechnicalStaff.IsApproved,
                    DormName = cmt.TechnicalStaff.Dorm.Name,
                    DormNumber = cmt.TechnicalStaff.DormNumber,
                    Error = cmt.TechnicalStaff.Error
                }).ToListAsync();
        }


        public async Task<CommonTechnicalStaffModel> Get(int competitionId, int representativeUserId)
        {
            return await _competitionCommonTechnicalStaffs.Where(cts => cts.CompetitonId == competitionId && cts.RepresentativeUserId == representativeUserId).Select(cts => new CommonTechnicalStaffModel
            {
                CompetitionName = cts.Competition.Name,
                University = cts.RepresentativeUser.University.Name
            }).FirstOrDefaultAsync();
        }







        public async Task<int> GetUnverifiedCommonTechnicalStaffsCount(int competitionId, int representativeUserId)
        {
            return
                await
                    _competitionCommonTechnicalStaffs.Where(
                        cts => cts.CompetitonId == competitionId && cts.RepresentativeUserId == representativeUserId)
                        .CountAsync(cts => cts.TechnicalStaff.IsApproved == null);
        }

        public async Task<IList<CommonTechnicalStaffReportModel>> GetCommonTechnicalStaffsReportAsync(int competitionId)
        {
            return
                await
                    _competitionCommonTechnicalStaffs.Where(cts => cts.Competition.Id == competitionId).GroupBy(cts => new { cts.RepresentativeUser, cts.Competition })
                        .Select(c => new CommonTechnicalStaffReportModel()
                        {
                            CompetitionName = c.Key.Competition.Name,
                            UniversityName = c.Key.RepresentativeUser.University.Name,
                            TechnicalStaves = c.Key.RepresentativeUser.CompetitionCommonTechnicalStaffs.Where(pts => pts.TechnicalStaff.IsApproved == true && pts.CompetitonId == c.Key.Competition.Id).Select(pts => new TechnicalStaffReportModel
                            {
                                Image = pts.TechnicalStaff.Image,
                                Role = pts.TechnicalStaffRole.Name,
                                FatherName = pts.TechnicalStaff.FatherName,
                                FullName = pts.TechnicalStaff.FirstName + " " + pts.TechnicalStaff.LastName,
                                NationalCode = pts.TechnicalStaff.NationalCode
                            }).ToList()
                        }).ToListAsync();
        }

        public async Task<IList<RejectedTechnicalStaff>> GetRejectedTechnicalStaffsList(int representativeUserId)
        {
            var currentDate = DateTime.Now;

            return await _technicalStaffs.Where(
                  c =>
                      c.CompetitionCommonTechnicalStaffs.Any(p => p.Competition.RegisterStartDate <= currentDate &&
                      currentDate < p.Competition.PrintCardStartDate && c.IsApproved == false && p.RepresentativeUserId == representativeUserId)).
                  Select(t => new RejectedTechnicalStaff()
                  {
                      Id = t.Id,
                      NationalCode = t.NationalCode,
                      LastName = t.LastName,
                      FirstName = t.FirstName,
                      CompetitionName = t.CompetitionCommonTechnicalStaffs.Select(cts => cts.Competition.Name).FirstOrDefault(),
                      Image = t.Image,
                      Role = t.CompetitionCommonTechnicalStaffs.Select(p => p.TechnicalStaffRole.Name).FirstOrDefault(),
                      CompetitionId = t.CompetitionCommonTechnicalStaffs.Select(cts => cts.CompetitonId).FirstOrDefault()
                  }).ToListAsync();
        }

        public async Task<int> GetCompetitionId(int technicalStaffId)
        {
            return
                await
                    _technicalStaffs.Where(t => t.Id == technicalStaffId)
                        .Select(t => t.CompetitionCommonTechnicalStaffs.Select(cts => cts.CompetitonId).FirstOrDefault())
                        .FirstOrDefaultAsync();
        }


    }
}
