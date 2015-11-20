using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;
using System.Threading.Tasks;

namespace IAUNSportsSystem.Web.Areas.UserPanel.Controllers
{
    public class CommonTechnicalStaffController : Controller
    {
        // GET: UserPanel/TechnicalStaff
        private readonly ITechnicalStaffRoleService _technicalStaffRoleService;

        public CommonTechnicalStaffController(ITechnicalStaffRoleService technicalStaffRoleService)
        {
            _technicalStaffRoleService = technicalStaffRoleService;
        }

        public ActionResult Index()
        {
            return PartialView();
        }
        public ActionResult Add()
        {
            return PartialView();
        }

        public ActionResult Edit()
        {
            return PartialView();
        }
    
    }
}