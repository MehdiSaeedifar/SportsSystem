using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;

namespace IAUNSportsSystem.Web.Controllers
{
    public partial class HomeController : Controller
    {
        private readonly ICompetitionService _competitionService;
        private readonly ISlideShowService _slideShowService;
        private readonly INewsService _newsService;

        public HomeController(ICompetitionService competitionService, ISlideShowService slideShowService, INewsService newsService)
        {
            _competitionService = competitionService;
            _slideShowService = slideShowService;
            _newsService = newsService;
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        public ActionResult RunningCompetitionsList()
        {
            return PartialView("_RunningCompetitionsList", _competitionService.GetRunningCompetitions());
        }

        public ActionResult SlideShow()
        {
            return PartialView("_SlideShow", _slideShowService.GetSliderImage());
        }

        public ActionResult GetRecentNews()
        {

            return PartialView(_newsService.GetRecentNewsList(5));
        }

    }
}