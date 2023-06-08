using MailKit.Search;
using PdfSharp.Pdf.Content.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace Delta_Coop365
{
    /// <summary>
    /// [ Author: Pernille ]
    /// - Interaction logic for CheckOut.xaml
    /// </summary>
    public partial class CheckOut : Window
    {
        public static Order order;
        ObservableCollection<OrderLine> orderLines;
        DbAccessor dbAccessor = new DbAccessor();
        Email emailService = new Email();
        public static Customer customer = null;
        DateTime date = DateTime.Now;
        private bool isPhoneNumberCheckOpened = false;

        /// <summary>
        /// Opens checkout Window with order object
        /// </summary>
        /// <param name="o"></param>
        public CheckOut(Order o)
        {
            InitializeComponent();
            order = o;
            orderLines = new ObservableCollection<OrderLine>();
            GetCartItems();
            ShowCartItems();
        }
        /// <summary>
        /// Removes orderline from the order both visual in the ObservableCollection and in the order object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Decrement the amount of products on the orderline.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Increments the amount of products
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var context = button.DataContext;
            if (context is OrderLine orderLine)
            {
                if (orderLine.amount <= orderLine.GetProduct().GetStock())
                {
                    orderLine.amount++;
                    orderLine.SetAmount(orderLine.amount);
                }

                orderLine.SetDate(date);
                order.UpdateTotalPrice();
                MainWindow.UpdateTotalPriceText(order.GetPrice().ToString() + " Kr.");
                App.Current.Dispatcher.Invoke(delegate { txtTotal.Text = order.GetPrice().ToString(); });
                cartItems.Items.Refresh();
            }
        }
        /// <summary>
        /// Get all the orderLines
        /// </summary>
        private void GetCartItems()
        {
            if (order != null)
            {
                foreach (var item in order.GetOrderLines())
                {
                        orderLines.Add(item);
                        Console.WriteLine("Adding " + item.GetProduct().productName + " ( " + "amount: " + item.GetAmount() + ") " + "to the collection");
                }
            }
            else
            {
                Console.WriteLine("No products is in the cart.");
            }
        }
        /// <summary>
        /// Changes itemsource to the orderlinescollection
        /// updates the total price
        /// </summary>
        private void ShowCartItems()
        {
            cartItems.ItemsSource = orderLines;
            txtTotal.Text = order.GetPrice().ToString() + " Kr.";
        }
        /// <summary>
        /// Clears the orderlines
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUndoAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (OrderLine orderLine in orderLines)
            {
                int amount = orderLine.amount;
                int productIndex = -1;
                foreach (Product product in MainWindow.productsCollection)
                {
                    productIndex++;
                    if (product.GetID() == orderLine.GetProduct().GetID())
                    {
                        break;
                    }
                }
                MainWindow.UpdateTotalPriceText(order.GetPrice().ToString() + " Kr.");
                MainWindow.productsCollection[productIndex].SetStock(orderLine.GetProduct().GetStock() + amount);
                orderLine.GetProduct().SetStock(orderLine.GetProduct().GetStock() + amount);
            }
            order.ClearOrderLines();
            order.UpdateTotalPrice();

            MainWindow.UpdateTotalPriceText(order.GetPrice().ToString() + " Kr.");
            App.Current.Dispatcher.Invoke(delegate { txtTotal.Text = order.GetPrice().ToString(); });
            Close();
            Console.WriteLine("The order history was cleared.");
        }

        /// <summary>
        /// [Author] Daniel
        /// Controls only one window is opened at a time for registering
        /// And checks customer for phonenumber, if checkbox is marked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (isPhoneNumberCheckOpened)
            {
                MessageBox.Show("Please finish what you started");
            }
            if ((bool)checkBox.IsChecked)
            {

                if (!isPhoneNumberCheckOpened)
                {
                    CheckOutPointsCheck phoneNumberCheck = new CheckOutPointsCheck(ConvertItemsToPoints(orderLines.ToList()));
                    phoneNumberCheck.Closing += PhoneNumberCheck_Closing; // Attach the event handler
                    phoneNumberCheck.Show();
                    isPhoneNumberCheckOpened = true; // Set the flag to indicate that phoneNumberCheck window is opened
                }
            }
            else
            {
                UpdateStockOnConfirm();
                order.UpdateTotalPrice();
                int orderId = dbAccessor.InsertIntoOrders(order.GetPrice(), date);
                order.SetId(orderId);
                foreach (OrderLine ol in orderLines)
                {
                    dbAccessor.InsertIntoOrderLines(orderId, ol);
                }
                CreateQRCode(orderId);
                order = new Order();
                Close();
                MainWindow.ResetOrder();
            }
        }
        /// <summary>
        /// [Author] Daniel
        /// Eventhandler for noticing when phonenumbercheck is closed
        /// Confirms if customer exists, and if the customer just got registered it updates points.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneNumberCheck_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isPhoneNumberCheckOpened = false;
            bool isCustomerRegistered = false;
            CheckOutPointsCheck phoneNumberCheck = (CheckOutPointsCheck)sender;
            if (phoneNumberCheck.phoneNumberExist)
            {
                UpdateStockOnConfirm();
                order.UpdateTotalPrice();
                Customer c = dbAccessor.GetCustomer(Int32.Parse(phoneNumberCheck.phoneNumberTextBox.Text));
                int orderId = dbAccessor.InsertIntoOrders(order.GetPrice(), date, c.KundeID);
                order.SetId(orderId);
                foreach (OrderLine ol in orderLines)
                {
                    dbAccessor.InsertIntoOrderLines(orderId, ol);
                }

                CreateQRCode(orderId);
                isCustomerRegistered = true;

                
            }
            if (isCustomerRegistered)
            {
                dbAccessor.UpdateCustomerPoints(customer.phoneNumber, dbAccessor.GetCustomerPoints(customer.phoneNumber) + ConvertItemsToPoints(MainWindow.theOrder.GetOrderLines()));
            }
            MainWindow.ResetOrder();
            Close();
        }
        /// <summary>
        /// Back button closes the window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Console.WriteLine("Window closing but order is still saved.");
        }
        /// <summary>
        /// Generates the qr code
        /// </summary>
        /// <param name="orderId"></param>
        private void CreateQRCode(int orderId)
        {
            QrCodeService qRCodeGenerator = new QrCodeService();
            Bitmap qrCode = qRCodeGenerator.GenerateQRCodeImage(orderId);
            qRCodeGenerator.SaveQrCode(qrCode, orderId, DbAccessor.GetSolutionPath() + "\\QrCodes\\");
            PrintPreview CreateRecipe = new PrintPreview();  //the thing you want to print/display
            CreateRecipe.CreatePDFReceipt(order, orderId);
            Console.WriteLine("QR-Kode has been generated and added onto the PDF file.");
        }
        /// <summary>
        /// Sends an Email, but also shows the Email in the form of a pop-up window
        /// </summary>
        private void UpdateStockOnConfirm()
        {
            foreach (OrderLine orderline in order.orderLines)
            {
                int productIndex = -1;
                Product p = orderline.GetProduct();
                int newStock = p.GetStock() - orderline.amount;
                string date = DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss");
                if ( newStock <= 0) 
                {
                    Console.WriteLine("Sender mail.....");
                    emailService.SendNotice("daniel.htc.jacobsen@gmail.com",
                        "Stock is empty",
                        //string[]  er med så man kan attatch den i en email
                        "The stock of: " + p.GetName() +" is emptied out at the time: " + date, new string[] {});
                    EmailView emailView = new EmailView(false, "Stock is empty", "The stock of: " + p.GetName() + " is emptied out at the time: " + date, "");
                    emailView.Show();
                }
                foreach (Product collectiveProduct in MainWindow.productsCollection)
                {
                    productIndex++;
                    if (p.GetID() == collectiveProduct.GetID())
                    {
                        break;
                    }
                    MainWindow.productsCollection[productIndex].SetStock(newStock);
                    p.SetStock(newStock);
                }
                dbAccessor.updateStock(p.GetID(), newStock);
                Console.WriteLine("Stock has been updated to " + p.GetStock());
            }
        }
        /// <summary>
        /// [Author] Daniel
        /// Take the orderlines of the order
        /// Converts the amount of items to points
        /// Given that every produkt gives 5 points.
        /// <param name="orderLines"></param>
        /// <returns></returns>
        public static double ConvertItemsToPoints(List<OrderLine> orderLines)
        {
            double points = 0;
            foreach(OrderLine line in orderLines)
            {
                points += line.amount * 5;
            }
            return points;
        }
    }
}