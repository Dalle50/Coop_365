using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Delta_Coop365
{
    internal class QrCodeService
    {
        public QrCodeService(int ordreId, string path) //Define string path and and ordreId in main and generate a QrCode with this: QrCodeService qRCodeGenerator = new QrCodeService(ordreId, path);
        {
            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(ordreId.ToString(), QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                if (!Directory.Exists(path))
                {
                    Console.WriteLine("Failed to save QrCodeImage, no valid path. Trying to create one");
                    Console.WriteLine("Creating directory: {0}", path);
                    Directory.CreateDirectory(path);
                }

                qrCodeImage.Save(path + "\\" + ordreId + ".png", ImageFormat.Png);

            }
            catch (System.Exception)
            {
                Console.WriteLine("Failed to generate QrCode");
            }
        }
    }
}
