using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.DomainClasses;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Areas.Sport.ViewModels;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.Sport.Controllers
{
    public partial class HomeController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly ISportService _sportService;
        public HomeController(IDbContext dbContext, ISportService sportService)
        {
            _dbContext = dbContext;
            _sportService = sportService;
        }
        public virtual ActionResult Index()
        {
            return View();
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Add(AddSportViewModel sportModel)
        {

            var sport = new DomainClasses.Sport
            {
                Name = sportModel.SportName
            };

            if (sportModel.SportCategories != null && sportModel.SportCategories.Any())
            {
                foreach (var sportCategory in sportModel.SportCategories)
                {
                    sport.SportCategories.Add(new SportCategory() { Name = sportCategory.Name });
                }
            }

            if (sportModel.SportDetails != null && sportModel.SportDetails.Any())
            {
                foreach (var sportDetail in sportModel.SportDetails)
                {
                    sport.SportDetails.Add(new SportDetail() { Name = sportDetail.Name });
                }
            }

            _sportService.Add(sport);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int sportId)
        {
            _sportService.Delete(sportId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SiteAuthorize(Roles = "admin")]
        public virtual async Task<ActionResult> GetSportsList()
        {
            return Json(await _sportService.GetSportsListForAddCompititionAsync(), JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "admin")]
        public virtual ActionResult GetSportsCategoryList(int sportId)
        {
            var model = _sportService.GetSportsCategoryList(sportId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "admin")]
        public virtual ActionResult GetSportsDetailsList(int sportId)
        {
            var model = _sportService.GetSportsDetailsList(sportId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> GetSportWithCategoriesAndDetails(int sportId)
        {
            return Json(await _sportService.GetSportWithCategoriesAndDetails(sportId), JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> EditSport(SportViewModel sportModel)
        {
            _sportService.EditSport(new DomainClasses.Sport
            {
                Id = sportModel.Id,
                Name = sportModel.Name
            });

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> AddSportCategory(AddSportCategoryViewModel sportCategoryModel)
        {
            var sportCategory = new SportCategory
            {
                Name = sportCategoryModel.SportCategoryName,
                SportId = sportCategoryModel.SportId
            };

            _sportService.AddSportCategory(sportCategory);

            await _dbContext.SaveChangesAsync();

            return Json(sportCategory.Id, JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> AddSportDetail(AddSportDetailViewModel sportDetailModel)
        {
            var sportDetail = new SportDetail
            {
                Name = sportDetailModel.SportDetailName,
                SportId = sportDetailModel.SportId
            };

            _sportService.AddSportDetail(sportDetail);

            await _dbContext.SaveChangesAsync();

            return Json(sportDetail.Id, JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> DeleteSportCategory(int sportCategoryId)
        {
            _sportService.DeleteSportCategory(sportCategoryId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> DeleteSportDetail(int sportDetailId)
        {
            _sportService.DeleteSportDetail(sportDetailId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}