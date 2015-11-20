using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Areas.StudyFieldDegree.ViewModels;

namespace IAUNSportsSystem.Web.Areas.SlideShow.Controllers
{
    public class HomeController : Controller
    {
        // GET: SlideShow/Home
        private readonly IDbContext _dbContext;
        private readonly ISlideShowService _slideShowService;

        public HomeController(IDbContext dbContext, ISlideShowService slideShowService)
        {
            _dbContext = dbContext;
            _slideShowService = slideShowService;
        }

        public async Task<ActionResult> Get(int slideShowItemId)
        {
            return Json(await _slideShowService.Get(slideShowItemId), JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> GetAll()
        {
            return Json(await _slideShowService.GetAll(), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Add(SlideShowItemModel slideShowItemModel)
        {
            var slideShowItem = new DomainClasses.SlideShowItem()
            {
                Title = slideShowItemModel.Title,
                Image = slideShowItemModel.Image,
                Link = slideShowItemModel.Link,
                Order = slideShowItemModel.Order,
                Text = slideShowItemModel.Text
            };

            _slideShowService.Add(slideShowItem);

            await _dbContext.SaveChangesAsync();

            return Json(slideShowItem.Id);
        }

        public async Task<ActionResult> Edit(SlideShowItemModel slideShowItemModel)
        {
            var slideShowItem = new DomainClasses.SlideShowItem()
            {
                Id = slideShowItemModel.Id,
                Title = slideShowItemModel.Title,
                Image = slideShowItemModel.Image,
                Link = slideShowItemModel.Link,
                Order = slideShowItemModel.Order,
                Text = slideShowItemModel.Text
            };

            _slideShowService.Edit(slideShowItem);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> Delete(int slideShowItemId)
        {
            _slideShowService.Delete(slideShowItemId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}