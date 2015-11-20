using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult Index()
        {
            return View("Error");
        }

        // GET: Error
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

    }
}