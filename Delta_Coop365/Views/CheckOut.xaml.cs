using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents.Serialization;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;
using Delta_Coop365;
using Microsoft.EntityFrameworkCore.Migrations;


namespace Delta_Coop365
{
    /// <summary>
    /// Interaction logic for CheckOut.xaml
    /// </summary>
    public partial class CheckOut : Window
    {
        //Løbe gennem listen af orderLines som er på order
        //constructor i main window.
        Order order;
        OrderLine orderLine;
        ObservableCollection<OrderLine> orderLines;

        public CheckOut(Order order)
        {
            InitializeComponent();
            this.order = order;
            orderLines = new ObservableCollection<OrderLine>();
            GetCartItems();
            ShowCartItems();
        }

        private void removeItem_Click(object sender, RoutedEventArgs e)
        {
            //orderLines.RemoveAt(orderLine);
            Console.WriteLine("Remove item button is clicked.");
        }

        private void btnSubstract_Click(object sender, RoutedEventArgs e)
        {
            int amount = orderLine.amount;
            amount = -1;
            Console.WriteLine("Substracting 1 from " + amount + " which is the amount for the product: " + orderLine.GetProduct());
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            int amount = orderLine.amount;
            amount = +1;
            Console.WriteLine("adding 1 to " + amount + " which is the amount for the product: " + orderLine.GetProduct());
        }
        private void GetCartItems()
        {
            if (order != null)
            {
                foreach (var item in order.GetOrderLines())
                {
                    orderLine.GetAmount();
                    orderLines.Add(item);
                    Console.WriteLine("Adding " + item.GetProduct() + " ( " + "amount: " + item.GetAmount() + ") " + "to the collection");
                }
            }
            else
            {
                Console.WriteLine("No products is in the cart.");
            }
        }
        private void ShowCartItems()
        {
            cartItems.ItemsSource = orderLines;
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            order.ClearOrderLines();
            Close();
            Console.WriteLine("The order history was cleared and nothing was added to the database.");
        }

        private void btnAddMore_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Console.WriteLine("Closing window so customer can add more items.");
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Print_WPF_Preview(Grid_Plan); //the thing you want to print/display
            QrCodeService qRCodeGenerator = new QrCodeService(ordreId, path);
        }
    }
}