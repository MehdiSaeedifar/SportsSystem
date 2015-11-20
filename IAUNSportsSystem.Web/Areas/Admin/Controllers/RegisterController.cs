using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Admin.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Admin/Register
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult SportsList()
        {
            return PartialView();
        }

        public ActionResult CompetitorsList()
        {
            return PartialView();
        }

        public ActionResult TechnicalStaffsList()
        {
            return PartialView();
        }

        public ActionResult CommonTechnicalStaffsList()
        {
            return PartialView();
        }

    }
}