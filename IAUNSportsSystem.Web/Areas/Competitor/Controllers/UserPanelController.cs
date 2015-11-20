using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;
using System.Threading.Tasks;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.Web.Areas.Competitor.ViewModels;
using IAUNSportsSystem.Web.Infrastructure;

namespace IAUNSportsSystem.Web.Areas.Competitor.Controllers
{
    public class UserPanelController : Controller
    {
        // GET: Competitor/UserPanel
        private readonly IDbContext _dbContext;
        private readonly IParticipationService _participationService;
        private readonly ICompetitorService _competitorService;
        private readonly IStudyFieldService _studyFieldService;
        private readonly IStudyFieldDegreeService _studyFieldDegreeService;

        public UserPanelController(IDbContext dbContext, IParticipationService participationService,
            ICompetitorService competitorService, IStudyFieldService studyFieldService,
            IStudyFieldDegreeService studyFieldDegreeService)
        {
            _dbContext = dbContext;
            _participationService = participationService;
            _competitorService = competitorService;
            _studyFieldService = studyFieldService;
            _studyFieldDegreeService = studyFieldDegreeService;
        }

        public async Task<ActionResult> Index(int participationId)
        {
            return Json(await _participationService.GetCompetitorsList(participationId, Convert.ToInt32(User.Identity.Name)), JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> Delete(int competitorId)
        {
            var canDelete = await _participationService.CanDeleteCompetitor(competitorId, Convert.ToInt32(User.Identity.Name));

            if (!canDelete)
            {
                ModelState.AddModelError("", "بازه زمانی ثبت نام به پایان رسیده است.");
                return this.JsonValidationErrors();
            }

            _competitorService.Delete(competitorId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> Get(int competitorId, int participationId)
        {
            var userId = Convert.ToInt32(User.Identity.Name);

            var data = new
            {
                CompetitionSport = await _participationService.GetCompetitionSport(participationId, Convert.ToInt32(User.Identity.Name)),
                Competitor = await _competitorService.GetCompetitor(competitorId, userId),
                StudyFields = await _studyFieldService.GetAllAsync(),
                StudyFieldDegrees = await _studyFieldDegreeService.GetAllAsync(),
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Edit(CompetitorViewModel competitorModel)
        {
            var selectedCompetitor = await _competitorService.Find(competitorModel.Id);

            var userId = Convert.ToInt32(User.Identity.Name);

            if (selectedCompetitor.IsApproved == null || selectedCompetitor.IsApproved == true)
            {
                var canEdit = await _participationService.CanEditPerson(selectedCompetitor.ParticipateId, userId);

                if (!canEdit)
                {
                    ModelState.AddModelError("", "شما امکان ویرایش اطلاعات بازیکن را ندارید.");
                    return this.JsonValidationErrors();
                }
            }
            else
            {
                var canEdit = await _participationService.CanEditRejectedPerson(selectedCompetitor.ParticipateId, userId);

                if (!canEdit)
                {
                    ModelState.AddModelError("", "مهلت ویرایش اطلاعات بازیکن به پایان رسیده است.");
                    return this.JsonValidationErrors();
                }
            }


            var tmpPath = Server.MapPath("~/App_Data/tmp/");

            var fullName = string.Format("{0}-{1}", competitorModel.FirstName, competitorModel.LastName).ApplyCorrectYeKe();

            if (selectedCompetitor.UserImage != competitorModel.UserImage)
            {

                var userImagePath = Server.MapPath("~/App_Data/User_Image/");

                await
                    CopyFileAsync(tmpPath + competitorModel.UserImage,
                        userImagePath + string.Format("{0}-{1}", fullName, competitorModel.UserImage));

                System.IO.File.Delete(userImagePath + selectedCompetitor.UserImage);

                selectedCompetitor.UserImage = string.Format("{0}-{1}", fullName, competitorModel.UserImage);

            }

            if (selectedCompetitor.StudentCertificateImage != competitorModel.StudentCertificateImage)
            {
                var studentCertificateImagePath = Server.MapPath("~/App_Data/Student_Certificate_Image/");
                await
                    CopyFileAsync(tmpPath + competitorModel.StudentCertificateImage,
                        studentCertificateImagePath +
                        string.Format("{0}-{1}", fullName, competitorModel.StudentCertificateImage));


                System.IO.File.Delete(studentCertificateImagePath + selectedCompetitor.StudentCertificateImage);


                selectedCompetitor.StudentCertificateImage = string.Format("{0}-{1}", fullName,
                    competitorModel.StudentCertificateImage);
            }

            if (selectedCompetitor.InsuranceImage != competitorModel.InsuranceImage)
            {
                var insuranceImagePath = Server.MapPath("~/App_Data/Insurance_Image/");
                await
                    CopyFileAsync(tmpPath + competitorModel.InsuranceImage,
                        insuranceImagePath + string.Format("{0}-{1}", fullName, competitorModel.InsuranceImage));


                System.IO.File.Delete(insuranceImagePath + selectedCompetitor.InsuranceImage);

                selectedCompetitor.InsuranceImage = string.Format("{0}-{1}", fullName, competitorModel.InsuranceImage);
            }

            if (selectedCompetitor.AzmoonConfirmationImage != competitorModel.AzmoonConfirmationImage)
            {
                var azmoonImagePath = Server.MapPath("~/App_Data/Azmoon_Confirmation_Image/");
                await
                    CopyFileAsync(tmpPath + competitorModel.AzmoonConfirmationImage,
                        azmoonImagePath + string.Format("{0}-{1}", fullName, competitorModel.AzmoonConfirmationImage));

                System.IO.File.Delete(azmoonImagePath + selectedCompetitor.AzmoonConfirmationImage);

                selectedCompetitor.AzmoonConfirmationImage = string.Format("{0}-{1}", fullName,
                    competitorModel.AzmoonConfirmationImage);
            }


            selectedCompetitor.FirstName = competitorModel.FirstName;
            selectedCompetitor.LastName = competitorModel.LastName;
            selectedCompetitor.InsuranceEndDate = competitorModel.InsuranceEndDate;
            selectedCompetitor.StudyFieldId = competitorModel.StudyFieldId;
            selectedCompetitor.StudyFieldDegreeId = competitorModel.StudyFieldDegreeId;
            selectedCompetitor.NationalCode = competitorModel.NationalCode;
            selectedCompetitor.BirthDate = competitorModel.BirthDate;
            selectedCompetitor.FatherName = competitorModel.FatherName;
            selectedCompetitor.Email = competitorModel.Email;
            selectedCompetitor.MobileNumber = competitorModel.MobileNumber;
            selectedCompetitor.StudentNumber = competitorModel.StudentNumber;
            selectedCompetitor.Weight = competitorModel.Weight;
            selectedCompetitor.Height = competitorModel.Height;
            selectedCompetitor.PhoneNumber = competitorModel.PhoneNumber;
            selectedCompetitor.InsuranceNumber = competitorModel.InsuranceNumber;

            selectedCompetitor.IsApproved = null;

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