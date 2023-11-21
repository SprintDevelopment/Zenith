using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Fonts;
using System.IO;

namespace Zenith.Assets.Utils
{
    public class DocumentUtil
    {
        public void GeneratePdfReport(string reportTitle, string[] reportData)
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Add a page to the document
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing on the page
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("JetBrainsMono", 12, XFontStyleEx.Regular);

            // Draw the report title
            // Define the table headers
            string[] headers = { "ID", "Name", "Amount" };

            // Define the table data
            string[,] data = {
            { "1", "Product A", "$100" },
            { "2", "Product B", "$150" },
            { "3", "Product C", "$200" }
        };

            // Define the column widths and row height
            double[] columnWidths = { 50, 200, 100 };
            double rowHeight = 20;

            // Draw the table headers
            double xPosition = 100;
            double yPosition = 100;
            for (int i = 0; i < headers.Length; i++)
            {
                gfx.DrawString(headers[i], font, XBrushes.Black, new XRect(xPosition, yPosition, columnWidths[i], rowHeight), XStringFormats.Center);
                xPosition += columnWidths[i];
            }

            // Draw the table data
            yPosition += rowHeight;
            for (int i = 0; i < data.GetLength(0); i++)
            {
                xPosition = 100;
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    gfx.DrawString(data[i, j], font, XBrushes.Black, new XRect(xPosition, yPosition, columnWidths[j], rowHeight), XStringFormats.Center);
                    xPosition += columnWidths[j];
                }
                yPosition += rowHeight;
            }
            // Save the document to a file or memory stream
            document.Save("report.pdf");
        }

        
    }
    public class JetBrainsFontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            // Implement logic to retrieve the font data based on the specified faceName
            // You can load the font data from a file, stream, or any other source
            // For example, you can load the Tahoma font from the system's font directory

            // Replace "YourFontDirectory" with the actual directory where the Tahoma font file is located
            string fontFilePath = @"Fonts\JetBrainsMono.ttf";

            if (faceName == "JetBrainsMono")
            {
                return File.ReadAllBytes(fontFilePath);
            }

            // Return null if the specified font is not found
            return null;
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Implement logic to resolve the typeface based on the specified familyName, isBold, and isItalic
            // You can return the appropriate font information based on the specified parameters

            // For example, you can return the font information for the Tahoma font
            if (familyName == "JetBrainsMono")
            {
                return new FontResolverInfo("JetBrainsMono");
            }

            // Return null if the specified font is not found
            return null;
        }
    }
}
