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
    /// Interaction logic for ViewingProduct.xaml
    /// </summary>
    public partial class ViewingProduct : Window
    {
        private Product product;
        private Order order;
        private DateTime date = DateTime.Now.Date;
        
        public ViewingProduct(Product currentProduct)
        {
            InitializeComponent();
            order = MainWindow.theOrder;
            product = currentProduct;
            GetImage();
            GetInfo(currentProduct);
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
            int amount = Int32.Parse(txtAmount.Text);
            if (amount > 0 || amount == product.GetStock() && product.GetStock() != 0)
            {
                if (order.orderLines.Count == 0)
                {
                    CreateNewOrderLine();
                    UpdateStock();
                    Close();
                }
                else if (order.orderLines.Count > 0)
                {
                    bool isExisting = false;
                    foreach (OrderLine orderLine in order.orderLines)
                    {
                        
                        if (orderLine.GetProduct().GetID() == product.GetID())
                        {
                            isExisting = true;
                            UpdateExistingOrderLine(orderLine);
                            UpdateStock();
                            Close();
                        }
                    }
                    if (!isExisting)
                    {
                        CreateNewOrderLine();
                        UpdateStock();
                        Close();
                    }
                }
                Console.WriteLine("Product added to cart");
            }
            else if (amount < 0)
            {
                MessageBox.Show("Vælg en mængde");
            }

            if (product.GetStock() == 0)
            {
                Console.WriteLine($"Der er nu ikke flere {product.GetName()} tilbage i montren" );
            }
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
            foreach (Product product in MainWindow.productsCollection)
            {
                productIndex++;
                if (product.GetID() == this.product.GetID())
                {
                    break;
                }
            }
            MainWindow.UpdateTotalPriceText(order.GetPrice().ToString() + " Kr.");
            MainWindow.productsCollection[productIndex].SetStock(product.GetStock() - amount);
            product.SetStock(product.GetStock() - amount);
        }

        private void GetInfo(Product product)
        {
            txtProductName.Text = product.GetName();
            txtNutrition.Text = product.GetIngredients();
            txtPrice.Text = product.GetPrice().ToString();
        }
        private void GetImage()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = product.imgPath.UriSource;
            bitmap.EndInit();
            image.Source = bitmap;
        }
    }
}

