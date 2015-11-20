using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAUNSportsSystem.ServiceLayer;
using System.Threading.Tasks;
using IAUNSportsSystem.Web.Reporting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace IAUNSportsSystem.Web.Areas.Report.Controllers
{
    public class CardsController : Controller
    {
        private readonly ICompetitorService _competitorService;
        private readonly ITechnicalStaffService _techTechnicalStaffService;
        private readonly ICompetitionService _competitionService;

        public CardsController(ICompetitorService competitorService, ITechnicalStaffService technicalStaffService, ICompetitionService competitionService)
        {
            _competitorService = competitorService;
            _techTechnicalStaffService = technicalStaffService;
            _competitionService = competitionService;
        }

        // GET: Report/PrintCards
        public async Task<ActionResult> GetAllCards(int competitionId)
        {
            var userId = Convert.ToInt32(User.Identity.Name);

            var canReport = await _competitionService.CanReportCards(competitionId, userId);
            if (!canReport)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var competitors = await _competitorService.GetCompetitorsCard(competitionId, userId);

            if (competitors.Count < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var competitionLogo = await _competitionService.GetLogoImage(competitionId);

            var pdfListStream = new List<Stream>();

            foreach (var xx in competitors.Select(competitor => CardReport.Generate(competitor, competitionLogo.Logo, competitionLogo.CompetitionName)))
            {
                xx.Flush();
                xx.Position = 0;

                pdfListStream.Add(xx);
            }

            var technicalStaffs = await _techTechnicalStaffService.GetTechnicalStaffsCards(competitionId, userId);

            foreach (var xx in technicalStaffs.Select(technicalStaff => TechnicalStaffCardReport.Generate(technicalStaff, competitionLogo.Logo, competitionLogo.CompetitionName)))
            {
                xx.Flush();
                xx.Position = 0;

                pdfListStream.Add(xx);
            }

            var commonTechnicalStaffs = await _techTechnicalStaffService.GetCommonTechnicalStaffsCards(competitionId, userId);

            foreach (var xx in commonTechnicalStaffs.Select(technicalStaff => TechnicalStaffCardReport.Generate(technicalStaff, competitionLogo.Logo, competitionLogo.CompetitionName)))
            {
                xx.Flush();
                xx.Position = 0;

                pdfListStream.Add(xx);
            }

            var stream = new MemoryStream();

            new MergePdfDocuments
            {
                DocumentMetadata = new DocumentMetadata { Author = "دانشگاه آزاد اسلامی واحد نجف آباد", Application = "IAUN Sports System", Keywords = "کارت‌های شرکت کنندگان مسابقه", Subject = "کارت‌های شرکت کنندگان مسابقه", Title = "کارت‌های شرکت کنندگان مسابقه" },
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

            return File(stream, "application/pdf", "Cards.pdf");
        }
    }
}