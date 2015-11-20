using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.UserPanel.Controllers
{
    public class CompetitorController : Controller
    {
        // GET: UserPanel/Competitor
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult Edit()
        {
            return PartialView();
        }
    }
}