using IAUNSportsSystem.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface ISportService
    {
        Task<IList<SportModel>> GetSportsListForAddCompititionAsync();
        Task<IList<SportModel>> GetAllSportsAsync();
        IList<SportCategoryModel> GetSportsCategoryList(int sportId);
        IList<SportDetailModel> GetSportsDetailsList(int sportId);
        SportDetail FindSportDetail(int sportDetailId);
        void Add(Sport sport);
        void Delete(int sportId);
        Task<SportModel> GetSportWithCategoriesAndDetails(int sportId);
        void EditSport(Sport sport);
        void AddSportCategory(SportCategory sportCategory);
        void AddSportDetail(SportDetail sportDetail);
        void DeleteSportCategory(int sportCategoryId);
        void DeleteSportDetail(int sportDetailId);

    }

    public class SportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<SportCategoryModel> SportCategories { get; set; }
        public IList<SportDetailModel> SportDetails { get; set; }
    }
    public class SportCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SportDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
