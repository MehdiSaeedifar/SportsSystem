using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Areas.TechnicalStaff.ViewModels;
using IAUNSportsSystem.Web.Infrastructure;

namespace IAUNSportsSystem.Web.Areas.CommonTechnicalStaff.Controllers
{
    public class UserPanelController : Controller
    {
        // GET: CommonTechnicalStaff/UserPanel
        private readonly IDbContext _dbContext;
        private readonly ITechnicalStaffService _technicalStaffService;
        private readonly IParticipationService _participationService;
        private readonly ITechnicalStaffRoleService _technicalStaffRoleService;
        private readonly ICommonTechnicalStaffService _commonTechnicalStaffService;
        private readonly ICompetitionService _competitionService;
        public UserPanelController(IDbContext dbContext, ITechnicalStaffService technicalStaffService, IParticipationService participationService, ITechnicalStaffRoleService technicalStaffRoleService, ICommonTechnicalStaffService commonTechnicalStaffService, ICompetitionService competitionService)
        {
            _dbContext = dbContext;
            _technicalStaffService = technicalStaffService;
            _participationService = participationService;
            _technicalStaffRoleService = technicalStaffRoleService;
            _commonTechnicalStaffService = commonTechnicalStaffService;
            _competitionService = competitionService;
        }


        public async Task<ActionResult> GetList(int competitionId)
        {
            return Json(await _competitionService.GetTechnicalStaffsList(competitionId, Convert.ToInt32(User.Identity.Name)), JsonRequestBehavior.AllowGet);
        }

        // GET: TechnicalStaff/UserPanel
        public async Task<ActionResult> Add(TechnicalStaffViewModel technicalStaffModel, int competitionId)
        {
            var canAddTechnicalStaff = await _competitionService
               .CanAddCommonTechnicalStaff(competitionId, Convert.ToInt32(User.Identity.Name));

            if (!canAddTechnicalStaff)
            {
                ModelState.AddModelError("", "امکان درج کادر اجرایی جدید وجود ندارد.");
                return this.JsonValidationErrors();
            }


            var tmpPath = Server.MapPath("~/App_Data/tmp/");


            var fullName = string.Format("{0}-{1}", technicalStaffModel.FirstName, technicalStaffModel.LastName).ApplyCorrectYeKe();


            var userImagePath = Server.MapPath("~/App_Data/TechnicalStaff_Image/");

            await
                CopyFileAsync(tmpPath + technicalStaffModel.Image,
                    userImagePath + string.Format("{0}-{1}", fullName, technicalStaffModel.Image));

            technicalStaffModel.Image = string.Format("{0}-{1}", fullName, technicalStaffModel.Image);


            var technicalStaff = new DomainClasses.TechnicalStaff
            {
                FirstName = technicalStaffModel.FirstName,
                BirthDate = technicalStaffModel.BirthDate,
                FatherName = technicalStaffModel.FatherName,
                Image = technicalStaffModel.Image,
                LastName = technicalStaffModel.LastName,
                NationalCode = technicalStaffModel.NationalCode,
                Id = technicalStaffModel.Id,
                MobileNumber = technicalStaffModel.MobileNumber
            };

            _commonTechnicalStaffService.Add(technicalStaff, Convert.ToInt32(User.Identity.Name),
                competitionId,
                technicalStaffModel.TechnicalStaffRoleId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> Edit(TechnicalStaffViewModel technicalStaffModel)
        {

            var selectedTechnicalStaff = await _technicalStaffService.Find(technicalStaffModel.Id);

            var userId = Convert.ToInt32(User.Identity.Name);

            if (selectedTechnicalStaff.IsApproved == null || selectedTechnicalStaff.IsApproved == true)
            {
                var canEdit = await _competitionService.CanEditCommonTechnicalStaff(technicalStaffModel.CompetitionId, userId);

                if (!canEdit)
                {
                    ModelState.AddModelError("", "امکان ویرایش کادر اجرایی وجود ندارد.");
                    return this.JsonValidationErrors();
                }

            }
            else
            {
                var canEdit = await _competitionService.CanEditRejectedCommonTechnicalStaff(technicalStaffModel.CompetitionId, userId);
                if (!canEdit)
                {
                    ModelState.AddModelError("", "امکان ویرایش کادر اجرایی وجود ندارد.");
                    return this.JsonValidationErrors();
                }
            }


            if (technicalStaffModel.Image != selectedTechnicalStaff.Image)
            {
                var tmpPath = Server.MapPath("~/App_Data/tmp/");

                var fullName = string.Format("{0}-{1}", technicalStaffModel.FirstName, technicalStaffModel.LastName).ApplyCorrectYeKe();

                var userImagePath = Server.MapPath("~/App_Data/TechnicalStaff_Image/");

                await
                    CopyFileAsync(tmpPath + technicalStaffModel.Image,
                        userImagePath + string.Format("{0}-{1}", fullName, technicalStaffModel.Image));
                try
                {
                    System.IO.File.Delete(userImagePath + selectedTechnicalStaff.Image);
                }
                catch (Exception)
                {
                }

                selectedTechnicalStaff.Image = string.Format("{0}-{1}", fullName, technicalStaffModel.Image);
            }

            selectedTechnicalStaff.FirstName = technicalStaffModel.FirstName;
            selectedTechnicalStaff.LastName = technicalStaffModel.LastName;
            selectedTechnicalStaff.FatherName = technicalStaffModel.FatherName;
            selectedTechnicalStaff.NationalCode = technicalStaffModel.NationalCode;
            selectedTechnicalStaff.BirthDate = technicalStaffModel.BirthDate;
            selectedTechnicalStaff.MobileNumber = technicalStaffModel.MobileNumber;


            selectedTechnicalStaff.IsApproved = null;


            var selectedCompetitionTechnicalStaff = await _commonTechnicalStaffService.GetCompetitionTechnicalStaff(technicalStaffModel.Id, Convert.ToInt32(User.Identity.Name));


            if (technicalStaffModel.TechnicalStaffRoleId != selectedCompetitionTechnicalStaff.TechnicalStaffRoleId)
                selectedCompetitionTechnicalStaff.TechnicalStaffRoleId = technicalStaffModel.TechnicalStaffRoleId;


            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }



        public async Task<ActionResult> GetByNationalCode(string nationalCode, int participationId)
        {
            var competitionId = await _participationService.GetCompetitionId(participationId);

            var technicalStaff = await _technicalStaffService.GetByNationalCode(nationalCode,
                Convert.ToInt32(User.Identity.Name), competitionId);

            return Json(technicalStaff, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetEditData(int technicalStaffId)
        {

            return Json(new
            {
                TechnicalStaff = await _commonTechnicalStaffService.GetTechnicalStaff(technicalStaffId, Convert.ToInt32(User.Identity.Name)),
                technicalStaffRoles = await _technicalStaffRoleService.GetAllCommonRoles()
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetAddData(int competitionId)
        {

            return Json(new
            {
                Competition = await _competitionService.GetCompetition(competitionId, Convert.ToInt32(User.Identity.Name)),
                TechnicalStaffRoles = await _technicalStaffRoleService.GetAllCommonRoles()
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Delete(int technicalStaffId)
        {
            var competitionId = await _commonTechnicalStaffService.GetCompetitionId(technicalStaffId);

            var canEdit = await _competitionService
               .CanDeleteCommonTechncialStaff(competitionId, Convert.ToInt32(User.Identity.Name));

            if (!canEdit)
            {
                ModelState.AddModelError("", "امکان حذف کادر اجرایی وجود ندارد.");
                return this.JsonValidationErrors();
            }

            _commonTechnicalStaffService.Delete(technicalStaffId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        public async Task CopyFileAsync(string sourcePath, string destinationPath)
        {
            using (Stream source = System.IO.File.Open(sourcePath, FileMode.Open))
            {
                using (Stream destination = System.IO.File.Create(destinationPath))
                {
                    await source.CopyToAsync(destination);
                }


            }
        }
    }
}