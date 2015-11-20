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
    public class DormService : IDormService
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<Dorm> _dorms;

        public DormService(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _dorms = dbContext.Set<Dorm>();
        }

        public async Task<IList<DormModel>> GetAll()
        {
            return await _dorms.Select(d => new DormModel()
            {
                Id = d.Id,
                Name = d.Name
            }).ToListAsync();
        }


        public void Add(Dorm dorm)
        {
            _dorms.Add(dorm);
        }

        public void Edit(Dorm dorm)
        {
            _dbContext.Entry(dorm).State = EntityState.Modified;
        }

        public void Delete(int dormId)
        {
            var dorm = new Dorm() {Id = dormId};

            _dorms.Attach(dorm);
            _dorms.Remove(dorm);
        }
    }
}
