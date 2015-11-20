using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.UserPanel.Controllers
{
    public class RegisterController : Controller
    {
        // GET: UserPanel/Register
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult ApprovedParticipations()
        {
            return PartialView();
        }
    }
}