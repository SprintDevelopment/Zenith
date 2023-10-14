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
using Zenith.Repositories;
using Word = Microsoft.Office.Interop.Word;
namespace Zenith.Assets.Utils
{
    public class WordUtil
    {
        public static Word.Application wordApp;
        public static OperationResultDto PrintFactor(bool includeCustomerTRN = false, params Sale[] sales)
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
                companyTable.Cell(1, 1).Range.Text = $"Customer Code: {sales[0].Company.CompanyId:CPY0000}";
                companyTable.Cell(1, 3).Range.Text = sales.Count() == 1 ? $"{sales[0].SaleId:INV0000}" : "";
                companyTable.Cell(2, 1).Range.Text = sales[0].Company.Name;
                companyTable.Cell(2, 3).Range.Text = sales.Count() == 1 ? $"{sales[0].DateTime:yyyy-MMM-dd}" : $"{sales[0].DateTime:yyyy-MMM}";
                companyTable.Cell(3, 1).Range.Text = $"Tel: {sales[0].Company.Tel}";
                companyTable.Cell(4, 1).Range.Text = $"Fax: {sales[0].Company.Fax}";
                if (includeCustomerTRN)
                    companyTable.Cell(5, 1).Range.Text = $"TRN: {sales[0].Company.TaxRegistrationNumber}";

                var deliveriesTable = document.Tables[2];

                sales.SelectMany(s => s.Items).Where(si => si.Deliveries.Any()).SelectMany(si => si.Deliveries)
                    .GroupBy(d => new { d.SiteId, d.DeliveryNumber })
                    .Select((deliveries, i) =>
                    {
                        var deliveriesCount = deliveries.Count();

                        var newRow = deliveriesTable.Rows.Add();
                        newRow.Cells[1].Range.Text = $"{i + 1}";
                        newRow.Cells[2].Range.Text = deliveries.First().SaleItem.Material.Name;
                        newRow.Cells[3].Range.Text = deliveries.First().Site.Name;
                        newRow.Cells[4].Range.Text = deliveriesCount == 1 ? "1 Trip" : $"{deliveriesCount} Trips";
                        newRow.Cells[5].Range.Text = deliveriesCount == 1 ? deliveries.First().Machine.Name : "";
                        newRow.Cells[6].Range.Text = deliveries.First().DeliveryNumber;
                        newRow.Cells[7].Range.Text = $"{deliveries.First().DateTime:yyyy-MM-dd}";
                        newRow.Cells[8].Range.Text = $"{deliveries.Sum(d => d.Count):n2} (m)";
                        newRow.Cells[9].Range.Text = $"{(deliveries.Sum(d => d.DeliveryFee) + deliveries.First().SaleItem.TotalPrice):n2}";

                        return deliveries;
                    }).ToList();

                var totalPrice = sales.Sum(s => s.Items.Where(si => si.Deliveries.Any()).Sum(si => si.TotalPrice + si.Deliveries.Sum(d => d.DeliveryFee)));

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

        public static OperationResultDto PrintSalaryReceipt(SalaryPayment payment)
        {
            if (wordApp == null)
                wordApp = new Word.Application();

            var currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var templateFileFullName = Path.Combine(currentDirectory, @"Factors\SalaryTemplate.docx");

            if (File.Exists(templateFileFullName))
            {
                wordApp.Documents.Open(templateFileFullName);

                var document = wordApp.ActiveDocument;

                var companyTable = document.Tables[1];
                companyTable.Cell(1, 1).Range.Text = $"Personnel ID: {payment.Personnel.PersonId:PID0000}";
                companyTable.Cell(1, 3).Range.Text = $"{payment.SalaryPaymentId:PMT0000}";
                companyTable.Cell(2, 1).Range.Text = payment.Personnel.FullName;
                companyTable.Cell(2, 3).Range.Text = $"{payment.DateTime:yyyy-MMM-dd}";
                companyTable.Cell(3, 1).Range.Text = $"Amount: {payment.PaidValue:n2}";

                document.ExportAsFixedFormat(@"E:\newPdfFileName.Pdf", Word.WdExportFormat.wdExportFormatPDF, true);
                //wordApp.Visible = true;
                document.Close(false);

                //wordApp.Visible = true;

            }

            return null;
        }

        public static OperationResultDto PrintCompanyAggregateReport(short companyId, ObservableCollection<CompanyAggregateReport> reportItems, bool includeCustomerTRN = false)
        {
            if (wordApp == null)
                wordApp = new Word.Application();

            var currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var templateFileFullName = Path.Combine(currentDirectory, includeCustomerTRN ? @"Factors\Statement.docx" : @"Factors\Statement-Without-TRN.docx");

            if (File.Exists(templateFileFullName))
            {
                wordApp.Documents.Open(templateFileFullName);

                var document = wordApp.ActiveDocument;

                var company = new CompanyRepository().Single(companyId);

                var companyTable = document.Tables[1];
                companyTable.Cell(1, 1).Range.Text = $"Customer Code: {company.CompanyId:CPY0000}";
                companyTable.Cell(1, 3).Range.Text = $"";
                companyTable.Cell(2, 1).Range.Text = company.Name;
                companyTable.Cell(2, 3).Range.Text = $"{DateTime.Today:yyyy-MMM-dd}";
                companyTable.Cell(3, 1).Range.Text = $"Tel: {company.Tel}";
                companyTable.Cell(4, 1).Range.Text = $"Fax: {company.Fax}";
                if (includeCustomerTRN)
                    companyTable.Cell(5, 1).Range.Text = $"TRN: {company.TaxRegistrationNumber}";

                var deliveriesTable = document.Tables[2];

                reportItems
                    .Select((ri, i) =>
                    {
                        var newRow = deliveriesTable.Rows.Add();
                        newRow.Cells[1].Range.Text = $"{i + 1}";
                        newRow.Cells[2].Range.Text = $"{ri.Month} of {ri.Year}";
                        newRow.Cells[3].Range.Text = ri.SiteName;
                        newRow.Cells[4].Range.Text = ri.InvoiceNo;
                        newRow.Cells[5].Range.Text = $"{ri.TotalAmount:n2} (m)";

                        return ri;
                    }).ToList();

                var totalPrice = reportItems.Sum(ri => ri.TotalAmount);

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

