using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.UserPanel.Controllers
{
    public class CardController : Controller
    {
        // GET: UserPanel/Card
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}