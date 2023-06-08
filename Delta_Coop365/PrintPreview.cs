using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Delta_Coop365
{
    internal class PrintPreview
    {
        public PrintPreview()
        {
        }
        public void CreatePDFReceipt(Order order, int orderId)
        {
            double x = 0;
            double y = 50;
            string price = order.GetPrice().ToString();
            PdfDocument document = new PdfDocument();
            document.Info.Title = orderId.ToString();
            // Create an empty page
            PdfPage page = document.AddPage();
            document.Save(DbAccessor.GetSolutionPath() + "\\Receipts\\" + orderId + ".pdf");
            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);
            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
            // Draw the text
            gfx.DrawString("Ordernr : " + order.GetID().ToString(), font, XBrushes.Black,
            new XRect(x, y, page.Width, page.Height), XStringFormats.TopCenter);
            y += 40;
            int counter = 0;
            foreach (OrderLine ol in order.GetOrderLines())
            {
                counter++;
                font = new XFont("Verdana", 10, XFontStyle.BoldItalic);

                // Draw the text
                gfx.DrawString(ol.productName, font, XBrushes.Black, new XRect(x, y, page.Width, page.Height),
                    XStringFormats.TopLeft);
                gfx.DrawString(ol.amount.ToString(), font, XBrushes.Black, new XRect(x, y, page.Width, page.Height),
    XStringFormats.TopCenter);
                gfx.DrawString((ol.GetAmount() * ol.GetProduct().GetPrice()).ToString("N" + 2) + "(" + ol.GetProduct().GetPrice() + "  pr. stk)", font, XBrushes.Black, new XRect(x, y, page.Width, page.Height),
    XStringFormats.TopRight);
                y += 40;

            }
            XImage image = XImage.FromFile(DbAccessor.GetSolutionPath() + "\\QrCodes\\" + orderId + ".Jpeg");
            gfx.DrawImage(image, x, y, page.Width, page.Width);

            document.Save(DbAccessor.GetSolutionPath() + "\\Receipts\\" + orderId + ".pdf");
        }
        public void CreateDailyPDF(List<OrderLine> ols)
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss");
            double x = 0;
            double y = 50;
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Daglig Rapport for salg " + date.ToString();
            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);
            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
            // Draw the text
            gfx.DrawString("Oversigt", font, XBrushes.Black,
            new XRect(x, y, page.Width, page.Height), XStringFormats.TopCenter);
            y += 40;
            int counter = 0;
            double total = 0.0;
            int totalMængde = 0;
            foreach (OrderLine ol in ols)
            {
                counter++;
                font = new XFont("Verdana", 10, XFontStyle.BoldItalic);

                // Draw the text
                gfx.DrawString(ol.productName, font, XBrushes.Black, new XRect(x, y, page.Width, page.Height),
                    XStringFormats.TopLeft);
                gfx.DrawString(ol.amount.ToString(), font, XBrushes.Black, new XRect(x, y, page.Width, page.Height),
    XStringFormats.TopCenter);
                gfx.DrawString((ol.GetAmount() * ol.GetProduct().GetPrice()).ToString("N" + 2) + "(" + ol.GetProduct().GetPrice() + "  pr. stk)", font, XBrushes.Black, new XRect(x, y, page.Width, page.Height),
    XStringFormats.TopRight);
                y += 40;
                total += (ol.amount * ol.GetProduct().GetPrice());
                totalMængde += ol.amount;
            }
            gfx.DrawString(("Total mængde produkter: " + totalMængde), font, XBrushes.Black, new XRect(x, y, page.Width, page.Height),
    XStringFormats.TopLeft);
            gfx.DrawString(("Total pris for  produkter: " + total), font, XBrushes.Black, new XRect(x, y, page.Width, page.Height),
    XStringFormats.TopCenter);

            string path = DbAccessor.GetSolutionPath();
            document.Save(path + "\\Receipts\\" + "Daglig_Rapport_for_salg_" + date + ".pdf");
            EmailView emailView = new EmailView(true, "Daily Report", "Den daglige rapport er vedlagt som fil", "Daglig_Rapport_for_salg_" + date + ".pdf");
            emailView.Show();
        }
    }
}