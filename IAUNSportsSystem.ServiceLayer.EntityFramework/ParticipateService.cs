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
    public class ParticipateService : IParticipateService
    {
        private readonly IDbSet<Participate> _participates;

        public ParticipateService(IDbContext dbContext)
        {
            _participates = dbContext.Set<Participate>();
        }

        public void Add(Participate participate)
        {
            _participates.Add(participate);
        }


        public async Task<CompetitonReportModel> GetReportAsync(int participationId)
        {
            return await _participates.Select(pa => new CompetitonReportModel()
              {
                  ParticipationId = pa.Id,
                  CompetitionName = pa.PresentedSport.Competition.Name,
                  Gender = pa.PresentedSport.Gender.ToString(),
                  SportDetail = pa.PresentedSport.SportDetail.Name,
                  SportName = pa.PresentedSport.Sport.Name,
                  SportCategory = pa.PresentedSport.SportCategory.Name,
                  UniversityName = pa.RepresentativeUser.University.Name,

                  Competitors = pa.Competitors.Select(c => new CompetitorsReportModel
                  {

                      FullName = c.FirstName +" "+ c.LastName,
                      FatherName = c.FatherName,
                      //BirthDate = c.BirthDate.ToString(),
                      StudentNumber = c.StudentNumber,
                      StudyField = c.StudyField.Name,
                      IdentityNumber = c.IdentityNumber,
                      InsuranceNumber = c.InsuranceNumber,
                      Image = c.UserImage
                  }).ToList()
              }).SingleAsync(pa => pa.ParticipationId == participationId);
        }

    }
}
