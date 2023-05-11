using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Delta_Coop365;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Delta_Coop365
{
    /// <summary>
    /// Interaction logic for CheckOut.xaml
    /// </summary>
    public partial class CheckOut : Window
    {
        private ObservableCollection<OrderLine> orderLinesCollection;
        DbAccessor db;
        public CheckOut()
        {
            InitializeComponent();
            orderLinesCollection = new ObservableCollection<OrderLine>();
            Console.WriteLine("Setting data context to the collection");
            DataContext = orderLinesCollection;

            try
            {
                Console.WriteLine("Trying to fetch the data from OrderLines");
                DbAccessor dbAcess = new DbAccessor();
                List<OrderLine> orderLines = dbAcess.GetOrderLines(2);
                foreach (OrderLine order in orderLines)
                {
                    orderLinesCollection.Add(order);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("getting data failed " + e.Message + " " + e.StackTrace);
            }
        }

        private void removeItem_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Remove item button is clicked.");
        }

        private void btnSubstract_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Substracting from product amount");
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Adding to product amount");
        }
        private void GetCartItems()
        {
            foreach (OrderLine item in orderLinesCollection)
            {
                Console.WriteLine("Adding products...");
                StackPanel stackpanel = new StackPanel();
                Image productPicture = (Image)orderScrollview.FindName("imgProduct");
                string imagePath = db.picturesUrl + item.GetProduct().GetID() + ".jpeg";
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath);
                bitmap.EndInit();
                productPicture.Source = bitmap;
                stackpanel.Children.Add(productPicture);

                TextBlock productName = (TextBlock)orderScrollview.FindName("txtProductName");
                productName.Text = item.GetProduct().GetName();
                stackpanel.Children.Add(productName);

                TextBlock productPrice = (TextBlock)orderScrollview.FindName("txtProductprice");
                productPrice.Text = item.GetProduct().GetPrice().ToString();
                stackpanel.Children.Add(productPrice);

                StackPanel ordersStackPanel = (StackPanel)orderScrollview.FindName("ordersStackPanel");
                ordersStackPanel.Children.Add(stackpanel);
            }
            if (orderLinesCollection == null)
            {
                Console.WriteLine("Collection is null.");
                return;
            }
        }
    }
}
