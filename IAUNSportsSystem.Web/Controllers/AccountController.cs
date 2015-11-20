using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.DomainClasses;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Infrastructure;
using IAUNSportsSystem.Web.ViewModels;
using IAUNSportsSystem.Web.PersianCaptcha;
using System.Threading.Tasks;
using IAUNSportsSystem.Utilities;
using Iris.Web.IrisMembership;
using Iris.Web.IrisMembership.System.Web.Helpers;
using Microsoft.Ajax.Utilities;
using PdfRpt.Core.Contracts;

namespace IAUNSportsSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly IRepresentativeUserService _representativeUserService;
        private readonly IFormsAuthenticationService _formsAuthentication;
        private readonly IUserService _userService;

        public AccountController(IDbContext dbContext, IRepresentativeUserService representativeUserService, IFormsAuthenticationService formsAuthentication, IUserService userService)
        {
            _dbContext = dbContext;
            _representativeUserService = representativeUserService;
            _formsAuthentication = formsAuthentication;
            _userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {

            if (User.Identity.IsAuthenticated && User.IsInRole("user"))
            {
                ViewBag.UserPanelUrl = Url.Action("Index", "Home", new { area = "UserPanel" }) + "/#/dashboard";
            }
            return PartialView();
        }

        [HttpPost]
        [ValidateCaptcha(ExpireTimeCaptchaCodeBySeconds = 600)]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {

            if (!ModelState.IsValid)
                return this.JsonValidationErrors();

            var loginResult = await _representativeUserService
                .Login(loginModel.Email, EncryptionHelper.Encrypt(loginModel.Password, EncryptionHelper.Key));

            if (!loginResult.IsValid)
            {
                ModelState.AddModelError(string.Empty, "پست الکترونیکی یا کلمه عبور نامعتبر است.");
                return this.JsonValidationErrors();
            }

            _formsAuthentication.SignIn(new IrisUser
            {
                UserName = loginResult.UserId.ToString(),

                Role = new IrisRole
                {
                    Name = "user"
                }

            }, false);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult LoginCompetition()
        {
            return PartialView();
        }

        public async Task<ActionResult> IsExistByEmail(string term)
        {
            return Json(await _representativeUserService.IsExistByEmail(term), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SignOut()
        {
            _formsAuthentication.SignOut();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult LoginAdmin()
        {

            if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
            {
                ViewBag.UserPanelUrl = Url.Action("Index", "Home", new { area = "Admin" }) + "/#/";
            }

            return PartialView();
        }

        [HttpPost]
        [ValidateCaptcha(ExpireTimeCaptchaCodeBySeconds = 600)]
        public async Task<ActionResult> LoginAdmin(LoginModel loginModel)
        {

            if (!ModelState.IsValid)
                return this.JsonValidationErrors();

            var loginResult = await _userService
                .Login(loginModel.Email, EncryptionHelper.Encrypt(loginModel.Password, EncryptionHelper.Key));

            if (!loginResult.IsValid)
            {
                ModelState.AddModelError(string.Empty, "پست الکترونیکی یا کلمه عبور نامعتبر است.");
                return this.JsonValidationErrors();
            }

            _formsAuthentication.SignIn(new IrisUser
            {
                UserName = loginResult.UserId.ToString(),

                Role = new IrisRole
                {
                    Name = "admin"
                }

            }, false);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}