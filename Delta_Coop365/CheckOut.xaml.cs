using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

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
        DbAccessor dbAccessor = new DbAccessor();
        DateTime date = DateTime.Now;


        public CheckOut(Order o)
        {
            InitializeComponent();
            order = o;
            orderLines = new ObservableCollection<OrderLine>();
            GetCartItems();
            ShowCartItems();
        }

        private void removeItem_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var context = button.DataContext;

            if (context is OrderLine orderLine && orderLine.amount > 0)
            {
                orderLines.Remove(orderLine);
                order.DeleteOrderLine(orderLine);
                order.UpdateTotalPrice();
                MainWindow.UpdateTotalPriceText(order.GetPrice().ToString() + " Kr.");
                App.Current.Dispatcher.Invoke(delegate { txtTotal.Text = order.GetPrice().ToString(); });
            }
        }

        private void btnSubstract_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var context = button.DataContext;

            if (context is OrderLine orderLine && orderLine.amount > 0)
            {
                orderLine.amount--;
                orderLine.SetAmount(orderLine.amount);
                order.UpdateTotalPrice();
                MainWindow.UpdateTotalPriceText(order.GetPrice().ToString() + " Kr.");
                App.Current.Dispatcher.Invoke(delegate { txtTotal.Text = order.GetPrice().ToString(); });
                cartItems.Items.Refresh();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var context = button.DataContext;
            if (context is OrderLine orderLine)
            {
                orderLine.amount++;
                orderLine.SetAmount(orderLine.amount);
                order.UpdateTotalPrice();
                MainWindow.UpdateTotalPriceText(order.GetPrice().ToString() + " Kr.");
                App.Current.Dispatcher.Invoke(delegate { txtTotal.Text = order.GetPrice().ToString(); });
                cartItems.Items.Refresh();
            }
        }
        private void GetCartItems()
        {
            if (order != null)
            {
                foreach (var item in order.GetOrderLines())
                {
                    if (orderLines.Contains(item.GetProduct()))
                    {
                        item.amount++;
                        orderLine.SetAmount(orderLine.amount);
                    }
                    else
                    {
                        orderLines.Add(item);
                        Console.WriteLine("Adding " + item.GetProduct().productName + " ( " + "amount: " + item.GetAmount() + ") " + "to the collection");
                    }
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
            txtTotal.Text = order.GetPrice().ToString() + " Kr.";
        }

        private void btnUndoAll_Click(object sender, RoutedEventArgs e)
        {
            order.ClearOrderLines();
            order.UpdateTotalPrice();
            MainWindow.UpdateTotalPriceText(order.GetPrice().ToString() + " Kr.");
            App.Current.Dispatcher.Invoke(delegate { txtTotal.Text = order.GetPrice().ToString(); });
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
            order.UpdateTotalPrice();
            int orderId = dbAccessor.InsertIntoOrders(order.GetPrice(), date);
            order.SetId(orderId);
            foreach(OrderLine ol in orderLines)
            {
                dbAccessor.InsertIntoOrderLines(orderId, ol);
            }
            QrCodeService qRCodeGenerator = new QrCodeService();  //
            Bitmap qrCode = qRCodeGenerator.GenerateQRCodeImage(orderId);
            qRCodeGenerator.SaveQrCode(qrCode, orderId, DbAccessor.GetSolutionPath() + "\\QrCodes\\");
            PrintPreview CreateRecipe = new PrintPreview();  //the thing you want to print/display
            CreateRecipe.CreatePDFReceipt(order, orderId);
            Close();
        }
    }
}