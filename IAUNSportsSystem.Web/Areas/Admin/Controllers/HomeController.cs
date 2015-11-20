using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.Admin.Controllers
{
    [SiteAuthorize(Roles = "admin")]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Admin
        public virtual ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetLayoutData()
        {
            return Json(new
            {
                User = new { fullName = await _userService.GetFullName(Convert.ToInt32(User.Identity.Name)) }
            }, JsonRequestBehavior.AllowGet);
        }


    }
}