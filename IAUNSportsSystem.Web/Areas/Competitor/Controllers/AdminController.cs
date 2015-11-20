using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.Competitor.Controllers
{
    [SiteAuthorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IDormService _dormService;
        private readonly ICompetitorService _competitorService;

        public AdminController(IDormService dormService, ICompetitorService competitorService)
        {
            _dormService = dormService;
            _competitorService = competitorService;
        }

        // GET: Competitor/Admin
       

    }
}