using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Admin.Controllers
{
    public class CompetitionSportController : Controller
    {
        // GET: Admin/CompetitionSport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return PartialView();
        }

    }
}