using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Delta_Coop365
{
    /// <summary>
    /// Interaction logic for EmailView.xaml
    /// </summary>
    public partial class EmailView : Window
    {
        public EmailView(bool isDailyReport, string subject, string body, string dailyReport)
        {
            InitializeComponent();
            TextBlockReciever.Text = "ButiksAdministration";
            TextBlockReciever.Foreground = Brushes.Black;
            if (isDailyReport)
            {
                TextBlockAttachment.Text = dailyReport;
                TextBlockAttachment.Foreground = Brushes.Black;
                TextBlockCc.Text = "";
            }
            else if(!isDailyReport)
            {
                TextBlockCc.Text = "Bageri";
                TextBlockCc.Foreground = Brushes.Black;
                TextBlockAttachment.Text = "";
            }
            TextBlockSubject.Text = subject;
            TextBlockSubject.Foreground = Brushes.Black;
            TextBlockBody.Text = body;
            TextBlockBody.Foreground = Brushes.Black;
        }
    }
}
