using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Areas.TechnicalStaff.ViewModels;
using System.Threading.Tasks;
using IAUNSportsSystem.Web.Infrastructure;

namespace IAUNSportsSystem.Web.Areas.TechnicalStaff.Controllers
{
    public class UserPanelController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly ITechnicalStaffService _technicalStaffService;
        private readonly IParticipationService _participationService;
        private readonly ITechnicalStaffRoleService _technicalStaffRoleService;
        public UserPanelController(IDbContext dbContext, ITechnicalStaffService technicalStaffService, IParticipationService participationService, ITechnicalStaffRoleService technicalStaffRoleService)
        {
            _dbContext = dbContext;
            _technicalStaffService = technicalStaffService;
            _participationService = participationService;
            _technicalStaffRoleService = technicalStaffRoleService;
        }


        public async Task<ActionResult> GetList(int participationId)
        {
            return Json(await _participationService.GetTechnicalStaffsList(participationId, Convert.ToInt32(User.Identity.Name)), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetAddData(int participationId)
        {
            return Json(new
            {
                CompetitionSport = await _participationService.GetCompetitionSport(participationId, Convert.ToInt32(User.Identity.Name)),
                TechnicalStaffRoles = await _technicalStaffRoleService.GetAllParticipationRoles()
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Add(TechnicalStaffViewModel technicalStaffModel)
        {
            var canAddTechnicalStaff = await _participationService
                .CanAddTechnicalStaff(technicalStaffModel.ParticipationId, Convert.ToInt32(User.Identity.Name));

            if (!canAddTechnicalStaff)
            {
                ModelState.AddModelError("", "امکان درج کادر فنی جدید وجود ندارد.");
                return this.JsonValidationErrors();
            }

            var tmpPath = Server.MapPath("~/App_Data/tmp/");

            var fullName = string.Format("{0}-{1}", technicalStaffModel.FirstName, technicalStaffModel.LastName).ApplyCorrectYeKe();

            if (technicalStaffModel.Id == 0)
            {

                var userImagePath = Server.MapPath("~/App_Data/TechnicalStaff_Image/");

                await
                    CopyFileAsync(tmpPath + technicalStaffModel.Image,
                        userImagePath + string.Format("{0}-{1}", fullName, technicalStaffModel.Image));

                technicalStaffModel.Image = string.Format("{0}-{1}", fullName, technicalStaffModel.Image);
            }

            var technicalStaff = new DomainClasses.TechnicalStaff
            {
                FirstName = technicalStaffModel.FirstName,
                BirthDate = technicalStaffModel.BirthDate,
                FatherName = technicalStaffModel.FatherName,
                Image = technicalStaffModel.Image,
                LastName = technicalStaffModel.LastName,
                NationalCode = technicalStaffModel.NationalCode,
                Id = technicalStaffModel.Id
            };

            var similarParticipationIds =
                await _participationService.GetSimilarParticipationSport(technicalStaffModel.ParticipationId,
                Convert.ToInt32(User.Identity.Name));

            foreach (var participationId in similarParticipationIds)
            {
                _technicalStaffService.Add(technicalStaff, participationId, technicalStaffModel.TechnicalStaffRoleId);
            }

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> Edit(TechnicalStaffViewModel technicalStaffModel)
        {

            var selectedTechnicalStaff = await _technicalStaffService.Find(technicalStaffModel.Id);

            var userId = Convert.ToInt32(User.Identity.Name);

            if (selectedTechnicalStaff.IsApproved == null || selectedTechnicalStaff.IsApproved == true)
            {
                var canEdit = await _participationService.CanEditPerson(technicalStaffModel.ParticipationId, userId);

                if (!canEdit)
                {
                    ModelState.AddModelError("", "امکان ویرایش کادر فنی وجود ندارد.");
                    return this.JsonValidationErrors();
                }

            }
            else
            {
                var canEdit = await _participationService.CanEditRejectedPerson(technicalStaffModel.ParticipationId, userId);
                if (!canEdit)
                {
                    ModelState.AddModelError("", "امکان ویرایش کادر فنی وجود ندارد.");
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

            selectedTechnicalStaff.IsApproved = null;


            var selectedParticipationTechnicalStaff = await _technicalStaffService.GetParticipationTechnicalStaff(technicalStaffModel.ParticipationId, technicalStaffModel.Id);


            if (technicalStaffModel.TechnicalStaffRoleId != selectedParticipationTechnicalStaff.TechnicalStaffRoleId)
                selectedParticipationTechnicalStaff.TechnicalStaffRoleId = technicalStaffModel.TechnicalStaffRoleId;


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

        public async Task<ActionResult> GetEditData(int technicalStaffId, int participationId)
        {

            return Json(new
            {
                TechnicalStaff = await _technicalStaffService.GetTechnicalStaff(technicalStaffId,
                participationId, Convert.ToInt32(User.Identity.Name)),
                technicalStaffRoles = await _technicalStaffRoleService.GetAllParticipationRoles()
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Delete(int technicalStaffId, int participationId)
        {
            var canEdit = await _participationService
                .CanDeleteTechnicalStaff(technicalStaffId, Convert.ToInt32(User.Identity.Name));

            if (!canEdit)
            {
                ModelState.AddModelError("", "امکان حذف کادر فنی وجود ندارد.");
                return this.JsonValidationErrors();
            }

            var simailarParticipations = await _participationService
                .GetTechnicalStaffSimilarParticipationSport(participationId, technicalStaffId);

            if (simailarParticipations.Count > 1)
            {
                await _technicalStaffService.Delete(technicalStaffId, simailarParticipations.ToArray());
            }
            else
            {
                await _technicalStaffService.Delete(technicalStaffId, participationId);
            }


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