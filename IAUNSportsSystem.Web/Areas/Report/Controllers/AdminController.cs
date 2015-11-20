using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;
using IAUNSportsSystem.Web.Filters;
using IAUNSportsSystem.Web.Reporting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace IAUNSportsSystem.Web.Areas.Report.Controllers
{
    [SiteAuthorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IParticipationService _participationService;
        private readonly ICommonTechnicalStaffService _commonTechnicalStaffService;

        public AdminController(IParticipationService participationService, ICommonTechnicalStaffService commonTechnicalStaffService)
        {
            _participationService = participationService;
            _commonTechnicalStaffService = commonTechnicalStaffService;
        }

        public virtual async Task<ActionResult> GetCompetitionReport(int competitionId)
        {

            var reportModel = await _participationService.GetReportAsync(competitionId);

            var pdfListStream = new List<Stream>();


            foreach (var participation in reportModel)
            {
                foreach (var competitor in participation.Competitors)
                {
                    competitor.Image = Server.MapPath("~/App_Data/User_Image/" + competitor.Image);
                }

                foreach (var technicalStaff in participation.TechnicalStaves)
                {
                    technicalStaff.Image = Server.MapPath("~/App_Data/TechnicalStaff_Image/" + technicalStaff.Image);
                }

                participation.Gender = participation.Gender == "Male" ? "مردان" : "زنان";

                var headerMessage = string.Format("اعضای تیم {0}", participation.SportName);

                if (!string.IsNullOrEmpty(participation.SportCategory))
                {
                    headerMessage += string.Format(" {0}", participation.SportCategory);
                }
                if (!string.IsNullOrEmpty(participation.SportDetail))
                {
                    headerMessage += string.Format(" {0}", participation.SportDetail);
                }

                headerMessage += string.Format(" {0} {1} {2}", participation.Gender, participation.UniversityName, participation.CompetitionName);

                var xx = DocsPdfReport.CreatePdfReport(participation, headerMessage);

                pdfListStream.Add(xx.PdfStreamOutput);

            }

            var individualsSportsReport = await _participationService.GetIndividualSportCompetitonReport(competitionId);

            foreach (var participation in individualsSportsReport)
            {
                foreach (var competitor in participation.Competitors)
                {
                    competitor.Image = Server.MapPath("~/App_Data/User_Image/" + competitor.Image);
                }

                foreach (var technicalStaff in participation.TechnicalStaves)
                {
                    technicalStaff.Image = Server.MapPath("~/App_Data/TechnicalStaff_Image/" + technicalStaff.Image);
                }

                participation.Gender = participation.Gender == "Male" ? "مردان" : "زنان";

                var headerMessage = string.Format("فهرست شرکت‌کنندگان {0}", participation.SportName);

                if (!string.IsNullOrEmpty(participation.SportCategory))
                {
                    headerMessage += string.Format(" {0}", participation.SportCategory);
                }
                if (!string.IsNullOrEmpty(participation.SportDetail))
                {
                    headerMessage += string.Format(" {0}", participation.SportDetail);
                }

                headerMessage += string.Format(" {0} {1}", participation.Gender, participation.CompetitionName);

                var xx = IndividualSportsReport.CreatePdfReport(participation, headerMessage);
                pdfListStream.Add(xx.PdfStreamOutput);
            }


            var commonTechnicalStaffsReport = await _commonTechnicalStaffService.GetCommonTechnicalStaffsReportAsync(competitionId);


            foreach (var participation in commonTechnicalStaffsReport)
            {

                foreach (var technicalStaff in participation.TechnicalStaves)
                {
                    technicalStaff.Image = Server.MapPath("~/App_Data/TechnicalStaff_Image/" + technicalStaff.Image);
                }

                var headerMessage = string.Format("فهرست اعضای کادر اجرایی {0} {1}", participation.UniversityName, participation.CompetitionName);

                var xx = CommonTechnicaStaffReport.CreatePdfReport(participation, headerMessage);

                pdfListStream.Add(xx.PdfStreamOutput);
            }


            var stream = new MemoryStream();

            new MergePdfDocuments
            {
                DocumentMetadata = new DocumentMetadata { Author = "IAUN", Application = "IAUN Sport System", Keywords = "", Subject = "رشته‌های ورزشی", Title = "فهرست رشته‌های ورزشی" },
                WriterCustomizer = info =>
                {
                    info.ImportedPage.PdfWriter.CloseStream = false;
                },
                InputFileStreams = pdfListStream,

                OutputFileStream = stream,
                AttachmentsBookmarkLabel = "Attachment(s) ",
            }
            .PerformMerge();

            stream.Flush(); //Always catches me out
            stream.Position = 0; //Not sure if this is required

            return File(stream, "application/pdf", "CompetitionReport.pdf");
        }
    }
}