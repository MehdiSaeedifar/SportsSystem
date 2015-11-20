using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;
using System.Threading.Tasks;

namespace IAUNSportsSystem.Web.Areas.RepresentativeUser.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepresentativeUserService _representativeUserService;
        
        public HomeController(IRepresentativeUserService representativeUserService, INewsService newsService)
        {
            _representativeUserService = representativeUserService;
        }

        // GET: RepresentativeUser/Home
        public ActionResult Index()
        {
            return View();
        }

        
    }
}