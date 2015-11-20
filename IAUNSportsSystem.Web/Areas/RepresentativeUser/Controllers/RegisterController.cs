using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Areas.RepresentativeUser.ViewModels;
using System.Threading.Tasks;

namespace IAUNSportsSystem.Web.Areas.RepresentativeUser.Controllers
{
    public partial class RegisterController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly IRepresentativeUserService _representativeUserService;

        public RegisterController(IDbContext dbContext, IRepresentativeUserService representativeUserService)
        {
            _dbContext = dbContext;
            _representativeUserService = representativeUserService;
        }

        // GET: RepresentativeUser/Register
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public virtual async Task<ActionResult> Index(RegisterRepresentativeUser representativeUser)
        {
            _representativeUserService.Add(new DomainClasses.RepresentativeUser()
            {
                FirstName = representativeUser.FirstName,
                LastName = representativeUser.LastName,
                FatherName = representativeUser.FatherName,
                MobileNumber = representativeUser.MobileNumber,
                NationalCode = representativeUser.NationalCode,
                Email = representativeUser.Email,
                UniversityId = representativeUser.UniversityId
            });

            await _dbContext.SaveChangesAsync();

            return View();
        }
    }
}