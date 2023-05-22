using PdfSharp.Drawing;
using PdfSharp.Pdf;
using QRCoder;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;


namespace Delta_Coop365
{
    internal class GenerateService
    {
        PdfDocument document;
        QRCodeGenerator qrGenerator;


        public GenerateService(Order order, int orderId, string path, string qrPath)
        {

            // Create a new PDF document 
            document = new PdfDocument();
            // Create QrCodeBitMap
            qrGenerator = new QRCodeGenerator();
        }
        public Bitmap GenerateQRCodeImage(int orderId)
        {
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(orderId.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;
        }
        public void GeneratePdf(Order order, int orderId, string path, string qrPath)
        {
            // Define the documents name/title
            document.Info.Title = orderId.ToString();
            // Create an empty page
            PdfPage page = document.AddPage();
            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);
            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
            // Draw the text defined as the first parameter of the DrawString method
            
            gfx.DrawString("Ordernr : " + order.GetID().ToString(), font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.TopCenter);
            int counter = 0;
            foreach (OrderLine ol in order.GetOrderLines())
            {
                counter++;
                font = new XFont("Verdana", 10, XFontStyle.BoldItalic);
                // Draw the text
                gfx.DrawString(ol.productName, font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height),
                    XStringFormats.TopLeft);
                gfx.DrawString(ol.amount.ToString(), font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height),
                    XStringFormats.TopCenter);
                gfx.DrawString((ol.GetAmount() * ol.GetProduct().GetPrice()).ToString("N" + 2) + "(" + ol.GetProduct().GetPrice() + "  pr. stk)", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height),
                    XStringFormats.TopRight);                

            }
            //Define page Location for QrCode with 0 meaning last page
            PdfPage qrCodepage = document.Pages[0];
            // Get an XGraphics object for drawing
            XGraphics gfxQR = XGraphics.FromPdfPage(qrCodepage);
            // Insert Image
            XImage image = XImage.FromGdiPlusImage(GenerateQRCodeImage(orderId)); //you can use XImage.FromGdiPlusImage to get the bitmap object as image (not a stream)
            gfxQR.DrawImage(image, 50, 50, 150, 150);
        }
        public void SaveQrCode(Bitmap qrCode, int orderId, string qrPath)// Save QrCode to a file and seperate folder
        {
            qrCode.Save(qrPath + orderId + ".Jpeg", ImageFormat.Jpeg);

        }
        public void SavePdf(int orderId, string path)// Save Pdf to a file and seperate folder
        {
            document.Save(path + orderId + ".pdf");
        }
        public void OpenPdf(int orderId, string path)// Open Pdf
        {
            Process.Start(path + orderId + ".pdf");
        }
    }
}