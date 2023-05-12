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
        private int orderId;

        public CheckOut()
        {
            InitializeComponent();
            orderLinesCollection = new ObservableCollection<OrderLine>();
            DataContext = orderLinesCollection;
            db = new DbAccessor();
            GetCartItems(orderId);
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
        private void GetCartItems(int orderID)
        {
            orderLinesCollection.Clear();
            List<OrderLine> orderLines = db.GetOrderLines(orderID);
            foreach (OrderLine item in orderLines)
            {
                Console.WriteLine("Adding products...");
                orderLinesCollection.Add(item);

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

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAddMore_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
