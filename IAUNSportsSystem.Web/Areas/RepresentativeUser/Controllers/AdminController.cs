using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Utilities;
using IAUNSportsSystem.Web.Areas.RepresentativeUser.ViewModels;
using IAUNSportsSystem.Web.Filters;
using IAUNSportsSystem.Web.Infrastructure;
using Iris.Web.IrisMembership.System.Web.Helpers;

namespace IAUNSportsSystem.Web.Areas.RepresentativeUser.Controllers
{
    [SiteAuthorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly IRepresentativeUserService _representativeUserService;

        public AdminController(IDbContext dbContext, IRepresentativeUserService representativeUserService)
        {
            _dbContext = dbContext;
            _representativeUserService = representativeUserService;
        }

        // GET: RepresentativeUser/Admin
        public async Task<ActionResult> GetAll()
        {
            return Json(await _representativeUserService.GetAll(), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Add(RegisterRepresentativeUser representativeUserModel)
        {

            if (await _representativeUserService.IsExistByEmail(representativeUserModel.Email))
            {
                ModelState.AddModelError("", "پست الکترونیکی وارد شده قبلا در سیستم ثبت شده است.");
                return this.JsonValidationErrors();
            }

            var representativeUser = new DomainClasses.RepresentativeUser
            {
                Email = representativeUserModel.Email,
                FirstName = representativeUserModel.FirstName,
                LastName = representativeUserModel.LastName,
                FatherName = representativeUserModel.FatherName,
                MobileNumber = representativeUserModel.MobileNumber,
                NationalCode = representativeUserModel.NationalCode,
                UniversityId = representativeUserModel.UniversityId,
                Password = EncryptionHelper.Encrypt(
                    !string.IsNullOrEmpty(representativeUserModel.Password)
                        ? representativeUserModel.Password
                        : CreatePassword(6), EncryptionHelper.Key
                    ),
            };


            _representativeUserService.Add(representativeUser);

            await _dbContext.SaveChangesAsync();

            return Json(representativeUser.Id);
        }

        public async Task<ActionResult> Edit(RegisterRepresentativeUser representativeUserModel)
        {

            if (!await _representativeUserService.CanEditEmail(representativeUserModel.Id, representativeUserModel.Email))
            {
                ModelState.AddModelError("", "پست الکترونیکی وارد شده قبلا در سیستم ثبت شده است.");
                return this.JsonValidationErrors();
            }

            var representativeUser = new DomainClasses.RepresentativeUser
            {
                Id = representativeUserModel.Id,
                Email = representativeUserModel.Email,
                FirstName = representativeUserModel.FirstName,
                LastName = representativeUserModel.LastName,
                FatherName = representativeUserModel.FatherName,
                MobileNumber = representativeUserModel.MobileNumber,
                NationalCode = representativeUserModel.NationalCode,
                UniversityId = representativeUserModel.UniversityId,
                Password = EncryptionHelper.Encrypt(
                    !string.IsNullOrEmpty(representativeUserModel.Password)
                        ? representativeUserModel.Password
                        : CreatePassword(6), EncryptionHelper.Key
                    ),
            };


            _representativeUserService.Edit(representativeUser);

            await _dbContext.SaveChangesAsync();

            return Json(representativeUser.Id);
        }

        public async Task<ActionResult> Delete(int representativeUserId)
        {
            _representativeUserService.Delete(representativeUserId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }




    }
}