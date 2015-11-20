using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.DomainClasses;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Areas.Participation.ViewModels;
using IAUNSportsSystem.Web.Filters;
using IAUNSportsSystem.Web.Infrastructure;
using IoFile = System.IO.File;

namespace IAUNSportsSystem.Web.Areas.Participation.Controllers
{
    public partial class CompetitorController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly IStudyFieldService _studyFieldService;
        private readonly IStudyFieldDegreeService _studyFieldDegreeService;
        private readonly ICompetitorService _competitorService;
        private readonly IParticipationService _participationService;
        private readonly ICompetitionService _competitionService;

        public CompetitorController(IDbContext dbContext, IStudyFieldService studyFieldService, IStudyFieldDegreeService studyFieldDegreeService, ICompetitorService competitorService, IParticipationService participationService, ICompetitionService competitionService)
        {
            _dbContext = dbContext;
            _studyFieldService = studyFieldService;
            _studyFieldDegreeService = studyFieldDegreeService;
            _competitorService = competitorService;
            _participationService = participationService;
            _competitionService = competitionService;
        }

        // GET: Participate/Competitor
        public virtual ActionResult Index()
        {
            return PartialView();
        }


        [SiteAuthorize(Roles = "user")]
        [HttpPost]
        public virtual async Task<ActionResult> Add(AddCompetitorViewModel competitorModel)
        {

            var canAddCompetitor = await _participationService.CanAddCompetitor(competitorModel.ParticipationId, Convert.ToInt32(User.Identity.Name));

            if (!canAddCompetitor)
            {
                ModelState.AddModelError("", "شما امکان ثبت بازیکن جدید را ندارید.");
                return this.JsonValidationErrors();
            }

            var tmpPath = Server.MapPath("~/App_Data/tmp/");

            var userImagePath = Server.MapPath("~/App_Data/User_Image/");

            var studentCertificateImagePath = Server.MapPath("~/App_Data/Student_Certificate_Image/");

            var insuranceImagePath = Server.MapPath("~/App_Data/Insurance_Image/");

            var azmoonImagePath = Server.MapPath("~/App_Data/Azmoon_Confirmation_Image/");

            var fullName = string.Format("{0}-{1}", competitorModel.FirstName, competitorModel.LastName).ApplyCorrectYeKe();



            await CopyFileAsync(tmpPath + competitorModel.UserImage, userImagePath + string.Format("{0}-{1}", fullName, competitorModel.UserImage));

            competitorModel.UserImage = string.Format("{0}-{1}", fullName, competitorModel.UserImage);


            await CopyFileAsync(tmpPath + competitorModel.StudentCertificateImage, studentCertificateImagePath + string.Format("{0}-{1}", fullName, competitorModel.StudentCertificateImage));

            competitorModel.StudentCertificateImage = string.Format("{0}-{1}", fullName, competitorModel.StudentCertificateImage);


            await CopyFileAsync(tmpPath + competitorModel.InsuranceImage, insuranceImagePath + string.Format("{0}-{1}", fullName, competitorModel.InsuranceImage));

            competitorModel.InsuranceImage = string.Format("{0}-{1}", fullName, competitorModel.InsuranceImage);


            await CopyFileAsync(tmpPath + competitorModel.AzmoonConfirmationImage, azmoonImagePath + string.Format("{0}-{1}", fullName, competitorModel.AzmoonConfirmationImage));

            competitorModel.AzmoonConfirmationImage = string.Format("{0}-{1}", fullName, competitorModel.AzmoonConfirmationImage);


            var competitor = new DomainClasses.Competitor()
            {
                FirstName = competitorModel.FirstName,
                LastName = competitorModel.LastName,
                FatherName = competitorModel.FatherName,
                BirthDate = competitorModel.BirthDate,
                ParticipateId = competitorModel.ParticipationId,
                PhoneNumber = competitorModel.PhoneNumber,
                StudentCertificateImage = competitorModel.StudentCertificateImage,
                StudentNumber = competitorModel.StudentNumber,
                StudyFieldId = competitorModel.StudyFieldId,
                StudyFieldDegreeId = competitorModel.StudyFieldDegreeId,
                UserImage = competitorModel.UserImage,
                Weight = competitorModel.Weight,
                Height = competitorModel.Height,
                NationalCode = competitorModel.NationalCode,
                MobileNumber = competitorModel.MobileNumber,
                Email = competitorModel.Email,
                InsuranceNumber = competitorModel.InsuranceNumber,
                InsuranceImage = competitorModel.InsuranceImage,
                InsuranceEndDate = competitorModel.InsuranceEndDate,
                AzmoonConfirmationImage = competitorModel.AzmoonConfirmationImage
            };

            _competitorService.Add(competitor);

            await _dbContext.SaveChangesAsync();

            return Json(new { competitor.Id, competitor.UserImage });
        }

        public virtual async Task<ActionResult> GetStudyFieldsList()
        {
            return Json(await _studyFieldService.GetAllAsync(), JsonRequestBehavior.AllowGet);
        }

        public virtual async Task<ActionResult> GetStudyFieldsDegreesList()
        {
            return Json(await _studyFieldDegreeService.GetAllAsync(), JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "user")]
        [HttpPost]
        public async Task<ActionResult> GetAddCompetitorData(int participationId)
        {

            var data = new
            {
                CompetitionSport = await _participationService.GetCompetitionSport(participationId, Convert.ToInt32(User.Identity.Name)),
                StudyFields = await _studyFieldService.GetAllAsync(),
                StudyFieldDegrees = await _studyFieldDegreeService.GetAllAsync(),
            };

            return Json(data);
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

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> GetCompetitorsList(int participationId)
        {
            return Json(await _participationService.GetPartricipationForCompetitiorsList(participationId),
                JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetCompetitor(int competitorId)
        {
            return Json(await _competitorService.GetCompetitor(competitorId), JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> EditApproval(EditApprovalViewModel competitor)
        {
            _competitorService.EditApproval(new DomainClasses.Competitor()
            {
                Id = competitor.CompetitorId,
                DormId = competitor.DormId,
                Error = competitor.Error,
                IsApproved = competitor.IsApproved,
                DormNumber = competitor.DormNumber
            });

            await _dbContext.SaveChangesAsync();

            return Json(true);
        }



    }
}