using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;
using System.Threading.Tasks;
using IAUNSportsSystem.Web.Areas.University.Models;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.University.Controllers
{
    public partial class HomeController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly IUniversityService _universityService;

        public HomeController(IDbContext dbContext, IUniversityService universityService)
        {
            _dbContext = dbContext;
            _universityService = universityService;
        }

        // GET: University/Home
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual async Task<ActionResult> GetUniversitiesList()
        {
            return Json(await _universityService.GetAll(), JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Add(AddUniversityViewModel universityModel)
        {
            var university = new DomainClasses.University
            {
                Name = universityModel.Name
            };

            _universityService.Add(university);

            await _dbContext.SaveChangesAsync();

            return Json(university.Id);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int universityId)
        {
            _universityService.Delete(universityId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Edit(AddUniversityViewModel universityModel)
        {
            _universityService.Edit(new DomainClasses.University
            {
                Id = universityModel.Id,
                Name = universityModel.Name
            });

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}