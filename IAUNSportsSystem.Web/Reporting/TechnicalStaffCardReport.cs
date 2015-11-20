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
using Image = iTextSharp.text.Image;
using Rectangle = iTextSharp.text.Rectangle;

namespace IAUNSportsSystem.Web.Reporting
{
    public class TechnicalStaffCardReport
    {

        public static iTextSharp.text.Font GetTahoma()
        {
            const string fontName = "Iranian Sans";
            if (FontFactory.IsRegistered(fontName))
                return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            var fontPath = HttpContext.Current.Server.MapPath("~/Content/Fonts/irsans.ttf");
            FontFactory.Register(fontPath);
            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }

        public static MemoryStream Generate(TechnicalStaffCardModel technicalStaff, string competitionImagePath, string competitionName)
        {
            var userImagePath =
                HttpContext.Current.Server.MapPath(
                    "~/App_Data/TechnicalStaff_Image/" + technicalStaff.Image);

            var universityLogoPath = HttpContext.Current.Server.MapPath(
                    "~/Content/IAU_Najafabad_Branch_logo.png");

            var competitionLogoPath = HttpContext.Current.Server.MapPath(
                    "~/App_Data/Logo_Image/" + competitionImagePath);

            var fs = new MemoryStream(); //new FileStream(HttpContext.Current.Server.MapPath("~/App_Data/Pdf/aaa.pdf"), FileMode.Create);

            Document doc = new Document(new Rectangle(PageSize.A6.Rotate()));

            doc.SetMargins(18f, 18f, 15f, 2f);

            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            writer.CloseStream = false;

            doc.Open();

            PdfContentByte canvas = writer.DirectContentUnder;

            var logoImg = Image.GetInstance(competitionLogoPath);

            logoImg.SetAbsolutePosition(0, 0);

            logoImg.ScaleAbsolute(PageSize.A6.Rotate());

            PdfGState graphicsState = new PdfGState();
            graphicsState.FillOpacity = 0.2F;  // (or whatever)
            //set graphics state to pdfcontentbyte    
            canvas.SetGState(graphicsState);

            canvas.AddImage(logoImg);

            //create new graphics state and assign opacity    



            var table = new PdfPTable(3)
            {
                WidthPercentage = 100,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                ExtendLastRow = false,
            };

            var imgg = Image.GetInstance(universityLogoPath);

            var cell1 = new PdfPCell(imgg)
            {
                HorizontalAlignment = 0,
                Border = 0

            };

            imgg.ScaleAbsolute(70, 100);

            table.AddCell(cell1);


            var cell2 = new PdfPCell(new Phrase(string.Format("کارت {0}", competitionName), GetTahoma()))
            {
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                HorizontalAlignment = 1,
                Border = 0

            };

            table.AddCell(cell2);


            var imgg2 = Image.GetInstance(userImagePath);

            imgg2.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
            imgg2.BorderWidth = 1f;
            imgg2.BorderColor = new BaseColor(ColorTranslator.FromHtml("#CCCCCC").ToArgb());

            var cell3 = new PdfPCell(imgg2)
            {
                HorizontalAlignment = 2,
                Border = 0
            };

            imgg2.ScaleAbsolute(70, 100);

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
            var celll1 = new PdfPCell(new Phrase(technicalStaff.FirstName, GetTahoma()))
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

            var celll3 = new PdfPCell(new Phrase(technicalStaff.LastName, GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0

            };

            table2.AddCell(celll0);
            table2.AddCell(celll1);
            table2.AddCell(celll2);
            table2.AddCell(celll3);

            //table2.Rows.Add(new PdfPRow(new[] { celll0, cell1, cell2, cell3 }));

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


            var cellll0 = new PdfPCell(new Phrase("", GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0
            };
            var cellll1 = new PdfPCell(new Phrase("", GetTahoma()))
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

            var cellll3 = new PdfPCell(new Phrase(technicalStaff.NationalCode, GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0
            };

            table3.AddCell(cellll2);
            table3.AddCell(cellll3);
            table3.AddCell(cellll0);
            table3.AddCell(cellll1);


            //table2.Rows.Add(new PdfPRow(new[] { celll0, cell1, cell2, cell3 }));

            int[] table3cellwidth = { 20, 15, 20, 15 };

            table3.SetWidths(table3cellwidth);

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
                Border = 0
            };
            var celllll1 = new PdfPCell(new Phrase(technicalStaff.University, GetTahoma()))
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

            var celllll3 = new PdfPCell(new Phrase(string.Format("{0}-{1}", technicalStaff.Dorm, technicalStaff.DormNumber), GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0
            };

            table4.AddCell(celllll1);
            table4.AddCell(celllll0);
            table4.AddCell(celllll2);
            table4.AddCell(celllll3);

            //table2.Rows.Add(new PdfPRow(new[] { celll0, cell1, cell2, cell3 }));

            int[] table4cellwidth = { 20, 15, 0, 35 };

            table4.SetWidths(table4cellwidth);

            doc.Add(table4);



            var table5 = new PdfPTable(4)
            {
                WidthPercentage = 100,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                ExtendLastRow = false,
                SpacingBefore = 15,

            };


            var cellllll0 = new PdfPCell(new Phrase("سمت:", GetTahoma()))
            {
                RunDirection = (int)PdfRunDirection.RightToLeft,
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                Border = 0
            };

            var techRoles = "";

            foreach (var role in technicalStaff.Roles.Distinct())
            {
                techRoles += role + "-";
            }

            techRoles = techRoles.Substring(0, techRoles.Length - 1);

            var cellllll1 = new PdfPCell(new Phrase(techRoles, GetTahoma()))
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

            //table2.Rows.Add(new PdfPRow(new[] { celll0, cell1, cell2, cell3 }));

            int[] table5cellwidth = { 0, 0, 40, 10 };

            table5.SetWidths(table5cellwidth);

            doc.Add(table5);


            doc.Close();

            return fs;
        }
    }

}
