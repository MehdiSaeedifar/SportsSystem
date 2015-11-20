using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Admin.Controllers
{
    public class DormController : Controller
    {
        // GET: Admin/Dorm
        public ActionResult Index()
        {
            return PartialView();
        }

    }
}