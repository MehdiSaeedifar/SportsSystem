using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Areas.StudyField.ViewModels;
using IAUNSportsSystem.Web.Areas.StudyFieldDegree.ViewModels;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.StudyFieldDegree.Controllers
{
    public class HomeController : Controller
    {
        // GET: StudyFieldDegree/Home
        private readonly IDbContext _dbContext;
        private readonly IStudyFieldDegreeService _studyFieldDegreeService;

        public HomeController(IDbContext dbContext, IStudyFieldDegreeService studyFieldDegreeService)
        {
            _dbContext = dbContext;
            _studyFieldDegreeService = studyFieldDegreeService;
        }

        public async Task<ActionResult> GetAll()
        {
            return Json(await _studyFieldDegreeService.GetAllAsync(), JsonRequestBehavior.AllowGet);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Add(StudyFieldDegreeViewModel studyFieldDegreeModel)
        {
            var studyField = new DomainClasses.StudyFieldDegree
            {
                Id = studyFieldDegreeModel.Id,
                Name = studyFieldDegreeModel.Name
            };

            _studyFieldDegreeService.Add(studyField);

            await _dbContext.SaveChangesAsync();

            return Json(studyField.Id);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Edit(StudyFieldDegreeViewModel studyFieldDegreeModel)
        {
            _studyFieldDegreeService.Edit(new DomainClasses.StudyFieldDegree
            {
                Id = studyFieldDegreeModel.Id,
                Name = studyFieldDegreeModel.Name
            });

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SiteAuthorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int studyFieldDegreeId)
        {
            _studyFieldDegreeService.Delete(studyFieldDegreeId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}