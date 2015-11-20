using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.UserPanel.Controllers
{
    public class ReadinessController : Controller
    {
        // GET: UserPanel/Readiness
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult CompetitionSportsList()
        {
            return PartialView();
        }

        public ActionResult ConfirmCompetitionSportsList()
        {
            return PartialView();
        }
    }
}