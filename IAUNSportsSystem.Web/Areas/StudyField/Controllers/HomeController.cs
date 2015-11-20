using IAUNSportsSystem.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.Web.Areas.StudyField.ViewModels;
using IAUNSportsSystem.DataLayer;
using System.Net;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.StudyField.Controllers
{
    public class HomeController : Controller
    {
        // GET: StudyField/Home
        private readonly IDbContext _dbContext;
        private readonly IStudyFieldService _studyFieldService;

        public HomeController(IDbContext dbContext, IStudyFieldService studyFieldService)
        {
            _dbContext = dbContext;
            _studyFieldService = studyFieldService;
        }

        public async Task<ActionResult> GetAll()
        {
            return Json(await _studyFieldService.GetAllAsync(), JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Add(StudyFieldViewModel studyFieldModel)
        {
            var studyField = new DomainClasses.StudyField
            {
                Id = studyFieldModel.Id,
                Name = studyFieldModel.Name
            };

            _studyFieldService.Add(studyField);

            await _dbContext.SaveChangesAsync();

            return Json(studyField.Id);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Edit(StudyFieldViewModel studyFieldModel)
        {
            _studyFieldService.Edit(new DomainClasses.StudyField
            {
                Id = studyFieldModel.Id,
                Name = studyFieldModel.Name
            });

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int studyFieldId)
        {
            _studyFieldService.Delete(studyFieldId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}