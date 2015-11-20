using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using IAUNSportsSystem.ServiceLayer;
using PdfRpt;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using Font = iTextSharp.text.Font;
using Image = iTextSharp.text.Image;
using Rectangle = iTextSharp.text.Rectangle;

namespace IAUNSportsSystem.Web.Reporting
{
    public class CardReport
    {

        public static Font GetTahoma()
        {
            const string fontName = "Iranian Sans";

            if (FontFactory.IsRegistered(fontName))
                return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            var fontPath = HttpContext.Current.Server.MapPath("~/Content/Fonts/irsans.ttf");

            FontFactory.Register(fontPath);

            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }

        public static MemoryStream Generate(CompetitorCardModel competitor, string competitionImagePath, string competitionName)
        {
            var userImagePath =
                HttpContext.Current.Server.MapPath(
                    "~/App_Data/User_Image/" + competitor.Image);

            var universityLogoPath = HttpContext.Current.Server.MapPath(
                    "~/Content/IAU_Najafabad_Branch_logo.png");

            var competitionLogoPath = HttpContext.Current.Server.MapPath(
                    "~/App_Data/Logo_Image/" + competitionImagePath);

            var memoryStream = new MemoryStream();

            var pageSize = PageSize.A6.Rotate();

            Document doc = new Document(pageSize);

            doc.SetMargins(18f, 18f, 15f, 2f);


            PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);

            writer.CloseStream = false;

            doc.Open();

            PdfContentByte canvas = writer.DirectContentUnder;

            var logoImg = Image.GetInstance(competitionLogoPath);

            logoImg.SetAbsolutePosition(0, 0);

            logoImg.ScaleAbsolute(pageSize);

            PdfGState graphicsState = new PdfGState { FillOpacity = 0.2F };

            canvas.SetGState(graphicsState);

            canvas.AddImage(logoImg);


            var table = new PdfPTable(3)
            {
                WidthPercentage = 100,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                ExtendLastRow = false,
            };

            var universityLogoImage = Image.GetInstance(universityLogoPath);

            var cell1 = new PdfPCell(universityLogoImage)
            {
                HorizontalAlignment = 0,
                Border = 0
            };

            universityLogoImage.ScaleAbsolute(70, 100);

            table.AddCell(cell1);


            var cell2 = new PdfPCell(new Phrase(string.Format("کارت {0}", competitionName), GetTahoma()))
            {
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                HorizontalAlignment = 1,
                Border = 0
            };

            table.AddCell(cell2);


            var userImage = Image.GetInstance(userImagePath);

            userImage.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
            userImage.BorderWidth = 1f;
            userImage.BorderColor = new BaseColor(ColorTranslator.FromHtml("#CCCCCC").ToArgb());

            var cell3 = new PdfPCell(userImage)
            {
                HorizontalAlignment = 2,
                Border = 0
            };

            userImage.ScaleAbsolute(70, 100);

            table.AddCell(cell3);

            int[] firstTablecellwidth = { 10, 20, 10 };

            table.SetWidths(firstTablecellwidth);

            doc.Add(table);


            var table2 = new PdfPTable(4)
            {
                WidthPercentage = 100,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                ExtendLastRow = false,
                SpacingBefore = 15
            };

            var celll0 = new PdfPCell(new Phrase("نام:", GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0

            };
            var celll1 = new PdfPCell(new Phrase(competitor.FirstName, GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0

            };

            var celll2 = new PdfPCell(new Phrase("نام خانوادگی:", GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0

            };

            var celll3 = new PdfPCell(new Phrase(competitor.LastName, GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0

            };

            table2.AddCell(celll0);
            table2.AddCell(celll1);
            table2.AddCell(celll2);
            table2.AddCell(celll3);


            int[] secondTablecellwidth = { 20, 15, 20, 15 };

            table2.SetWidths(secondTablecellwidth);

            doc.Add(table2);


            var table3 = new PdfPTable(4)
            {
                WidthPercentage = 100,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                ExtendLastRow = false,
                SpacingBefore = 15,

            };


            var cellll0 = new PdfPCell(new Phrase("شماره\nدانشجویی:", GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0
            };
            var cellll1 = new PdfPCell(new Phrase(competitor.StudentNumber, GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0
            };

            var cellll2 = new PdfPCell(new Phrase("کد ملی:", GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0
            };

            var cellll3 = new PdfPCell(new Phrase(competitor.NationalCode, GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0
            };

            table3.AddCell(cellll0);
            table3.AddCell(cellll1);
            table3.AddCell(cellll2);
            table3.AddCell(cellll3);


            int[] table3Cellwidth = { 20, 15, 20, 15 };

            table3.SetWidths(table3Cellwidth);

            doc.Add(table3);


            var table4 = new PdfPTable(4)
            {
                WidthPercentage = 100,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                ExtendLastRow = false,
                SpacingBefore = 15,

            };


            var celllll0 = new PdfPCell(new Phrase("", GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0,
            };
            var celllll1 = new PdfPCell(new Phrase(competitor.University, GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0
            };

            var celllll2 = new PdfPCell(new Phrase("محل اسکان:", GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0
            };

            var celllll3 = new PdfPCell(new Phrase(string.Format("{0}-{1}", competitor.Dorm, competitor.DormNumber), GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0
            };

            table4.AddCell(celllll1);
            table4.AddCell(celllll0);
            table4.AddCell(celllll2);
            table4.AddCell(celllll3);


            int[] table4Cellwidth = { 20, 15, 0, 35 };

            table4.SetWidths(table4Cellwidth);

            doc.Add(table4);



            var table5 = new PdfPTable(4)
            {
                WidthPercentage = 100,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                ExtendLastRow = false,
                SpacingBefore = 15,

            };


            var cellllll0 = new PdfPCell(new Phrase("رشته ورزشی:", GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0
            };
            var cellllll1 = new PdfPCell(new Phrase(competitor.Sport, GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0
            };

            var cellllll2 = new PdfPCell(new Phrase("", GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0
            };

            var cellllll3 = new PdfPCell(new Phrase("", GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0
            };

            table5.AddCell(cellllll0);
            table5.AddCell(cellllll1);
            table5.AddCell(cellllll2);
            table5.AddCell(cellllll3);


            int[] table5Cellwidth = { 0, 0, 55, 15 };

            table5.SetWidths(table5Cellwidth);

            doc.Add(table5);

            doc.Close();


            return memoryStream;
        }
    }

}
