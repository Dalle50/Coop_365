using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace Delta_Coop365
{
    internal class QrCodeService
    {
        public QrCodeService(string ordreId)//evt lav en filepath string.
        {            
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("userId", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            qrCodeImage.Save($"C:\\temp\\{ordreId}.png", ImageFormat.Png); 
            // ImageFormat.Jpeg, etc and folder location
        }
    }
}
