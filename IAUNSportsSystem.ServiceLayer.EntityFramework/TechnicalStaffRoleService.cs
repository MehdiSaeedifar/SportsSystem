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
    public class TechnicalStaffRoleService : ITechnicalStaffRoleService
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<TechnicalStaffRole> _technicalStaffRoles;

        public TechnicalStaffRoleService(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _technicalStaffRoles = dbContext.Set<TechnicalStaffRole>();
        }

        public void Add(DomainClasses.TechnicalStaffRole technicalStaffRole)
        {
            _technicalStaffRoles.Add(technicalStaffRole);
        }

        public void Edit(DomainClasses.TechnicalStaffRole technicalStaffRole)
        {
            _technicalStaffRoles.Attach(technicalStaffRole);

            _dbContext.Entry(technicalStaffRole).State = EntityState.Modified;

        }

        public void Delete(int technicalStaffRoleId)
        {
            var technicalStaffRole = new TechnicalStaffRole()
            {
                Id = technicalStaffRoleId,
            };

            _technicalStaffRoles.Attach(technicalStaffRole);
            _technicalStaffRoles.Remove(technicalStaffRole);

        }

        public async Task<IList<TechnicalStaffModel>> GetAllParticipationRoles()
        {
            return await _technicalStaffRoles.AsNoTracking().Where(t => t.IsCommon == false)
                .Select(t => new TechnicalStaffModel()
                {
                    Id = t.Id,
                    Name = t.Name
                }).Cacheable().ToListAsync();
        }


        public async Task<IList<TechnicalStaffModel>> GetAllCommonRoles()
        {
            return await _technicalStaffRoles.AsNoTracking().Where(t => t.IsCommon == true)
                .Select(t => new TechnicalStaffModel()
                {
                    Id = t.Id,
                    Name = t.Name
                }).Cacheable().ToListAsync();
        }


        public async Task<IList<TechnicalStaffModel>> GetAllRoles()
        {
            return await _technicalStaffRoles.AsNoTracking()
                .Select(t => new TechnicalStaffModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    IsCommon = t.IsCommon
                }).Cacheable().ToListAsync();
        }
    }
}
