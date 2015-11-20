using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Admin.Controllers
{
    public class TechnicalStaffRoleController : Controller
    {
        // GET: Admin/TechnicalStaffRole
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}