using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents.Serialization;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;
using System.Windows;
using QRCoder;
using System.Windows.Shapes;
using System.IO.Packaging;
using System.Collections.ObjectModel;

namespace Delta_Coop365
{
    class Print_Preview
    {
        public void CreatePreviewRecipe(, int ordreId)
        {
            //------------< WPF_Print_current_Window > ------------
            var recipe = new XpsDocument(package, CompressionOption.Normal);
            var xpsWriter = xpsDoc.AddFixedDocumentSequence();

            var fixedDocSeq = xpsDoc.GetFixedDocumentSequence();

            // A4 = 210 x 297 mm = 8.267 x 11.692 inches = 793.632 * 1122.432 dots
            fixedDocSeq.DocumentPaginator.PageSize = new Size(793.632, 1122.432);

            foreach (var fixedDocument in fixedDocuments)
            {
                var docWriter = xpsWriter.AddFixedDocument();

                var pageWriter = docWriter.AddFixedPage();

                var image = pageWriter.AddImage(XpsImageType.JpegImageType);

                Stream imageStream = image.GetStream();

                //Write your image to stream

                //Write the rest of your document based on the fixedDocument object
            }



            // A4 = 210 x 297 mm = 8.267 x 11.692 inches = 793.632 * 1122.432 dots
            fixedDocSeq.DocumentPaginator.PageSize = new Size(793.632, 1122.432);

            foreach (var fixedDocument in fixedDocuments)
            {
                var docWriter = xpsWriter.AddFixedDocument();

                var pageWriter = docWriter.AddFixedPage();

                var image = pageWriter.AddImage(XpsImageType.JpegImageType);

                Stream imageStream = image.GetStream();

                //Write your image to stream

                //Write the rest of your document based on the fixedDocument object

                XpsDocument recipe = new XpsDocument("recipe.xps", FileAccess.ReadWrite);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(recipe);
            SerializerWriterCollator preview_Document = writer.CreateVisualsCollator();
            preview_Document.BeginBatchWrite();
            preview_Document.Write(wpf_Element); //*this or wpf xaml control
            preview_Document.i
            preview_Document.EndBatchWrite();

            //--</ create xps document > --          

            FixedDocumentSequence preview = recipe.GetFixedDocumentSequence();

            var window = new Window();
            window.Content = new DocumentViewer { Document = preview };
            window.ShowDialog();     
            recipe.Close();

            //------------</ WPF_Print_current_Window > ------------

        }
    }
}






    /*  private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Print_WPF_Preview(Grid_Plan); //the thing you want to print/display
            QrCodeService qRCodeGenerator = new QrCodeService(ordreId, path);
        }*/