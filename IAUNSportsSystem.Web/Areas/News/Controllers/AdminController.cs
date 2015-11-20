using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.ServiceLayer;
using System.Threading.Tasks;
using IAUNSportsSystem.Utilities.HtmlCleaner;
using IAUNSportsSystem.Web.Areas.News.ViewModels;
using IAUNSportsSystem.Web.Filters;

namespace IAUNSportsSystem.Web.Areas.News.Controllers
{
    [SiteAuthorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly INewsService _newsService;

        public AdminController(IDbContext dbContext, INewsService newsService)
        {
            _dbContext = dbContext;
            _newsService = newsService;
        }

        public async Task<ActionResult> GetAll()
        {
            return Json(await _newsService.GetAll(), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Add(NewsViewModel newsModel)
        {
            var news = new DomainClasses.News()
            {
                Title = newsModel.Title,
                Body = newsModel.Body.ToSafeHtml(),
                CreatedDate = DateTime.Now
            };

            _newsService.Add(news);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        public async Task<ActionResult> Edit(NewsViewModel newsModel)
        {
            _newsService.Edit(new DomainClasses.News()
            {
                Id = newsModel.Id,
                Title = newsModel.Title,
                Body = newsModel.Body
            });

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> GetEditData(int newsId)
        {
            return Json(await _newsService.Get(newsId), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Delete(int newsId)
        {
            _newsService.Delete(newsId);

            await _dbContext.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}