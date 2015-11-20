using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Admin.Controllers
{
    public class ReadinessController : Controller
    {
        // GET: Admin/Readiness
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult UserList()
        {
            return PartialView();
        }

        public ActionResult CompetitorsList()
        {
            return PartialView();
        }
    }
}