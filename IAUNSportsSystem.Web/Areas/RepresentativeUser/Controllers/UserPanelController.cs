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
using IAUNSportsSystem.Web.Areas.RepresentativeUser.ViewModels;
using IAUNSportsSystem.Web.Infrastructure;

namespace IAUNSportsSystem.Web.Areas.RepresentativeUser.Controllers
{
    public class UserPanelController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly IRepresentativeUserService _representativeUserService;

        public UserPanelController(IDbContext dbContext, IRepresentativeUserService representativeUserService)
        {
            _representativeUserService = representativeUserService;
            _dbContext = dbContext;
        }

        public async Task<ActionResult> GetEditData()
        {
            return Json(await _representativeUserService.Get(Convert.ToInt32(User.Identity.Name)),
                JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Edit(RepresentativeUserViewModel representativeUserModel)
        {
            var userId = Convert.ToInt32(User.Identity.Name);

            if (!await _representativeUserService.CanEditEmail(userId, representativeUserModel.Email))
            {
                ModelState.AddModelError("", "پست الکترونیکی وارد شده قبلا توسط فرد دیگری ثبت شده است.");
                return this.JsonValidationErrors();
            }

            var selectedUser = await _representativeUserService.FindById(userId);

            selectedUser.FirstName = representativeUserModel.FirstName;
            selectedUser.LastName = representativeUserModel.LastName;
            selectedUser.MobileNumber = representativeUserModel.MobileNumber;
            selectedUser.NationalCode = representativeUserModel.NationalCode;
            selectedUser.FatherName = representativeUserModel.FatherName;
            selectedUser.Email = representativeUserModel.Email;

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel changePasswordModel)
        {
            var userId = Convert.ToInt32(User.Identity.Name);

            var currentPassword = EncryptionHelper.Decrypt(await _representativeUserService.GetPassword(userId), EncryptionHelper.Key);

            if (currentPassword != changePasswordModel.CurrentPassword)
            {
                ModelState.AddModelError("", "کلمه عبور فعلی اشتباه است.");
                return this.JsonValidationErrors();
            }

            _representativeUserService.ChangePassword(userId, EncryptionHelper.Encrypt(changePasswordModel.NewPassword, EncryptionHelper.Key));

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}