using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFSecondLevelCache;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer.EntityFramework
{
    public class StudyFieldService : IStudyFieldService
    {
        private readonly IDbContext _dbcontext;
        private readonly IDbSet<StudyField> _studyFields;

        public StudyFieldService(IDbContext dbContext)
        {
            _dbcontext = dbContext;
            _studyFields = dbContext.Set<StudyField>();
        }

        public async Task<IList<StudyFieldModel>> GetAllAsync()
        {
            return await _studyFields.AsNoTracking().Select(sf => new StudyFieldModel
            {
                Id = sf.Id,
                Name = sf.Name
            }).OrderBy(x => x.Name).Cacheable().ToListAsync();
        }


        public void Add(StudyField studyField)
        {
            _studyFields.Add(studyField);
        }

        public void Delete(int studyFiledId)
        {
            var studyField = new StudyField { Id = studyFiledId };

            _studyFields.Attach(studyField);

            _studyFields.Remove(studyField);
        }

        public void Edit(StudyField studyField)
        {
            _studyFields.Attach(studyField);

            _dbcontext.Entry(studyField).Property(s => s.Name).IsModified = true;
        }
    }
}
