using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;
using System.Threading.Tasks;
using IAUNSportsSystem.Models;

namespace IAUNSportsSystem.Web.Areas.News.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsService _newsService;

        public HomeController(INewsService newsService)
        {
            _newsService = newsService;
        }

        // GET: News/Home
        public async Task<ActionResult> Show(int id)
        {
            var news = await _newsService.Get(id);

            return View(news);
        }

        public async Task<ActionResult> List(int? page)
        {
            var pageIndex = (page ?? 1) - 1;

            const int pageSize = 5;

            var totalNewsCount = await _newsService.Count();

            var news = await _newsService.GetNewsList(pageIndex, pageSize);

            var newsesAsIPagedList = new StaticPagedList<NewsModel>(news, pageIndex + 1, pageSize, totalNewsCount);

            ViewBag.OnePageOfNewses = newsesAsIPagedList;

            return View();
        }


    }
}