
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using IAUNSportsSystem.DomainClasses;
using IAUNSportsSystem.ServiceLayer;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace IAUNSportsSystem.Web.Reporting
{
    public class DocsPdfReport
    {
        public static IPdfReportData CreatePdfReport(CompetitonReportModel reportModel, string headerMessage)
        {
            var appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (HttpContext.Current != null)
            {
                appPath = HttpContext.Current.Server.MapPath("~/App_Data");
            }

            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.RightToLeft);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata
                {
                    Author = "Vahid",
                    Application = "PdfRpt",
                    Keywords = "IList Rpt.",
                    Subject = "Test Rpt",
                    Title = "Test"
                });
                doc.Compression(new CompressionSettings
                {
                    EnableCompression = true,
                    EnableFullCompression = true
                });

            })
                .DefaultFonts(fonts =>
                {
                    //fonts.Path(
                    //    System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\arial.ttf"),
                    //    System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\verdana.ttf"));
                    fonts.Path(HttpContext.Current.Server.MapPath("~/Content/Fonts/irsans.ttf"),
                           System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\Tahoma.ttf"));
                    fonts.Size(9);
                    fonts.Color(System.Drawing.Color.Black);
                })
                .PagesFooter(footer =>
                {
                    footer.DefaultFooter(DateTime.Now.ToPersianDateTime());
                    //var date = DateTime.Now.ToString("MM/dd/yyyy");
                    //footer.InlineFooter(inlineFooter =>
                    //{
                    //    inlineFooter.FooterProperties(new FooterBasicProperties
                    //    {
                    //        PdfFont = footer.PdfFont,
                    //        HorizontalAlignment = HorizontalAlignment.Center,
                    //        RunDirection = PdfRunDirection.LeftToRight,
                    //        SpacingBeforeTable = 30,
                    //        TotalPagesCountTemplateHeight = 9,
                    //        TotalPagesCountTemplateWidth = 50
                    //    });

                    //    //return inlineFooter;
                    //});
                })
                .PagesHeader(header =>
                {
                    header.CacheHeader(cache: true); // It's a default setting to improve the performance.
                    header.DefaultHeader(defaultHeader =>
                    {
                        defaultHeader.RunDirection(PdfRunDirection.RightToLeft);
                        //defaultHeader.ImagePath(System.IO.Path.Combine(appPath, "Images\\01.png"));
                        defaultHeader.Message(headerMessage);



                    });

                })
                .MainTableTemplate(template =>
                {
                    //template.BasicTemplate(BasicTemplate.BlackAndBlue1Template);
                    template.CustomTemplate(new MyTemplate());

                })
                .MainTablePreferences(table =>
                {
                    table.ColumnsWidthsType(TableColumnWidthType.Relative);
                    table.SpacingBefore(10);
                    table.SplitLate(true);

                })
                .MainTableDataSource(dataSource =>
                {
                    dataSource.StronglyTypedList(reportModel.Competitors);
                })
                .MainTableColumns(columns =>
                {
                    columns.AddColumn(column =>
                    {
                        column.PropertyName("rowNo");
                        column.IsRowNumber(true);
                        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                        column.IsVisible(true);
                        column.Order(0);
                        column.Width(1);
                        column.HeaderCell("#");
                    });

                    columns.AddColumn(column =>
                    {
                        column.PropertyName<CompetitorsReportModel>(x => x.Image);
                        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                        column.IsVisible(true);
                        column.Order(0);
                        column.Width(3);
                        column.HeaderCell("تصویر");
                        column.FixedHeight(70);
                        column.ColumnItemsTemplate(t => t.ImageFilePath(defaultImageFilePath: string.Empty,
                            fitImages: true));
                    });


                    columns.AddColumn(column =>
                    {
                        column.PropertyName<CompetitorsReportModel>(x => x.FullName);
                        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                        column.IsVisible(true);
                        column.Order(1);
                        column.Width(3.5f);
                        column.HeaderCell("نام و نام خانوادگی");
                    });

                    columns.AddColumn(column =>
                    {
                        column.PropertyName<CompetitorsReportModel>(x => x.FatherName);
                        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                        column.IsVisible(true);
                        column.Order(2);
                        column.Width(2);
                        column.HeaderCell("نام پدر");
                    });

                    columns.AddColumn(column =>
                    {
                        column.PropertyName<CompetitorsReportModel>(x => x.BirthDate);
                        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                        column.IsVisible(true);
                        column.Order(3);
                        column.Width(2.5f);
                        column.HeaderCell("تاریخ تولد");
                        column.ColumnItemsTemplate(template =>
                        {
                            template.TextBlock();
                            template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                ? string.Empty
                                : ((DateTime)obj).ToPersianDateTime(includeHourMinute: false));
                        });
                    });

                    columns.AddColumn(column =>
                    {
                        column.PropertyName<CompetitorsReportModel>(x => x.NationalCode);
                        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                        column.IsVisible(true);
                        column.Order(3);
                        column.Width(3);
                        column.HeaderCell("کد ملی");
                    });



                    columns.AddColumn(column =>
                    {
                        column.PropertyName<CompetitorsReportModel>(x => x.StudyField);
                        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                        column.IsVisible(true);
                        column.Order(3);
                        column.Width(2);
                        column.HeaderCell("رشته");
                    });

                    columns.AddColumn(column =>
                    {
                        column.PropertyName<CompetitorsReportModel>(x => x.StudentNumber);
                        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                        column.IsVisible(true);
                        column.Order(3);
                        column.Width(3);
                        column.HeaderCell("شماره دانشجویی");
                    });

                    columns.AddColumn(column =>
                    {
                        column.PropertyName<CompetitorsReportModel>(x => x.InsuranceNumber);
                        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                        column.IsVisible(true);
                        column.Order(3);
                        column.Width(3);
                        column.HeaderCell("شماره کارت بیمه ورزشی");
                    });
                })
                .MainTableEvents(events =>
                {
                    events.DataSourceIsEmpty(message: "There is no data available to display.");

                    events.DocumentClosing(e =>
                    {
                        // close the document without closing the underlying stream
                        e.PdfWriter.CloseStream = false;
                        e.PdfDoc.Close();

                        e.PdfStreamOutput.Position = 0;

                    });


                    events.MainTableAdded(args =>
                    {


                        var infoTable = new PdfGrid(numColumns: 2)
                        {
                            WidthPercentage = 100,
                            SpacingAfter = 50,
                            SpacingBefore = 50,
                            RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                            SplitLate = true,
                            SplitRows = true
                        };

                        infoTable.AddSimpleRow(

                            (cellData, properties) =>
                            {
                                cellData.Value =
                                    "مراتب فوق مورد تایید است.\n \n \n \n \n مهر و امضاء مسئول تربیت بدنی واحد";
                                properties.ShowBorder = true;
                                properties.BorderWidth = 0;
                                properties.PdfFont = events.PdfFont;
                                properties.RunDirection = PdfRunDirection.RightToLeft;

                                properties.FixedHeight = 80;
                                //properties.PaddingTop = 0;
                                //properties.PaddingRight = 25;
                                //properties.PaddingLeft = 25;
                                //properties.PaddingBottom = 0;

                                properties.HorizontalAlignment = HorizontalAlignment.Left;
                                //properties.PdfFontStyle = DocumentFontStyle.Bold;
                            },
                            (cellData, properties) =>
                            {
                                cellData.Value = "مراتب فوق مورد تایید است.\n \n \n \n \n مهر و امضاء رئیس واحد";
                                properties.ShowBorder = true;
                                properties.BorderWidth = 0;
                                properties.PdfFont = events.PdfFont;
                                properties.RunDirection = PdfRunDirection.RightToLeft;

                                properties.FixedHeight = 80;
                                //properties.PaddingTop = 0;
                                //properties.PaddingRight = 25;
                                //properties.PaddingLeft = 25;
                                //properties.PaddingBottom = 0;

                                properties.HorizontalAlignment = HorizontalAlignment.Left;
                                //properties.PdfFontStyle= DocumentFontStyle.Bold;

                            }
                            );


                        //args.Table.ad


                        var techTitleTable = new PdfGrid(numColumns: 2)
                        {
                            WidthPercentage = 100,
                            SpacingAfter = 20,
                            SpacingBefore = 20,
                            RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                        };

                        techTitleTable.AddSimpleRow(

                            (cellData, properties) =>
                            {
                                cellData.Value =
                                    "فهرست اعضای کادر فنی";
                                properties.ShowBorder = true;
                                properties.BorderWidth = 0;
                                properties.PdfFont = events.PdfFont;
                                properties.RunDirection = PdfRunDirection.RightToLeft;

                                properties.FixedHeight = 20;
                                //properties.PaddingTop = 0;
                                //properties.PaddingRight = 25;
                                //properties.PaddingLeft = 25;
                                //properties.PaddingBottom = 0;

                                properties.HorizontalAlignment = HorizontalAlignment.Left;
                                //properties.PdfFontStyle = DocumentFontStyle.Bold;
                            },
                            (cellData, properties) =>
                            {
                                cellData.Value = "";
                                properties.ShowBorder = true;
                                properties.BorderWidth = 0;
                                properties.PdfFont = events.PdfFont;
                                properties.RunDirection = PdfRunDirection.RightToLeft;

                                properties.FixedHeight = 20;
                                //properties.PaddingTop = 0;
                                //properties.PaddingRight = 25;
                                //properties.PaddingLeft = 25;
                                //properties.PaddingBottom = 0;

                                properties.HorizontalAlignment = HorizontalAlignment.Left;
                                //properties.PdfFontStyle= DocumentFontStyle.Bold;

                            }
                            );



                        var table = new PdfGrid(6)
                        {
                            WidthPercentage = 100,
                            SpacingAfter = 50,
                            SpacingBefore = 50,
                            RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                            ExtendLastRow = false,
                            HeaderRows = 1,
                            SplitLate = true,
                            SplitRows = true,
                        };


                        var borderColor = new BaseColor(ColorTranslator.FromHtml("#999999").ToArgb());

                        var oddRowColor = new BaseColor(ColorTranslator.FromHtml("#CCCCCC").ToArgb());

                        //table.SetExtendLastRow(false, false);

                        table.AddSimpleRow(
                            (cellData, cellProperties) =>
                            {
                                cellProperties.CellPadding = 3;
                                cellData.Value = "#";
                                //cellProperties.PdfFont = PdfRptFont;
                                cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                                cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
                                cellProperties.ShowBorder = true;
                                cellProperties.BorderWidth = 0;
                                cellProperties.PdfFont = events.PdfFont;
                                cellProperties.RunDirection = PdfRunDirection.RightToLeft;
                                cellProperties.BorderColor = borderColor;
                            },
                            (cellData, cellProperties) =>
                            {
                                cellData.Value = "تصویر";
                                //cellProperties.PdfFont = PdfRptFont;
                                cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                                cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
                                cellProperties.ShowBorder = true;
                                cellProperties.BorderWidth = 0;
                                cellProperties.PdfFont = events.PdfFont;
                                cellProperties.RunDirection = PdfRunDirection.RightToLeft;
                                cellProperties.BorderColor = borderColor;
                            },
                            (cellData, cellProperties) =>
                            {
                                cellData.Value = "نام و نام خانوادگی";
                                //cellProperties.PdfFont = PdfRptFont;
                                cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                                cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
                                cellProperties.ShowBorder = true;
                                cellProperties.BorderWidth = 0;
                                cellProperties.PdfFont = events.PdfFont;
                                cellProperties.RunDirection = PdfRunDirection.RightToLeft;
                                cellProperties.BorderColor = borderColor;
                            },
                            (cellData, cellProperties) =>
                            {
                                cellData.Value = "نام پدر";
                                //cellProperties.PdfFont = PdfRptFont;
                                cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                                cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
                                cellProperties.ShowBorder = true;
                                cellProperties.BorderWidth = 0;
                                cellProperties.PdfFont = events.PdfFont;
                                cellProperties.RunDirection = PdfRunDirection.RightToLeft;
                                cellProperties.BorderColor = borderColor;
                            },
                            (cellData, cellProperties) =>
                            {
                                cellData.Value = "کد ملی";
                                //cellProperties.PdfFont = PdfRptFont;
                                cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                                cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
                                cellProperties.ShowBorder = true;
                                cellProperties.BorderWidth = 0;
                                cellProperties.PdfFont = events.PdfFont;
                                cellProperties.RunDirection = PdfRunDirection.RightToLeft;
                                cellProperties.BorderColor = borderColor;
                            },
                            (cellData, cellProperties) =>
                            {
                                cellData.Value = "سمت";
                                cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                                cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
                                cellProperties.ShowBorder = true;
                                cellProperties.BorderWidth = 0;
                                cellProperties.PdfFont = events.PdfFont;
                                cellProperties.RunDirection = PdfRunDirection.RightToLeft;
                                cellProperties.BorderColor = borderColor;
                            }
                            );

                        var index = 1;

                        foreach (var technicalStaff in reportModel.TechnicalStaves)
                        {
                            var staff = technicalStaff;
                            var staff1 = technicalStaff;
                            var technicalStaff1 = technicalStaff;
                            bool isOdd = (index % 2 != 0);

                            var staff2 = technicalStaff;
                            table.AddSimpleRow(
                                (cellData, cellProperties) =>
                                {
                                    cellData.Value = index.ToString();
                                    index++;
                                    //cellProperties.PdfFont = PdfRptFont;
                                    cellProperties.PdfFontStyle = DocumentFontStyle.Normal;
                                    cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
                                    cellProperties.ShowBorder = true;
                                    cellProperties.BorderWidth = 0;
                                    cellProperties.PdfFont = events.PdfFont;
                                    cellProperties.RunDirection = PdfRunDirection.RightToLeft;
                                    cellProperties.BorderColor = borderColor;
                                    if (isOdd) cellProperties.BackgroundColor = oddRowColor;
                                },
                                (cellData, cellProperties) =>
                                {
                                    cellData.CellTemplate = new ImageFilePathField(defaultImageFilePath: string.Empty,
                            fitImages: true);
                                    cellData.Value = staff2.Image;
                                    cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
                                    cellProperties.PdfFontStyle = DocumentFontStyle.None;
                                    cellProperties.ShowBorder = true;
                                    cellProperties.BorderWidth = 0;
                                    cellProperties.PdfFont = events.PdfFont;
                                    cellProperties.RunDirection = PdfRunDirection.RightToLeft;
                                    cellProperties.FixedHeight = 70;
                                    cellProperties.CellPadding = 0;
                                    cellProperties.BorderColor = borderColor;
                                    if (isOdd) cellProperties.BackgroundColor = oddRowColor;
                                },
                            (cellData, cellProperties) =>
                            {
                                cellData.Value = staff1.FullName;
                                //cellProperties.PdfFont = PdfRptFont;
                                cellProperties.PdfFontStyle = DocumentFontStyle.Normal;
                                cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
                                cellProperties.ShowBorder = true;
                                cellProperties.BorderWidth = 0;
                                cellProperties.PdfFont = events.PdfFont;
                                cellProperties.RunDirection = PdfRunDirection.RightToLeft;
                                cellProperties.BorderColor = borderColor;
                                if (isOdd) cellProperties.BackgroundColor = oddRowColor;

                            },
                            (cellData, cellProperties) =>
                            {
                                cellData.Value = technicalStaff1.FatherName;
                                //cellProperties.PdfFont = PdfRptFont;
                                cellProperties.PdfFontStyle = DocumentFontStyle.Normal;
                                cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
                                cellProperties.ShowBorder = true;
                                cellProperties.BorderWidth = 0;
                                cellProperties.PdfFont = events.PdfFont;
                                cellProperties.RunDirection = PdfRunDirection.RightToLeft;
                                cellProperties.BorderColor = borderColor;
                                if (isOdd) cellProperties.BackgroundColor = oddRowColor;
                            },
                            (cellData, cellProperties) =>
                            {
                                cellData.Value = staff.NationalCode;
                                //cellProperties.PdfFont = PdfRptFont;
                                cellProperties.PdfFontStyle = DocumentFontStyle.Normal;
                                cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
                                cellProperties.ShowBorder = true;
                                cellProperties.BorderWidth = 0;
                                cellProperties.PdfFont = events.PdfFont;
                                cellProperties.RunDirection = PdfRunDirection.RightToLeft;
                                cellProperties.BorderColor = borderColor;
                                if (isOdd) cellProperties.BackgroundColor = oddRowColor;
                            },
                            (cellData, cellProperties) =>
                            {
                                cellData.Value = staff.Role;
                                //cellProperties.PdfFont = PdfRptFont;
                                cellProperties.PdfFontStyle = DocumentFontStyle.Normal;
                                cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
                                cellProperties.ShowBorder = true;
                                cellProperties.BorderWidth = 0;
                                cellProperties.PdfFont = events.PdfFont;
                                cellProperties.RunDirection = PdfRunDirection.RightToLeft;
                                cellProperties.BorderColor = borderColor;
                                if (isOdd) cellProperties.BackgroundColor = oddRowColor;
                            }
                            );
                        }


                        techTitleTable.AddBorderToTable(borderColor: new BaseColor(ColorTranslator.FromHtml("#999999").ToArgb()), spacingBefore: 5f);


                        table.AddBorderToTable(borderColor: new BaseColor(ColorTranslator.FromHtml("#999999").ToArgb()), spacingBefore: 5f);

                        table.SetExtendLastRow(false, false);

                        int[] firstTablecellwidth = { 13, 15, 12, 20, 12, 5 };

                        table.SetWidths(firstTablecellwidth);

                        infoTable.SetExtendLastRow(false, false);


                        args.PdfDoc.Add(table);
                        //args.PdfDoc.Add(techTitleTable);
                        args.PdfDoc.Add(infoTable);


                    });


                })
                .Export(export =>
                {
                    //export.ToExcel();
                })
                .Generate(
                    data => data.AsPdfStream(new MemoryStream())
                //data.AsPdfFile(string.Format("{0}\\Pdf\\EFSample-{1}.pdf", appPath, Guid.NewGuid().ToString("N")))
                        );
            // data.AsPdfFile(string.Format("{0}\\Pdf\\EFSample-{1}.pdf", appPath, Guid.NewGuid().ToString("N"))));
        }





        public class MyTemplate : ITableTemplate
        {

            public BaseColor AlternatingRowBackgroundColor
            {
                get { return new BaseColor(ColorTranslator.FromHtml("#CCCCCC").ToArgb()); }
            }

            public BaseColor AlternatingRowFontColor
            {
                get { return new BaseColor(Color.Black.ToArgb()); }
            }

            public BaseColor CellBorderColor
            {
                get { return new BaseColor(ColorTranslator.FromHtml("#999999").ToArgb()); }
            }

            public IList<BaseColor> HeaderBackgroundColor
            {
                get { return new List<BaseColor> { new BaseColor(Color.White.ToArgb()) }; }
            }

            public BaseColor HeaderFontColor
            {
                get { return new BaseColor(Color.Black.ToArgb()); }
            }

            public HorizontalAlignment HeaderHorizontalAlignment
            {
                get { return HorizontalAlignment.Center; }
            }

            public IList<BaseColor> PageSummaryRowBackgroundColor
            {
                get
                {
                    return new List<BaseColor>
                    {
                        new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()),
                        new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())
                    };
                }
            }

            public BaseColor PageSummaryRowFontColor
            {
                get { return new BaseColor(Color.Black.ToArgb()); }
            }

            public IList<BaseColor> PreviousPageSummaryRowBackgroundColor
            {
                get { throw new NotImplementedException(); }
            }

            public BaseColor PreviousPageSummaryRowFontColor
            {
                get { throw new NotImplementedException(); }
            }

            public BaseColor RowBackgroundColor
            {
                get { return new BaseColor(Color.White.ToArgb()); }
            }

            public BaseColor RowFontColor
            {
                get { return new BaseColor(Color.Black.ToArgb()); }
            }

            public bool ShowGridLines
            {
                get { return true; }
            }

            public IList<BaseColor> SummaryRowBackgroundColor
            {
                get
                {
                    return new List<BaseColor>
                    {
                        new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()),
                        new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())
                    };
                }
            }

            public BaseColor SummaryRowFontColor
            {
                get { return new BaseColor(Color.Black.ToArgb()); }
            }
        }
    }
}





