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
    public class UniversityService : IUniversityService
    {
        private readonly IDbContext _dbcontext;
        private readonly IDbSet<University> _universities;

        public UniversityService(IDbContext dbContext)
        {
            _dbcontext = dbContext;
            _universities = dbContext.Set<University>();
        }

        public async Task<IList<UniversityModel>> GetAll()
        {
            return await _universities.AsNoTracking().Select(u => new UniversityModel
            {
                Id = u.Id,
                Name = u.Name
            }).OrderBy(x => x.Name).Cacheable().ToListAsync();
        }


        public void Add(University university)
        {
            _universities.Add(university);
        }


        public void Delete(int universityId)
        {
            var university = new University { Id = universityId };
            _universities.Attach(university);

            _universities.Remove(university);
        }

        public void Edit(University university)
        {
            _universities.Attach(university);

            _dbcontext.Entry(university).Property(u => u.Name).IsModified = true;
        }
    }
}
