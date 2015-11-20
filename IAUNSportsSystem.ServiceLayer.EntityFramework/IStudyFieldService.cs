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
    public class StudyFieldService : IStudyFieldService
    {
        private readonly IDbSet<StudyField> _studyFields;

        public StudyFieldService(IDbContext dbContext)
        {
            _studyFields = dbContext.Set<StudyField>();
        }

        public async Task<IList<StudyFieldModel>> GetAllAsync()
        {
            return await _studyFields.Select(sf => new StudyFieldModel
            {
                Id = sf.Id,
                Name = sf.Name
            }).ToListAsync();
        }
    }
}
