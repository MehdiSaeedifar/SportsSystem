using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.UserPanel.Controllers
{
    [SiteAuthorize(Roles = "user")]
    public class HomeController : Controller
    {
        private readonly IRepresentativeUserService _representativeUserService;
        private readonly ICompetitorService _competitorService;
        private readonly ITechnicalStaffService _technicalStaffService;
        private readonly ICommonTechnicalStaffService _commonTechnicalStaffService;

        public HomeController(IRepresentativeUserService representativeUserService, ICompetitorService competitorService, ITechnicalStaffService technicalStaffService, ICommonTechnicalStaffService commonTechnicalStaffService)
        {
            _representativeUserService = representativeUserService;
            _competitorService = competitorService;
            _technicalStaffService = technicalStaffService;
            _commonTechnicalStaffService = commonTechnicalStaffService;
        }

        // GET: UserPanel/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return PartialView();
        }

        public async Task<ActionResult> GetLayoutData()
        {
            return Json(new
            {
                User = new { fullName = await _representativeUserService.GetFullName(Convert.ToInt32(User.Identity.Name)) }
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetRejectedPersons()
        {
            var userId = Convert.ToInt32(User.Identity.Name);

            var rejectedTechnicalStaffs = await _technicalStaffService.GetRejectedTechnicalStaffsList(userId);

            var rejectedCommonTechnicalStaffs = await _commonTechnicalStaffService.GetRejectedTechnicalStaffsList(userId);

            ((List<RejectedTechnicalStaff>)rejectedTechnicalStaffs).AddRange(rejectedCommonTechnicalStaffs);

            return Json(new
            {
                TechnicalStaffs = rejectedTechnicalStaffs,
                Competitors = await _competitorService.GetRejectedCompetitorsList(userId),
            }, JsonRequestBehavior.AllowGet);
        }

    }
}