using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using System.Threading.Tasks;
using IAUNSportsSystem.Web.Areas.Dorm.Models;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.Dorm.Controllers
{
    [SiteAuthorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly IDormService _dormService;

        public AdminController(IDbContext dbContext, IDormService dormService)
        {
            _dbContext = dbContext;
            _dormService = dormService;
        }
        // GET: Dorm/Admin
        public async Task<ActionResult> GetAll()
        {
            return Json(await _dormService.GetAll(), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Add(DormViewModel dormModel)
        {
            var dorm = new DomainClasses.Dorm()
            {
                Name = dormModel.Name
            };

            _dormService.Add(dorm);

            await _dbContext.SaveChangesAsync();

            return Json(dorm.Id);
        }

        public async Task<ActionResult> Edit(DormViewModel dormModel)
        {
            var dorm = new DomainClasses.Dorm()
            {
                Id = dormModel.Id,
                Name = dormModel.Name
            };

            _dormService.Edit(dorm);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> Delete(int dormId)
        {
            _dormService.Delete(dormId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}