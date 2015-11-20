using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.Web.Filters;
using IAUNSportsSystem.Web.Helpers;
using IAUNSportsSystem.Web.Infrastructure;

namespace IAUNSportsSystem.Web.Areas.File.Controllers
{
    [SiteAuthorize(Roles = "admin,user")]
    public partial class HomeController : Controller
    {
        [HttpPost]
        [AllowUploadSpecialFilesOnly(".jpg,.gif,.png")]
        public virtual ActionResult ImageUpload(HttpPostedFileBase file)
        {
            file.IsImageFile();
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Server.MapPath("~/App_Data/tmp/" + fileName);

            file.SaveAs(filePath);
            return Json(new
            {
                url = Url.Action("Index", "Home", new { area = "File", name = fileName }),
                name = fileName
            });
        }

        [HttpPost]
        [AllowUploadSpecialFilesOnly(".jpg,.gif,.png")]
        public virtual ActionResult UploadUserImage(HttpPostedFileBase file)
        {

            file.IsImageFile();

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var filePath = Server.MapPath("~/App_Data/tmp/" + fileName);

            file.SaveAs(filePath);

            return Json(new
            {
                url = Url.Action("Index", "Home", new { area = "File", name = fileName }),
                name = fileName
            });
        }

        public virtual ActionResult Index(string name)
        {
            name = Path.GetFileName(name);
            return File(Server.MapPath("~/App_Data/tmp/" + name), MimeMapping.GetMimeMapping(name));
        }

        public ActionResult GetUserImage(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            if (Request.Browser.Browser == "IE")
            {
                var attachment = string.Format("attachment; filename=\"{0}\"", Server.UrlPathEncode(fileName));
                Response.AddHeader("Content-Disposition", attachment);
            }

            return File(Server.MapPath("~/App_Data/User_Image/" + fileName), MimeMapping.GetMimeMapping(fileName));
        }

        public ActionResult GetInsuranceImage(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            return File(Server.MapPath("~/App_Data/Insurance_Image/" + fileName), MimeMapping.GetMimeMapping(fileName));
        }

        public ActionResult GetStudentCertificateImage(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            return File(Server.MapPath("~/App_Data/Student_Certificate_Image/" + fileName), MimeMapping.GetMimeMapping(fileName));
        }

        public ActionResult GetAzmoonConfirmationImage(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            return File(Server.MapPath("~/App_Data/Azmoon_Confirmation_Image/" + fileName), MimeMapping.GetMimeMapping(fileName));
        }

        public ActionResult GetTechnicalStaffImage(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            return File(Server.MapPath("~/App_Data/TechnicalStaff_Image/" + fileName), MimeMapping.GetMimeMapping(fileName));
        }

        [SiteAuthorize(Roles = "admin")]
        [HttpPost]
        [AllowUploadSpecialFilesOnly(".jpg,.gif,.png")]
        public virtual ActionResult UploadLogoImage(HttpPostedFileBase file)
        {

            file.IsImageFile();

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var filePath = Server.MapPath("~/App_Data/tmp/" + fileName);

            file.SaveAs(filePath);

            return Json(new
            {
                url = Url.Action("Index", "Home", new { area = "File", name = fileName }),
                name = fileName

            });
        }

        public ActionResult GetLogoImage(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            return File(Server.MapPath("~/App_Data/Logo_Image/" + fileName), MimeMapping.GetMimeMapping(fileName));
        }

    }
}