using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Diagnostics;

namespace Delta_Coop365
{
    internal class PrintPreview
    {


        public PrintPreview(Order order, int ordreId, string path, string qrPath)
        {
            // Create a new PDF document            

            PdfDocument document = new PdfDocument();

            document.Info.Title = ordreId.ToString();
            // Create an empty page
            PdfPage page = document.AddPage();
            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);
            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
            // Draw the text
            gfx.DrawString(order.ToString(), font, XBrushes.Black,
            new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            // Insert Image
            XImage image = XImage.FromFile(qrPath + ordreId + ".Jpeg");
            gfx.DrawImage(image, 50, 50, 250, 250);          
            // Save the document...
            document.Save(path + ordreId + ".pdf");
            // ...and start a viewer.
            Process.Start(path + ordreId + ".pdf");            
        }
    }
}