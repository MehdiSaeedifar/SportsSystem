using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DataLayer;
using System.Data.Entity;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer.EntityFramework
{
    public class SportService : ISportService
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<Sport> _sports;
        private readonly IDbSet<SportCategory> _sportCategories;
        private readonly IDbSet<SportDetail> _sportDetails;
        public SportService(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _sports = dbContext.Set<Sport>();
            _sportCategories = dbContext.Set<SportCategory>();
            _sportDetails = dbContext.Set<SportDetail>();
        }
        public async Task<IList<SportModel>> GetSportsListForAddCompititionAsync()
        {
            return await _sports.Select(s => new SportModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToListAsync();
        }


        public async Task<IList<SportModel>> GetAllSportsAsync()
        {
            return await _sports.Select(s => new SportModel
            {
                Id = s.Id,
                Name = s.Name,
                SportCategories = s.SportCategories.Select(sc => new SportCategoryModel
                {
                    Id = sc.Id,
                    Name = sc.Name
                }).ToList(),
                SportDetails = s.SportDetails.Select(sd => new SportDetailModel
                {
                    Id = sd.Id,
                    Name = sd.Name
                }).ToList()
            }).ToListAsync();
        }


        public IList<SportCategoryModel> GetSportsCategoryList(int sportId)
        {
            return _sports.SingleOrDefault(s => s.Id == sportId).SportCategories
                .Select(s => new SportCategoryModel()
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

        }


        public IList<SportDetailModel> GetSportsDetailsList(int sportId)
        {
            return _sports.SingleOrDefault(s => s.Id == sportId).SportDetails
                .Select(s => new SportDetailModel()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
        }


        public SportDetail FindSportDetail(int sportDetailId)
        {
            return _sportDetails.Find(sportDetailId);
        }


        public void Add(Sport sport)
        {
            _sports.Add(sport);
        }


        public void Delete(int sportId)
        {
            var sport = new Sport() { Id = sportId };

            _sports.Attach(sport);

            _sports.Remove(sport);
        }


        public async Task<SportModel> GetSportWithCategoriesAndDetails(int sportId)
        {
            return await _sports.Where(s => s.Id == sportId)
                .Select(s => new SportModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    SportCategories = s.SportCategories.Select(sc => new SportCategoryModel
                    {
                        Id = sc.Id,
                        Name = sc.Name
                    }).ToList(),
                    SportDetails = s.SportDetails.Select(sd => new SportDetailModel
                    {
                        Id = sd.Id,
                        Name = sd.Name
                    }).ToList()
                }).FirstOrDefaultAsync();
        }


        public void EditSport(Sport sport)
        {
            _sports.Attach(sport);

            _dbContext.Entry(sport).Property(s => s.Name).IsModified = true;

        }


        public void AddSportCategory(SportCategory sportCategory)
        {
            _sportCategories.Add(sportCategory);
        }


        public void AddSportDetail(SportDetail sportDetail)
        {
            _sportDetails.Add(sportDetail);
        }


        public void DeleteSportCategory(int sportCategoryId)
        {
            var sportCategory = new SportCategory {Id = sportCategoryId};

            _sportCategories.Attach(sportCategory);

            _sportCategories.Remove(sportCategory);

        }

        public void DeleteSportDetail(int sportDetailId)
        {
            var sportDetail = new SportDetail { Id = sportDetailId };

            _sportDetails.Attach(sportDetail);

            _sportDetails.Remove(sportDetail);
        }
    }
}
