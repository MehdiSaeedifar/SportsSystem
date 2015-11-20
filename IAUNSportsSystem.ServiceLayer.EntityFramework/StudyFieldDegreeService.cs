using IAUNSportsSystem.DomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFSecondLevelCache;
using IAUNSportsSystem.DataLayer;

namespace IAUNSportsSystem.ServiceLayer.EntityFramework
{
    public class StudyFieldDegreeService : IStudyFieldDegreeService
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<StudyFieldDegree> _studyFieldDegrees;

        public StudyFieldDegreeService(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _studyFieldDegrees = dbContext.Set<StudyFieldDegree>();
        }

        public async Task<IList<StudyFieldDegreeModel>> GetAllAsync()
        {
            return await _studyFieldDegrees.AsNoTracking().Select(sfd => new StudyFieldDegreeModel
            {
                Id = sfd.Id,
                Name = sfd.Name
            }).OrderBy(x => x.Name).Cacheable().ToListAsync();
        }


        public void Add(StudyFieldDegree studyFieldDegree)
        {
            _studyFieldDegrees.Add(studyFieldDegree);
        }

        public void Delete(int studyFieldDegreeId)
        {
            var studyFieldDegree = new StudyFieldDegree
            {
                Id = studyFieldDegreeId
            };

            _studyFieldDegrees.Attach(studyFieldDegree);

            _studyFieldDegrees.Remove(studyFieldDegree);

        }

        public void Edit(StudyFieldDegree studyFieldDegree)
        {
            _studyFieldDegrees.Attach(studyFieldDegree);
            _dbContext.Entry(studyFieldDegree).Property(sfd => sfd.Name).IsModified = true;
        }
    }
}
