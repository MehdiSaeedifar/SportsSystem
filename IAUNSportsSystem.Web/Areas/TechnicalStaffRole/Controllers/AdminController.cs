using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Areas.TechnicalStaffRole.ViewModels;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.TechnicalStaffRole.Controllers
{
    [SiteAuthorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly ITechnicalStaffRoleService _technicalStaffRoleService;

        public AdminController(IDbContext dbContext, ITechnicalStaffRoleService technicalStaffRoleService)
        {
            _dbContext = dbContext;
            _technicalStaffRoleService = technicalStaffRoleService;
        }

        public async Task<ActionResult> GetAll()
        {
            return Json(await _technicalStaffRoleService.GetAllRoles(), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Add(TechnicalStaffRoleModel technicalStaffRoleModel)
        {
            var technicalStaffRole = new DomainClasses.TechnicalStaffRole
            {
                IsCommon = technicalStaffRoleModel.IsCommon,
                Name = technicalStaffRoleModel.Name,
            };

            _technicalStaffRoleService.Add(technicalStaffRole);

            await _dbContext.SaveChangesAsync();

            return Json(technicalStaffRole.Id);
        }

        public async Task<ActionResult> Edit(TechnicalStaffRoleModel technicalStaffRoleModel)
        {
            var technicalStaffRole = new DomainClasses.TechnicalStaffRole
            {
                Id = technicalStaffRoleModel.Id,
                IsCommon = technicalStaffRoleModel.IsCommon,
                Name = technicalStaffRoleModel.Name,
            };

            _technicalStaffRoleService.Edit(technicalStaffRole);

            await _dbContext.SaveChangesAsync();

            return Json(technicalStaffRole.Id);
        }

        public async Task<ActionResult> Delete(int technicalStaffRoleId)
        {
            _technicalStaffRoleService.Delete(technicalStaffRoleId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);

        }
    }
}