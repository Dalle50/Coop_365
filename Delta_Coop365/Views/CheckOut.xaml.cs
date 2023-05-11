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
        DbAccessor db;
        Product product;
        DateTime date;
        private ObservableCollection<OrderLine> orderLinesCollection;
        public CheckOut()
        {

            InitializeComponent();
            orderLinesCollection = new ObservableCollection<OrderLine>();
            DataContext = orderLinesCollection;
            orderLinesCollection.CollectionChanged += OrderLinesCollection_CollectionChanged;
        }

        private void removeItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSubstract_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
        private void OrderLinesCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (orderLinesCollection == null)
            {
                return;
            }
            foreach (OrderLine orderLine in orderLinesCollection)
            {
                StackPanel stackpanel = new StackPanel();
                Image productPicture = (Image)orderScrollview.FindName("imgProduct");
                stackpanel.Children.Add(productPicture);

                TextBlock productName = (TextBlock)orderScrollview.FindName("txtProductName");
                productName.Text = orderLine.GetProduct().GetName();
                stackpanel.Children.Add(productName);

                TextBlock productPrice = (TextBlock)orderScrollview.FindName("txtProductprice");
                productPrice.Text = orderLine.GetProduct().GetPrice().ToString();
                stackpanel.Children.Add(productPrice);

                StackPanel ordersStackPanel = (StackPanel)orderScrollview.FindName("ordersStackPanel");
                ordersStackPanel.Children.Add(ordersStackPanel);
            }
        }
    }
}
