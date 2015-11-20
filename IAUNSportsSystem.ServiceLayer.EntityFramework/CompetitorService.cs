using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer.EntityFramework
{
    public class CompetitorService : ICompetitorService
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<Competitor> _competitors;

        public CompetitorService(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _competitors = dbContext.Set<Competitor>();
        }

        public void Add(Competitor competitor)
        {
            _competitors.Add(competitor);
        }


        public async Task<IList<CompetitorModel>> GetAll(int participationId)
        {
            return await _competitors.AsNoTracking()
                .Select(c => new CompetitorModel
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    NationalCode = c.NationalCode,
                    StudentNumber = c.StudentNumber,
                    BirthDate = c.BirthDate,
                    StudyField = c.StudyField.Name,
                    StudyFieldDegree = c.StudyFieldDegree.Name,
                    UserImage = c.UserImage,
                    ParticipateId = c.ParticipateId
                }).Where(c => c.ParticipateId == participationId).ToListAsync();
        }
        public async Task<SingleCompetitorModel> GetCompetitor(int competitorId)
        {

            return await _competitors.Where(c => c.Id == competitorId)
                .Select(c => new SingleCompetitorModel()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    BirthDate = c.BirthDate,
                    NationalCode = c.NationalCode,
                    Email = c.Email,
                    FatherName = c.FatherName,
                    MobileNumber = c.MobileNumber,
                    StudyField = c.StudyField.Name,
                    StudyFieldDegree = c.StudyFieldDegree.Name,
                    UserImage = c.UserImage,
                    Height = c.Height,
                    IsApproved = c.IsApproved,
                    InsuranceEndDate = c.InsuranceEndDate,
                    InsuranceImage = c.InsuranceImage,
                    InsuranceNumber = c.InsuranceNumber,
                    PhoneNumber = c.PhoneNumber,
                    StudentCertificateImage = c.StudentCertificateImage,
                    StudentNumber = c.StudentNumber,
                    Weight = c.Weight,
                    Error = c.Error,
                    AzmoonConfirmationImage = c.AzmoonConfirmationImage,
                    ParticipateId = c.ParticipateId,
                    StudyFieldDegreeId = c.StudyFieldDegreeId,
                    StudyFieldId = c.StudyFieldId,
                    DormId = c.DormId,
                    DormNumber = c.DormNumber
                }).SingleAsync();
        }

        public async Task<SingleCompetitorModel> GetCompetitor(int competitorId, int representativeUserId)
        {

            return await _competitors.Where(c => c.Id == competitorId && c.Participate.RepresentativeUserId == representativeUserId)
                .Select(c => new SingleCompetitorModel()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    BirthDate = c.BirthDate,
                    NationalCode = c.NationalCode,
                    Email = c.Email,
                    FatherName = c.FatherName,
                    MobileNumber = c.MobileNumber,
                    StudyField = c.StudyField.Name,
                    StudyFieldDegree = c.StudyFieldDegree.Name,
                    UserImage = c.UserImage,
                    Height = c.Height,
                    IsApproved = c.IsApproved,
                    InsuranceEndDate = c.InsuranceEndDate,
                    InsuranceImage = c.InsuranceImage,
                    InsuranceNumber = c.InsuranceNumber,
                    PhoneNumber = c.PhoneNumber,
                    StudentCertificateImage = c.StudentCertificateImage,
                    StudentNumber = c.StudentNumber,
                    Weight = c.Weight,
                    Error = c.Error,
                    AzmoonConfirmationImage = c.AzmoonConfirmationImage,
                    ParticipateId = c.ParticipateId,
                    StudyFieldDegreeId = c.StudyFieldDegreeId,
                    StudyFieldId = c.StudyFieldId,
                    DormId = c.DormId,
                    DormNumber = c.DormNumber
                }).SingleAsync();
        }


        public void EditApproval(Competitor competitor)
        {

            _competitors.Attach(competitor);

            _dbContext.Entry(competitor).Property(x => x.IsApproved).IsModified = true;
            _dbContext.Entry(competitor).Property(x => x.Error).IsModified = true;
            _dbContext.Entry(competitor).Property(x => x.DormId).IsModified = true;
            _dbContext.Entry(competitor).Property(x => x.DormNumber).IsModified = true;

        }



        public void Delete(int competitiorId)
        {
            var competitior = new Competitor
            {
                Id = competitiorId
            };

            _competitors.Attach(competitior);

            _competitors.Remove(competitior);
        }


        public async Task<Competitor> Find(int competitorId)
        {
            return await _competitors.FirstOrDefaultAsync(c => c.Id == competitorId);
        }


        public async Task<IList<CompetitorCardModel>> GetCompetitorsCard(int competitionId, int representativeId)
        {
            return await _competitors.AsNoTracking().Where(c => c.Participate.RepresentativeUserId == representativeId && c.Participate.PresentedSport.CompetitionId == competitionId && c.IsApproved == true)
                .Select(c => new CompetitorCardModel
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    NationalCode = c.NationalCode,
                    StudentNumber = c.StudentNumber,
                    Image = c.UserImage,
                    Sport =
                        c.Participate.PresentedSport.Sport.Name + " " + c.Participate.PresentedSport.SportCategory.Name +
                        " " + c.Participate.PresentedSport.SportDetail.Name,
                    University = c.Participate.RepresentativeUser.University.Name,
                    Dorm = c.Dorm.Name,
                    DormNumber = c.DormNumber
                }).ToListAsync();
        }

        public async Task<IList<RejectedCompetitor>> GetRejectedCompetitorsList(int representativeUserId)
        {
            var currentDate = DateTime.Now;

            return await _competitors.Where(
                  c =>
                      c.Participate.PresentedSport.Competition.RegisterStartDate <= currentDate &&
                      currentDate < c.Participate.PresentedSport.Competition.PrintCardStartDate && c.IsApproved == false && c.Participate.RepresentativeUserId == representativeUserId).
                  Select(c => new RejectedCompetitor()
                  {
                      Id = c.Id,
                      NationalCode = c.NationalCode,
                      LastName = c.LastName,
                      FirstName = c.FirstName,
                      Sport =
                          c.Participate.PresentedSport.Sport.Name + " " + c.Participate.PresentedSport.SportCategory.Name +
                          " " + c.Participate.PresentedSport.SportDetail.Name,
                      CompetitionName = c.Participate.PresentedSport.Competition.Name,
                      UserImage = c.UserImage,
                      ParticipationId = c.ParticipateId
                  }).ToListAsync();
        }


    }
}
