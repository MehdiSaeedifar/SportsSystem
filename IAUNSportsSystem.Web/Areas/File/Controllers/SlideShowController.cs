using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.Web.Filters;
using IAUNSportsSystem.Web.Helpers;

namespace IAUNSportsSystem.Web.Areas.File.Controllers
{
    [SiteAuthorize(Roles = "admin")]
    public class SlideShowController : Controller
    {
        // GET: File/SlideShow
        public ActionResult Upload(HttpPostedFileBase file)
        {

            file.IsImageFile();

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var filePath = Server.MapPath("~/Content/SlideShowImages/" + fileName);


            file.SaveAs(filePath);

            return Json(fileName);
        }

        public ActionResult GetAllImages()
        {
            var filesPath = Directory.GetFiles(Server.MapPath("~/Content/SlideShowImages"));

            var files = filesPath.Select(Path.GetFileName).ToList();


            return Json(files, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string fileName)
        {
            System.IO.File.Delete(Server.MapPath("~/Content/SlideShowImages/") + fileName);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


    }
}