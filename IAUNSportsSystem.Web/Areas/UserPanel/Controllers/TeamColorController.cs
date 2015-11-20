using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.UserPanel.Controllers
{
    public class TeamColorController : Controller
    {
        // GET: UserPanel/TeamColor
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}