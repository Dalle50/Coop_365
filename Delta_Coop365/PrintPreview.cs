using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Diagnostics;

namespace Delta_Coop365
{
    internal class PrintPreview
    {


        public PrintPreview(Order order, int orderId, string path, string qrPath)
        {
            // Create a new PDF document 
            PdfDocument document = new PdfDocument();
            // Define the documents name/title
            document.Info.Title = orderId.ToString();
            // Create an empty page
            PdfPage page = document.AddPage();
            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);
            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
            // Draw the text defined as the first parameter of the DrawString method
            gfx.DrawString(order.ToString(), font, XBrushes.Black,
            new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            // Define the path of the desired image
            XImage image = XImage.FromFile(qrPath + orderId + ".Jpeg");
            // Insert Image
            gfx.DrawImage(image, 50, 50, 250, 250);
            // Save the document
            document.Save(path + orderId + ".pdf");
            // Open the document
            Process.Start(path + orderId + ".pdf");
        }
    }
}