using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Delta_Coop365
{
    internal class QrCodeService
    {
        QRCodeGenerator qrGenerator;
        public QrCodeService()
        {
            qrGenerator = new QRCodeGenerator();
        }

        public Bitmap GenerateQRCodeImage(int ordreId)
        {
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(ordreId.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;
        }

        public void SaveQrCode(Bitmap qrCode, int ordreId, string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Console.WriteLine("Failed to save QrCodeImage, no valid path. Trying to create one");
                    Console.WriteLine("Creating directory: {0}", path);
                    Directory.CreateDirectory(path);
                }
                qrCode.Save(path + ordreId + ".Jpeg", ImageFormat.Jpeg);
                Console.WriteLine("Saved the qr code.");
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to generate QrCode");
            }
        }
    }
}