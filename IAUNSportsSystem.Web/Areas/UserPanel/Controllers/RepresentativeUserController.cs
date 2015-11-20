using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.UserPanel.Controllers
{
    public class RepresentativeUserController : Controller
    {
        // GET: UserPanel/User
        public ActionResult Edit()
        {
            return PartialView();
        }

        public ActionResult ChangePassword()
        {
            return PartialView();
        }
    }
}