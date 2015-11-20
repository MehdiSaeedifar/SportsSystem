using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using IAUNSportsSystem.Web.PersianCaptcha;

namespace IAUNSportsSystem.Web.Controllers
{
    public class CaptchaController : Controller
    {
        [NoBrowserCache]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "None")]
        public CaptchaImageResult Index(string rndDate)
        {
            return new CaptchaImageResult();
        }
    }
}