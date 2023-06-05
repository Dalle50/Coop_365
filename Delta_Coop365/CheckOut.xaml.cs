using MailKit.Search;
using PdfSharp.Pdf.Content.Objects;
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
    /// [ Author: Pernille ]
    /// - Interaction logic for CheckOut.xaml
    /// </summary>
    public partial class CheckOut : Window
    {
        Order order;
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
                DontUpdateStock(orderLine.GetProduct());
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
            Console.WriteLine("The order history was cleared.");
        }

        private void btnAddMore_Click(object sender, RoutedEventArgs e)
        {
            Close();
            
            Console.WriteLine("Closing window so customer can add more items.");
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)checkBox.IsChecked)
            {
                CheckOutPointsCheck phoneNumberCheck= new CheckOutPointsCheck();
                phoneNumberCheck.Show();
                if (phoneNumberCheck.phoneNumberExsist == true)
                {
                    phoneNumberCheck.Close();
                    UpdateStockOnConfirm();
                    order.UpdateTotalPrice();
                    int orderId = dbAccessor.InsertIntoOrders(order.GetPrice(), date);
                    order.SetId(orderId);
                    foreach (OrderLine ol in orderLines)
                    {
                        dbAccessor.InsertIntoOrderLines(orderId, ol);
                    }
                    CreateQRCode(orderId);
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
            }
            Close();
        }
        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Console.WriteLine("Window closing but order is still saved.");
        }
        private void CreateQRCode(int orderId)
        {
            QrCodeService qRCodeGenerator = new QrCodeService();
            Bitmap qrCode = qRCodeGenerator.GenerateQRCodeImage(orderId);
            qRCodeGenerator.SaveQrCode(qrCode, orderId, DbAccessor.GetSolutionPath() + "\\QrCodes\\");
            PrintPreview CreateRecipe = new PrintPreview();  //the thing you want to print/display
            CreateRecipe.CreatePDFReceipt(order, orderId);
            Console.WriteLine("QR-Kode has been generated and added onto the PDF file.");
        }
        private void UpdateStockOnConfirm()
        {
            foreach (OrderLine ol in order.orderLines)
            {
                int productIndex = -1;
                Product p = ol.GetProduct();
                int newStock = p.GetStock() - ol.amount;
                foreach (Product collectiveProduct in MainWindow.products)
                {
                    productIndex++;
                    if (p.GetID() == collectiveProduct.GetID())
                    {
                        break;
                    }
                    MainWindow.products[productIndex].SetStock(newStock);
                    p.SetStock(newStock);
                }
                dbAccessor.updateStock(p.GetID(), newStock);
                Console.WriteLine("Stock has been updated to " + p.GetStock());
            }
        }
        private void DontUpdateStock(Product p)
        {
            Console.WriteLine("Stock is set back to: " + p.GetStock());
            p.SetStock(p.GetStock());
            DbAccessor d = new DbAccessor();
            d.updateStock(p.GetID(), p.GetStock());
        }

        //Skal implementeres.
        private void Email()
        {
            string pathToOrderReciept = DbAccessor.GetSolutionPath() + "\\Receipts\\" + MainWindow.theOrder.GetID() + ".pdf";
            Email email = new Email();
            email.SendNotice("daniel.htc.jacobsen@gmail.com", "Produkt er blevet solgt", "Produkterne er solgt på dette tidspunkt: " + date, new[] { pathToOrderReciept });
            MainWindow.theOrder = new Order();
            MainWindow.UpdateTotalPriceText("");
        }
    }
}