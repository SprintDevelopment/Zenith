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
using Zenith.Models.ReportModels;
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

                document.ExportAsFixedFormat(@"E:\newPdfFileName.Pdf", Word.WdExportFormat.wdExportFormatPDF, true);
                //wordApp.Visible = true;
                document.Close(false);

                //wordApp.Visible = true;

            }

            return null;
        }

        public static OperationResultDto PrintCompanyAggregateReport(ObservableCollection<CompanyAggregateReport> reportItems, bool includeCustomerTRN = false)
        {
            if (wordApp == null)
                wordApp = new Word.Application();

            var currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var templateFileFullName = Path.Combine(currentDirectory, includeCustomerTRN ? @"Factors\Statement.docx" : @"Factors\Statement-Without-TRN.docx");

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

                document.ExportAsFixedFormat(@"E:\newPdfFileName.Pdf", Word.WdExportFormat.wdExportFormatPDF, true);
                //wordApp.Visible = true;
                document.Close(false);

                //wordApp.Visible = true;

            }

            return null;
        }
    }

}

