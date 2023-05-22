using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Shapes;

namespace Delta_Coop365
{
    internal class QrCodeService
    {
        QRCodeGenerator qrGenerator;
        public QrCodeService(int orderId)
        {
            qrGenerator = new QRCodeGenerator();
        }
        public Bitmap GenerateQRCodeImage(int orderId)
        {
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(orderId.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;
        }
        
        public void SaveQrCode(Bitmap qrCode, int orderId, string path)
        {
            
                qrCode.Save(path + orderId + ".Jpeg", ImageFormat.Jpeg);
            
            
        }        
    }
}