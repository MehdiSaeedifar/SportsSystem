using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Admin.Controllers
{
    public class CompetitionRepresentativeUserController : Controller
    {
        // GET: Admin/CompetitionRepresentativeUser
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}