using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Utilities;
using IAUNSportsSystem.Web.Areas.User.ViewModels;
using IAUNSportsSystem.Web.Filters;
using IAUNSportsSystem.Web.Infrastructure;
using Iris.Web.IrisMembership.System.Web.Helpers;
using PdfRpt.Core.Contracts;

namespace IAUNSportsSystem.Web.Areas.User.Controllers
{
    [SiteAuthorize(Roles = "admin")]
    public class HomeController : Controller
    {
        // GET: User/Home
        private readonly IDbContext _dbContext;
        private readonly IUserService _userService;

        public HomeController(IDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }


        public async Task<ActionResult> Get(int userId)
        {
            return Json(await _userService.Get(userId), JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> GetAll()
        {
            return Json(await _userService.GetAll(), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Add(UserViewModel userModel)
        {
            if (await _userService.IsEmailExist(userModel.Email))
            {
                ModelState.AddModelError("", "پست الکترنیکی وارد شده قبلا در سیستم ثبت شده است.");
                return this.JsonValidationErrors();
            }

            _userService.Add(new DomainClasses.User()
            {
                Email = userModel.Email,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Password = EncryptionHelper.Encrypt(userModel.Password, EncryptionHelper.Key),
                Role = userModel.Role
            });

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> Edit(UserViewModel userModel)
        {
            if (!await _userService.CanChangeEmail(userModel.Email, userModel.Id))
            {
                ModelState.AddModelError("", "پست الکترنیکی وارد شده قبلا در سیستم ثبت شده است.");
                return this.JsonValidationErrors();
            }

            var user = new DomainClasses.User()
            {
                Id = userModel.Id,
                Email = userModel.Email,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Role = userModel.Role
            };

            if (!string.IsNullOrEmpty(userModel.Password))
                user.Password = EncryptionHelper.Encrypt(userModel.Password, EncryptionHelper.Key);

            _userService.Edit(user);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> Delete(int userId)
        {
            _userService.Delete(userId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}