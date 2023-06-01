using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Delta_Coop365
{
    /// <summary>
    /// Interaction logic for ViewingProduct.xaml
    /// </summary>
    public partial class ViewingProduct : Window
    {
        Product product;
        Order order;
        DateTime date = DateTime.Now.Date;

        public ViewingProduct(Product product)
        {
            InitializeComponent();
            order = MainWindow.theOrder;
            this.product = product;
            getImage();
            getInfo(product);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(txtAmount.Text) <= product.GetStock() - 1)
            {
                int temp = Int32.Parse(txtAmount.Text);
                temp++;
                txtAmount.Text = temp.ToString();
            }
        }

        private void btnSubstract_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(txtAmount.Text) > 0)
            {
                int temp = Int32.Parse(txtAmount.Text);
                temp--;
                txtAmount.Text = temp.ToString();
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddToCart(object sender, RoutedEventArgs e)
        {
            var stock = product.GetStock();
            int amount = int.Parse(txtAmount.Text);

            if (stock == 0)
            {
                Console.WriteLine("Produktet er nu udsolgt...");
                return;
            }

            if (amount > stock)
            {
                Console.WriteLine($"Der kun {stock} på lager...");
                return;
            }

            if (amount < 1)
            {
                MessageBox.Show("Vælg en mængde");
                return;
            }

            if (order.orderLines.Any(x => x.GetProduct().GetID() == product.GetID()))
            {
                var orderline = order.orderLines.First(x => x.GetProduct().GetID() == product.GetID());
                UpdateExistingOrderLine(orderline);
            }
            else
            {
                CreateNewOrderLine();
            }

            UpdateStock();
            Close();

            Console.WriteLine("Product added to cart");
        }

        private void CreateNewOrderLine()
        {
            int amount = Int32.Parse(txtAmount.Text);
            OrderLine orderLine = new OrderLine(product, amount, date);
            order.AddOrderLine(orderLine);
            order.UpdateTotalPrice();
            MainWindow.UpdateTotalPriceText(order.GetPrice().ToString() + " Kr.");
        }

        private void UpdateExistingOrderLine(OrderLine orderLine)
        {
            int amount = Int32.Parse(txtAmount.Text);
            orderLine.SetAmount(orderLine.GetAmount() + amount);
            order.UpdateTotalPrice();
            MainWindow.UpdateTotalPriceText(order.GetPrice().ToString() + " Kr.");
        }

        private void UpdateStock()
        {
            int amount = Int32.Parse(txtAmount.Text);
            int productIndex = -1;
            foreach (Product product in MainWindow.products)
            {
                productIndex++;
                if (product.GetID() == this.product.GetID())
                {
                    break;
                }
            }
            MainWindow.UpdateTotalPriceText(order.GetPrice().ToString() + " Kr.");
            MainWindow.products[productIndex].SetStock(product.GetStock() - amount);
            product.SetStock(product.GetStock() - amount);
        }

        private void getInfo(Product product)
        {
            txtProductName.Text = product.GetName();
            txtNutrition.Text = product.GetIngredients();
            txtPrice.Text = product.GetPrice().ToString();
        }

        private void getImage()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = product.imagePath.UriSource;
            bitmap.EndInit();
            image.Source = bitmap;
        }
    }
}

