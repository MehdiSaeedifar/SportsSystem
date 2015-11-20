using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Admin.Controllers
{
    public class RepresentativeUserController : Controller
    {
        // GET: Admin/RepresentativeUser
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}