using System.Text.Json.Nodes;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using PdfSharp.Pdf.IO;
using SchoolSecuritySystem.Core.Entities;
using SchoolSecuritySystem.Core.Interfaces.Repositories;

namespace SchoolSecuritySystem.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly IPdfPasswordRepository _pdfPasswordRepo;

        public ReportService(IPdfPasswordRepository pdfPasswordRepo)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            _pdfPasswordRepo = pdfPasswordRepo;
        }

        public async Task<byte[]> GenerateReportAsync(JsonNode jsonContent, submission_dispatch SD, string webRootPath, DateTime printTime)
        {
            string headerImgPath = Path.Combine(webRootPath, "images", "header.png");

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.MarginHorizontal(1.5f, Unit.Centimetre);
                    page.MarginTop(0.8f, Unit.Centimetre);
                    page.MarginBottom(0.2f, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Microsoft JhengHei"));

                    // ==========================================
                    // 內容區塊 (Content)
                    // ==========================================
                    page.Content().Column(col =>
                    {
                        if (File.Exists(headerImgPath))
                        {
                            col.Item().AlignCenter().Image(headerImgPath);
                        }
                        else
                        {
                            col.Item().Height(1.5f, Unit.Centimetre).Column(c =>
                            {
                                c.Item().Text("教育部校園安全暨災害防救通報處理中心").FontSize(18).Bold().FontColor(Colors.Blue.Darken2).AlignCenter();
                                
                            });
                        }

                        col.Item().PaddingTop(5).PaddingBottom(5).Text("※本件為密件，請妥慎保管資料，恪遵保密規定。").FontSize(9).FontColor(Colors.Red.Medium).AlignRight();

                        col.Spacing(0);

                        BuildBasicInfoTable(col, jsonContent, SD);
                        BuildPersonsTable(col, jsonContent);
                        BuildDetailsTable(col, jsonContent);
                        BuildSignaturesTable(col, SD);
                    });

                    // ==========================================
                    // 頁尾區塊 (Footer) - 每頁皆會出現
                    // ==========================================
                    page.Footer().Height(2.0f, Unit.Centimetre).Column(footerCol =>
                    {
                        footerCol.Item().Height(1.5f, Unit.Centimetre); // 手寫留白

                        footerCol.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"列印時間：{printTime:yyyy/MM/dd HH:mm}").FontSize(8).FontColor(Colors.Grey.Medium);
                            row.RelativeItem().AlignRight().DefaultTextStyle(style => style.FontSize(8).FontColor(Colors.Grey.Medium))
                               .Text(x => {
                                   x.Span("第 "); x.CurrentPageNumber(); x.Span(" 頁 / 共 "); x.TotalPages(); x.Span(" 頁");
                               });
                        });
                    });
                });
            });

            // ==========================================
            // 🌟 從資料庫動態取得 PDF 密碼
            // ==========================================
            var passwordHistory = await _pdfPasswordRepo.GetHistoryAsync(1);

            // 如果資料庫有密碼則使用，若無則預設為空字串 (不加密) 或您可以改回 "123" 作為預設防護
            string pdfPassword = passwordHistory.FirstOrDefault()?.password ?? "";

            // ==========================================
            // 輸出 PDF 並套用 PdfSharp 保全設定
            // ==========================================
            byte[] rawPdfBytes = document.GeneratePdf();

            using (var inputStream = new MemoryStream(rawPdfBytes))
            using (var outputStream = new MemoryStream())
            {
                var pdfDocument = PdfReader.Open(inputStream, PdfDocumentOpenMode.Modify);
                var securitySettings = pdfDocument.SecuritySettings;

                securitySettings.UserPassword = pdfPassword;
                securitySettings.OwnerPassword = Guid.NewGuid().ToString();
                securitySettings.PermitFullQualityPrint = true;
                securitySettings.PermitExtractContent = false;
                securitySettings.PermitModifyDocument = false;
                securitySettings.PermitAssembleDocument = false;
                securitySettings.PermitAnnotations = false;
                securitySettings.PermitFormsFill = true;

                pdfDocument.Save(outputStream);
                return outputStream.ToArray();
            }
        }

        // ==============================================
        // 共用樣式定義 (Shared Styles)
        // ==============================================
        private static IContainer LabelCell(IContainer c) => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten4).AlignMiddle().AlignCenter().Padding(4).DefaultTextStyle(x => x.Bold());
        private static IContainer ValueCell(IContainer c) => c.Border(1).BorderColor(Colors.Black).AlignMiddle().Padding(4);
        private static IContainer LargeLabelCell(IContainer c) => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten4).PaddingTop(10).DefaultTextStyle(x => x.Bold());
        private static IContainer LargeValueCell(IContainer c) => c.Border(1).BorderColor(Colors.Black).AlignTop().Padding(4);

        // ==============================================
        // 模組 1: 基本資料
        // ==============================================
        private void BuildBasicInfoTable(ColumnDescriptor col, JsonNode jsonContent, submission_dispatch SD)
        {
            var basicNode = jsonContent["basic"];
            var detailsNode = jsonContent["details"];

            string dispatchDepts = (SD.dispatch_selects != null && SD.dispatch_selects.Any())
                ? string.Join("、", SD.dispatch_selects.Select(s => s.department?.name).Where(n => !string.IsNullOrEmpty(n)))
                : "尚未選擇分送單位";

            col.Item().Table(table =>
            {
                table.ColumnsDefinition(c => { c.ConstantColumn(85); c.RelativeColumn(); });

                table.Cell().Element(LabelCell).Text("分送單位");
                table.Cell().Element(ValueCell).Text(dispatchDepts);

                table.Cell().Element(LabelCell).Text("事件序號");
                table.Cell().Element(ValueCell).Text(jsonContent["trace_code"]?.ToString() ?? "無資料");

                table.Cell().Element(LabelCell).Text("事件類別");
                table.Cell().Element(ValueCell).Text(basicNode?["title"]?.ToString() ?? "無資料");

                table.Cell().Element(LabelCell).Text("發生時間");
                table.Cell().Element(ValueCell).Text(detailsNode?["incidentTime"]?.ToString()?.Replace("T", " ") ?? "無資料");

                table.Cell().Element(LabelCell).Text("知悉時間");
                table.Cell().Element(ValueCell).Text(detailsNode?["knownTime"]?.ToString()?.Replace("T", " ") ?? "無資料");

                table.Cell().Element(LabelCell).Text("發生地點");
                table.Cell().Element(ValueCell).Text(detailsNode?["incidentLocation"]?.ToString() ?? "無資料");
            });
        }

        // ==============================================
        // 模組 2: 人物清單
        // ==============================================
        private void BuildPersonsTable(ColumnDescriptor col, JsonNode jsonContent)
        {
            var personsArray = jsonContent["persons"]?.AsArray();

            col.Item().Table(table =>
            {
                table.ColumnsDefinition(c => {
                    c.ConstantColumn(35);  // 1. 性別
                    c.ConstantColumn(70);  // 2. 姓名
                    c.RelativeColumn();    // 3. 系級/處室
                    c.ConstantColumn(100); // 4. 學號/人員代號
                    c.ConstantColumn(70);  // 5. 身分別
                    c.RelativeColumn();    // 6. 備註
                });

                table.Header(header =>
                {
                    header.Cell().Element(LabelCell).Text("性別");
                    header.Cell().Element(LabelCell).Text("姓名");
                    header.Cell().Element(LabelCell).Text("系級/處室");
                    header.Cell().Element(LabelCell).Text("學號/人員代號");
                    header.Cell().Element(LabelCell).Text("身分別");
                    header.Cell().Element(LabelCell).Text("備註");
                });

                if (personsArray != null && personsArray.Count > 0)
                {
                    foreach (var p in personsArray)
                    {
                        table.Cell().Element(ValueCell).AlignCenter().Text(p?["gender"]?.ToString() ?? "");
                        table.Cell().Element(ValueCell).AlignCenter().Text(p?["name"]?.ToString() ?? "");
                        table.Cell().Element(ValueCell).AlignCenter().Text(p?["departmentOrClass"]?.ToString() ?? "");
                        table.Cell().Element(ValueCell).AlignCenter().Text(p?["id"]?.ToString() ?? "");
                        table.Cell().Element(ValueCell).AlignCenter().Text(p?["role"]?.ToString() ?? "");
                        table.Cell().Element(ValueCell).Text(p?["note"]?.ToString() ?? "");
                    }
                }
                else
                {
                    table.Cell().ColumnSpan(6).Element(ValueCell).AlignCenter().Text("無資料");
                }
            });
        }

        // ==============================================
        // 模組 3: 事件經過與處理
        // ==============================================
        private void BuildDetailsTable(ColumnDescriptor col, JsonNode jsonContent)
        {
            var basicNode = jsonContent["basic"];
            var detailsNode = jsonContent["details"];

            string eventName = basicNode?["eventName"]?.ToString() ?? "無資料";
            string process = detailsNode?["causeAndProcess"]?.ToString() ?? "無資料";
            string handlingText = detailsNode?["improvement"]?.ToString() ?? "無資料";

            col.Item().Table(table =>
            {
                table.ColumnsDefinition(c => { c.ConstantColumn(105); c.RelativeColumn(); });

                table.Cell().Element(LabelCell).Text("案  由");
                table.Cell().Element(ValueCell).Text(eventName);

                table.Cell().Element(LargeLabelCell).MinHeight(3.0f, Unit.Centimetre).AlignCenter().AlignMiddle().Text("事件原因及經過");
                table.Cell().Element(LargeValueCell).MinHeight(3.0f, Unit.Centimetre).Text(process);

                table.Cell().Element(LargeLabelCell).MinHeight(6.0f, Unit.Centimetre).AlignCenter().AlignMiddle().Text("處理情形");
                table.Cell().Element(LargeValueCell).MinHeight(6.0f, Unit.Centimetre).Text(handlingText);
            });
        }

        // ==============================================
        // 模組 4: 簽核欄位
        // ==============================================
        private void BuildSignaturesTable(ColumnDescriptor col, submission_dispatch SD)
        {
            col.Item().EnsureSpace().Table(table =>
            {
                table.ColumnsDefinition(c => { c.RelativeColumn(); c.RelativeColumn(); c.RelativeColumn(); });

                static IContainer SignatureCell(IContainer c) => c.Border(1).BorderColor(Colors.Black).MinHeight(60).Padding(5);

                table.Cell().Element(SignatureCell).Column(c =>
                {
                    c.Item().Text("通報人").FontSize(9).FontColor(Colors.Grey.Medium);
                    if (!string.IsNullOrEmpty(SD.officer_sign))
                    {
                        c.Item().AlignCenter().PaddingTop(5).Text(SD.officer_sign).FontSize(14).Bold();
                        if (SD.officer_sign_at.HasValue)
                            c.Item().AlignCenter().PaddingTop(2).Text(SD.officer_sign_at.Value.ToString("yyyy/MM/dd HH:mm")).FontSize(8).FontColor(Colors.Grey.Darken1);
                    }
                });

                table.Cell().Element(SignatureCell).Column(c =>
                {
                    c.Item().Text("單位主管").FontSize(9).FontColor(Colors.Grey.Medium);
                    if (!string.IsNullOrEmpty(SD.director_sign))
                    {
                        c.Item().AlignCenter().PaddingTop(5).Text(SD.director_sign).FontSize(14).Bold();
                        if (SD.director_sign_at.HasValue)
                            c.Item().AlignCenter().PaddingTop(2).Text(SD.director_sign_at.Value.ToString("yyyy/MM/dd HH:mm")).FontSize(8).FontColor(Colors.Grey.Darken1);
                    }
                });

                table.Cell().Element(SignatureCell).Text("校長").FontSize(9).FontColor(Colors.Grey.Medium);
            });
        }
    }
}