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
        //Løbe gennem listen af orderLines som er på order
        //constructor i main window.
        Order order;
        DbAccessor db;
        private int orderId;

        public CheckOut(Order order)
        {
            InitializeComponent();
            this.order = order;
            GetCartItems();
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
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
