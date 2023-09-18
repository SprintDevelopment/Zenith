using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static OperationResultDto PrintFactor(Sale sale)
        {
            if (wordApp == null)
                wordApp = new Word.Application();

            var document = wordApp.Documents.Add();
            document.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
            document.Paragraphs.ReadingOrder = Word.WdReadingOrder.wdReadingOrderLtr;
            document.Range().Font.Name = "JetBrains Mono";
            document.Range().Font.Size = 10;

            var range = document.Range();
            range.Text = $"Customer ID: {sale.Company.CompanyId:cpi0000};Customer TRN: {sale.Company.TaxRegistrationNumber}\n" +
                $"Name: {sale.Company.Name};Tel:{sale.Company.Tel}\n" +
                $"Address: {sale.Company.Address}";

            var table = range.ConvertToTable(Separator: ";");
            table.Range.ParagraphFormat.SpaceAfter = 3;
            table.Range.ParagraphFormat.SpaceBefore = 3;
            table.AllowAutoFit = true;
            table.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitContent);
            table.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitWindow);
            table.Borders.Enable = 0;
            table.PreferredWidth = 100;
            table.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            table.Cell(3, 1).Merge(table.Cell(3, 2));

            document.Range().Paragraphs.Add();
            var rangeForItems = document.Range().Paragraphs.Add().Range;
            rangeForItems.Text = $"#;Material;Qty;Unit;Price;Total\n";
            rangeForItems.Text += sale.Items.Select((item, i) => $"{i + 1};{item.Material.Name};{item.Count};{item.SaleCountUnit};{item.UnitPrice:n2};{item.TotalPrice:n2}").Join("\n");

            var itemsTable = rangeForItems.ConvertToTable(Separator: ";");
            itemsTable.Range.ParagraphFormat.SpaceAfter = 3;
            itemsTable.Range.ParagraphFormat.SpaceBefore = 3;
            itemsTable.AllowAutoFit = true;
            itemsTable.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitContent);
            itemsTable.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitWindow);
            itemsTable.Rows[1].Range.Bold = 1;
            itemsTable.Rows[1].Shading.BackgroundPatternColor = Word.WdColor.wdColorGray25;
            itemsTable.Borders.Enable = 1;
            itemsTable.PreferredWidth = 100;
            itemsTable.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            //For #
            itemsTable.Columns[1].PreferredWidth = 4f;
            itemsTable.Columns[1].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            //For Material
            itemsTable.Columns[2].PreferredWidth = 35f;
            itemsTable.Columns[2].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            //For Qty
            itemsTable.Columns[3].PreferredWidth = 10f;
            itemsTable.Columns[3].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            //For Unit
            itemsTable.Columns[4].PreferredWidth = 15f;
            itemsTable.Columns[4].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            //For Price
            itemsTable.Columns[5].PreferredWidth = 16f;
            itemsTable.Columns[5].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            //For Total
            itemsTable.Columns[6].PreferredWidth = 20f;
            itemsTable.Columns[6].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;

            var totalRow = itemsTable.Rows.Add();
            totalRow.Alignment = Word.WdRowAlignment.wdAlignRowRight;
            totalRow.Cells[1].Merge(totalRow.Cells[2]);
            totalRow.Cells[1].Merge(totalRow.Cells[2]);
            totalRow.Cells[1].Merge(totalRow.Cells[2]);
            totalRow.Cells[1].Merge(totalRow.Cells[2]);
            totalRow.Cells[1].Range.Text = "Total";
            totalRow.Cells[1].Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            totalRow.Cells[2].Range.Text = sale.Items.Sum(i => i.TotalPrice).ToString("n2");
            totalRow.Cells[2].Range.Bold = 1;

            var deliveries = sale.Items.SelectMany(si => si.Deliveries).ToList();

            if (deliveries.Any())
            {
                document.Range().Paragraphs.Add();

                var rangeForDeliveries = document.Range().Paragraphs.Add().Range;
                rangeForDeliveries.Text = $"#;Material;Site;Machine;Qty;Date;Del. number;Del. fee\n";
                rangeForDeliveries.Text += deliveries
                    .Select((delivery, i) => $"{i + 1};{delivery.SaleItem.Material.Name};{delivery.Site.Name};{delivery.Machine.Name};{delivery.Count};{delivery.DateTime:yy-MM-dd};{delivery.DeliveryNumber};{delivery.DeliveryFee:n2}").Join("\n");

                var deliveriestable = rangeForDeliveries.ConvertToTable(Separator: ";");
                deliveriestable.Range.ParagraphFormat.SpaceAfter = 3;
                deliveriestable.Range.ParagraphFormat.SpaceBefore = 3;
                deliveriestable.AllowAutoFit = true;
                deliveriestable.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitContent);
                deliveriestable.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitWindow);
                deliveriestable.Rows[1].Range.Bold = 1;
                deliveriestable.Rows[1].Shading.BackgroundPatternColor = Word.WdColor.wdColorGray25;
                deliveriestable.Borders.Enable = 1;
                deliveriestable.PreferredWidth = 100;
                deliveriestable.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;


                //For #
                deliveriestable.Columns[1].PreferredWidth = 4f;
                deliveriestable.Columns[1].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
                //For Material
                deliveriestable.Columns[2].PreferredWidth = 16f;
                deliveriestable.Columns[2].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
                //For Site
                deliveriestable.Columns[3].PreferredWidth = 15f;
                deliveriestable.Columns[3].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
                //For Machine
                deliveriestable.Columns[4].PreferredWidth = 15f;
                deliveriestable.Columns[4].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
                //For Qty
                deliveriestable.Columns[5].PreferredWidth = 12.5f;
                deliveriestable.Columns[5].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
                //For Date
                deliveriestable.Columns[6].PreferredWidth = 12.5f;
                deliveriestable.Columns[6].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
                //For Delivery fee
                deliveriestable.Columns[7].PreferredWidth = 12.5f;
                deliveriestable.Columns[7].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
                //For Delivery number
                deliveriestable.Columns[8].PreferredWidth = 12.5f;
                deliveriestable.Columns[8].PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;


                var totalDeliveriesRow = deliveriestable.Rows.Add();
                totalDeliveriesRow.Alignment = Word.WdRowAlignment.wdAlignRowRight;
                totalDeliveriesRow.Cells[1].Merge(totalDeliveriesRow.Cells[2]);
                totalDeliveriesRow.Cells[1].Merge(totalDeliveriesRow.Cells[2]);
                totalDeliveriesRow.Cells[1].Merge(totalDeliveriesRow.Cells[2]);
                totalDeliveriesRow.Cells[1].Merge(totalDeliveriesRow.Cells[2]);
                totalDeliveriesRow.Cells[1].Merge(totalDeliveriesRow.Cells[2]);
                totalDeliveriesRow.Cells[1].Merge(totalDeliveriesRow.Cells[2]);
                totalDeliveriesRow.Cells[1].Range.Text = "Total";
                totalDeliveriesRow.Cells[1].Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                totalDeliveriesRow.Cells[2].Range.Text = deliveries.Sum(d => d.DeliveryFee).ToString("n2");
                totalDeliveriesRow.Cells[2].Range.Bold = 1;
            }

            document.ExportAsFixedFormat(@"E:\newPdfFileName.Pdf", Word.WdExportFormat.wdExportFormatPDF, true);
            //wordApp.Visible = true;
            document.Close(false);

            return null;
        }

        public static void FillMainTable(Word.Range range, Sale sale)
        {
            var rangeTable = range.Tables[1];



            //rangeTable.set_Style("Plain Table 1");
        }
    }

}

