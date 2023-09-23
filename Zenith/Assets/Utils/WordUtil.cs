using Microsoft.Identity.Client;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Dtos;
using Zenith.Models;
using Word = Microsoft.Office.Interop.Word;
namespace Zenith.Assets.Utils
{
    public class WordUtil
    {
        public static Word.Application wordApp;
        public static OperationResultDto PrintFactor(Sale sale, bool includeCustomerTRN = false)
        {
            if (wordApp == null)
                wordApp = new Word.Application();

            var currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var templateFileFullName = Path.Combine(currentDirectory, includeCustomerTRN ? @"Factors\Template.docx" : @"Factors\Template-Without-TRN.docx");

            if (File.Exists(templateFileFullName))
            {
                wordApp.Documents.Open(templateFileFullName);

                var document = wordApp.ActiveDocument;

                var companyTable = document.Tables[1];
                companyTable.Cell(1, 1).Range.Text = $"Customer Code: {sale.Company.CompanyId:CPY0000}";
                companyTable.Cell(1, 3).Range.Text = $"{sale.SaleId:INV0000}";
                companyTable.Cell(2, 1).Range.Text = sale.Company.Name;
                companyTable.Cell(2, 3).Range.Text = $"{sale.DateTime:yyyy-MMM-dd}";
                companyTable.Cell(3, 1).Range.Text = $"Tel: {sale.Company.Tel}";
                companyTable.Cell(4, 1).Range.Text = $"Fax: {sale.Company.Fax}";
                if (includeCustomerTRN)
                    companyTable.Cell(5, 1).Range.Text = $"TRN: {sale.Company.TaxRegistrationNumber}";

                var deliveriesTable = document.Tables[2];

                sale.Items.SelectMany(si => si.Deliveries)
                    .Select((d, i) =>
                    {
                        var newRow = deliveriesTable.Rows.Add();
                        newRow.Cells[1].Range.Text = $"{i + 1}";
                        newRow.Cells[2].Range.Text = d.SaleItem.Material.Name;
                        newRow.Cells[3].Range.Text = d.Site.Name;
                        newRow.Cells[4].Range.Text = "Trip";
                        newRow.Cells[5].Range.Text = d.Machine.Name;
                        newRow.Cells[6].Range.Text = d.DeliveryNumber;
                        newRow.Cells[7].Range.Text = $"{d.Count:n2} (m)";
                        newRow.Cells[8].Range.Text = $"{(d.DeliveryFee + d.SaleItem.TotalPrice):n2}";

                        return d;
                    }).ToList();

                var totalPrice = sale.Items.Sum(si => si.TotalPrice + si.Deliveries.Sum(d => d.DeliveryFee));

                var rowsContents = new Tuple<string, string>[]
                {
                    new Tuple<string, string>("Sub Total", $"{totalPrice:n2}"),
                    new Tuple<string, string>("VAT 5%", $"{totalPrice * 0.05:n2}"),
                    new Tuple<string, string>("Total", $"{totalPrice * 1.05:n2}")
                };

                for (int i = 0; i < (includeCustomerTRN ? 3 : 1); i++)
                {
                    var newRow = deliveriesTable.Rows.Add();

                    if (i == 0)
                    {
                        newRow.Cells[1].Merge(newRow.Cells[2]);
                        newRow.Cells[1].Merge(newRow.Cells[2]);
                        newRow.Cells[1].Merge(newRow.Cells[2]);
                        newRow.Cells[1].Merge(newRow.Cells[2]);
                        newRow.Cells[1].Merge(newRow.Cells[2]);
                        newRow.Cells[1].Merge(newRow.Cells[2]);
                    }

                    newRow.Cells[1].Range.Text = rowsContents[i].Item1;
                    newRow.Cells[2].Range.Text = rowsContents[i].Item2;
                    newRow.Cells[1].Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                }
                //var subTotalRow = deliveriesTable.Rows.Add();
                //var taxRow = deliveriesTable.Rows.Add();
                //var totalRow = deliveriesTable.Rows.Add();

                //subTotalRow.Cells[1].Merge(subTotalRow.Cells[2]);
                //subTotalRow.Cells[1].Merge(subTotalRow.Cells[2]);
                //subTotalRow.Cells[1].Merge(subTotalRow.Cells[2]);
                //subTotalRow.Cells[1].Merge(subTotalRow.Cells[2]);
                //subTotalRow.Cells[1].Merge(subTotalRow.Cells[2]);
                //subTotalRow.Cells[1].Borders.Enable = 0;
                //subTotalRow.Cells[2].Range.Text = "Sub total";

                document.ExportAsFixedFormat(@"E:\newPdfFileName.Pdf", Word.WdExportFormat.wdExportFormatPDF, true);
                //wordApp.Visible = true;
                document.Close(false);

                //wordApp.Visible = true;

            }
            //var document = wordApp.Documents.Add();
            //document.PageSetup.LeftMargin = 20f;
            //document.PageSetup.RightMargin = 20f;
            //document.Paragraphs.ReadingOrder = Word.WdReadingOrder.wdReadingOrderLtr;
            //document.Range().Font.Name = "JetBrains Mono";
            //document.Range().Font.Size = 10;

            //var headerRange = document.Sections[1].Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
            //headerRange.Text = "Al Khan Transport EST\nMobile:+97506323549\nPO Box:359" + (includeCustomerTRN ? "\nTRN:100270851700003" : "");
            //headerRange.Paragraphs.ReadingOrder = Word.WdReadingOrder.wdReadingOrderLtr;

            //var headerTable = headerRange.ConvertToTable(Separator: ";");
            //headerTable.Range.ParagraphFormat.SpaceAfter = 3;
            //headerTable.Range.ParagraphFormat.SpaceBefore = 3;
            //headerTable.AllowAutoFit = true;
            //headerTable.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitContent);
            //headerTable.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitWindow);
            //headerTable.Borders.Enable = 0;

            //headerTable.Rows[1].Range.Font.Size = 14;

            //var range = document.Range();
            //var customerTRN = includeCustomerTRN ? $"Customer TRN: {sale.Company.TaxRegistrationNumber}" : " ";
            //range.Text = $"Customer ID: {sale.Company.CompanyId:cpi0000};{customerTRN}\n" +
            //    $"Name: {sale.Company.Name};Tel:{sale.Company.Tel}\n" +
            //    $"Address: {sale.Company.Address}";


            //var table = range.ConvertToTable(Separator: ";");
            //table.Range.ParagraphFormat.SpaceAfter = 3;
            //table.Range.ParagraphFormat.SpaceBefore = 3;
            //table.AllowAutoFit = true;
            //table.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitContent);
            //table.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitWindow);
            //table.Borders.Enable = 0;
            //table.PreferredWidth = 100;
            //table.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            //table.Cell(3, 1).Merge(table.Cell(3, 2));

            //document.Range().Paragraphs.Add();
            //var rangeForItems = document.Range().Paragraphs.Add().Range;
            //rangeForItems.Text = $"#;Material;Qty;Unit;Price;Total\n";
            //rangeForItems.Text += sale.Items.Select((item, i) => $"{i + 1};{item.Material.Name};{item.Count};{item.SaleCountUnit};{item.UnitPrice:n2};{item.TotalPrice:n2}").Join("\n");

            //var itemsTable = rangeForItems.ConvertToTable(Separator: ";");
            //itemsTable.Range.ParagraphFormat.SpaceAfter = 3;
            //itemsTable.Range.ParagraphFormat.SpaceBefore = 3;
            //itemsTable.AllowAutoFit = true;
            //itemsTable.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitContent);
            //itemsTable.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitWindow);
            //itemsTable.Rows[1].Range.Bold = 1;
            //itemsTable.Rows[1].Shading.BackgroundPatternColor = Word.WdColor.wdColorGray25;
            //itemsTable.Borders.Enable = 1;
            //itemsTable.PreferredWidth = 100;
            //itemsTable.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            ////For #
            //itemsTable.Columns[1].PreferredWidth = 4f;
            //itemsTable.Columns[1].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            ////For Material
            //itemsTable.Columns[2].PreferredWidth = 35f;
            //itemsTable.Columns[2].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            ////For Qty
            //itemsTable.Columns[3].PreferredWidth = 10f;
            //itemsTable.Columns[3].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            ////For Unit
            //itemsTable.Columns[4].PreferredWidth = 15f;
            //itemsTable.Columns[4].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            ////For Price
            //itemsTable.Columns[5].PreferredWidth = 16f;
            //itemsTable.Columns[5].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            ////For Total
            //itemsTable.Columns[6].PreferredWidth = 20f;
            //itemsTable.Columns[6].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;

            //var totalRow = itemsTable.Rows.Add();
            //totalRow.Alignment = Word.WdRowAlignment.wdAlignRowRight;
            //totalRow.Cells[1].Merge(totalRow.Cells[2]);
            //totalRow.Cells[1].Merge(totalRow.Cells[2]);
            //totalRow.Cells[1].Merge(totalRow.Cells[2]);
            //totalRow.Cells[1].Merge(totalRow.Cells[2]);
            //totalRow.Cells[1].Range.Text = "Total";
            //totalRow.Cells[1].Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            //totalRow.Cells[2].Range.Text = sale.Items.Sum(i => i.TotalPrice).ToString("n2");
            //totalRow.Cells[2].Range.Bold = 1;

            //var deliveries = sale.Items.SelectMany(si => si.Deliveries).ToList();

            //if (deliveries.Any())
            //{
            //    document.Range().Paragraphs.Add();

            //    var rangeForDeliveries = document.Range().Paragraphs.Add().Range;
            //    rangeForDeliveries.Text = $"#;Material;Site;Machine;Qty;Date;Del. number;Del. fee\n";
            //    rangeForDeliveries.Text += deliveries
            //        .Select((delivery, i) => $"{i + 1};{delivery.SaleItem.Material.Name};{delivery.Site.Name};{delivery.Machine.Name};{delivery.Count};{delivery.DateTime:yy-MM-dd};{delivery.DeliveryNumber};{delivery.DeliveryFee:n2}").Join("\n");

            //    var deliveriestable = rangeForDeliveries.ConvertToTable(Separator: ";");
            //    deliveriestable.Range.ParagraphFormat.SpaceAfter = 3;
            //    deliveriestable.Range.ParagraphFormat.SpaceBefore = 3;
            //    deliveriestable.AllowAutoFit = true;
            //    deliveriestable.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitContent);
            //    deliveriestable.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitWindow);
            //    deliveriestable.Rows[1].Range.Bold = 1;
            //    deliveriestable.Rows[1].Shading.BackgroundPatternColor = Word.WdColor.wdColorGray25;
            //    deliveriestable.Borders.Enable = 1;
            //    deliveriestable.PreferredWidth = 100;
            //    deliveriestable.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;


            //    //For #
            //    deliveriestable.Columns[1].PreferredWidth = 4f;
            //    deliveriestable.Columns[1].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            //    //For Material
            //    deliveriestable.Columns[2].PreferredWidth = 16f;
            //    deliveriestable.Columns[2].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            //    //For Site
            //    deliveriestable.Columns[3].PreferredWidth = 15f;
            //    deliveriestable.Columns[3].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            //    //For Machine
            //    deliveriestable.Columns[4].PreferredWidth = 15f;
            //    deliveriestable.Columns[4].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            //    //For Qty
            //    deliveriestable.Columns[5].PreferredWidth = 12.5f;
            //    deliveriestable.Columns[5].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            //    //For Date
            //    deliveriestable.Columns[6].PreferredWidth = 12.5f;
            //    deliveriestable.Columns[6].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            //    //For Delivery fee
            //    deliveriestable.Columns[7].PreferredWidth = 12.5f;
            //    deliveriestable.Columns[7].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            //    //For Delivery number
            //    deliveriestable.Columns[8].PreferredWidth = 12.5f;
            //    deliveriestable.Columns[8].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;


            //    var totalDeliveriesRow = deliveriestable.Rows.Add();
            //    totalDeliveriesRow.Alignment = Word.WdRowAlignment.wdAlignRowRight;
            //    totalDeliveriesRow.Cells[1].Merge(totalDeliveriesRow.Cells[2]);
            //    totalDeliveriesRow.Cells[1].Merge(totalDeliveriesRow.Cells[2]);
            //    totalDeliveriesRow.Cells[1].Merge(totalDeliveriesRow.Cells[2]);
            //    totalDeliveriesRow.Cells[1].Merge(totalDeliveriesRow.Cells[2]);
            //    totalDeliveriesRow.Cells[1].Merge(totalDeliveriesRow.Cells[2]);
            //    totalDeliveriesRow.Cells[1].Merge(totalDeliveriesRow.Cells[2]);
            //    totalDeliveriesRow.Cells[1].Range.Text = "Total";
            //    totalDeliveriesRow.Cells[1].Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            //    totalDeliveriesRow.Cells[2].Range.Text = deliveries.Sum(d => d.DeliveryFee).ToString("n2");
            //    totalDeliveriesRow.Cells[2].Range.Bold = 1;
            //}

            //document.ExportAsFixedFormat(@"D:\newPdfFileName.Pdf", Word.WdExportFormat.wdExportFormatPDF, true);
            ////wordApp.Visible = true;
            //document.Close(false);

            return null;
        }

        public static void FillMainTable(Word.Range range, Sale sale)
        {
            var rangeTable = range.Tables[1];



            //rangeTable.set_Style("Plain Table 1");
        }
    }

}

