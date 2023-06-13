using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Delta_Coop365
{
    /// <summary>
    /// [ Author: Rebecca ]
    /// Interaction logic for ViewingProduct.xaml
    /// </summary>
    public partial class ViewingProduct : Window
    {
        /// <summary>
        /// Field values used to save the current information about both the order and the product
        /// </summary>
        private Product product;
        private Order order;
        private DateTime date = DateTime.Now.Date;
        /// <summary>
        /// Constructs the ViewingProduct window with an instance of the current clicked product
        /// Goal is to display relevant information about the product.
        /// </summary>
        /// <param name="currentProduct"></param>
        public ViewingProduct(Product currentProduct)
        {
            InitializeComponent();
            order = MainWindow.theOrder;
            product = currentProduct;
            GetImage();
            GetInfo(currentProduct);
        }
        /// <summary>
        /// Button to increment amount of products
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(txtAmount.Text) <= product.GetStock() - 1)
            {
                int temp = Int32.Parse(txtAmount.Text);
                temp++;
                txtAmount.Text = temp.ToString();
            }
        }
        /// <summary>
        /// Button to decrement amount of products
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubstract_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(txtAmount.Text) > 0)
            {
                int temp = Int32.Parse(txtAmount.Text);
                temp--;
                txtAmount.Text = temp.ToString();
            }
        }
        /// <summary>
        /// Button to close the window securely
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Adds the OrderLine to the order
        /// Controls if the current product already exists in any of the orderlines
        /// to ensure not having multiple of same orderline
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Creates the orderLine with the current set field values
        /// and updates the total price on the main window
        /// </summary>
        private void CreateNewOrderLine()
        {
            int amount = Int32.Parse(txtAmount.Text);
            OrderLine orderLine = new OrderLine(product, amount, date);
            order.AddOrderLine(orderLine);
            order.UpdateTotalPrice();
            MainWindow.UpdateTotalPriceText(order.GetPrice().ToString() + " Kr.");
        }
        /// <summary>
        /// Updates the orderline that already exists in the orderlines list
        /// </summary>
        /// <param name="orderLine"></param>
        private void UpdateExistingOrderLine(OrderLine orderLine)
        {
            int amount = Int32.Parse(txtAmount.Text);
            orderLine.SetAmount(orderLine.GetAmount() + amount);
            order.UpdateTotalPrice();
            MainWindow.UpdateTotalPriceText(order.GetPrice().ToString() + " Kr.");
        }
        /// <summary>
        /// Gets the index of the product in Observable collection and updates the stock of the observable collection product
        /// </summary>
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
        /// <summary>
        /// Sets product information in the window
        /// </summary>
        /// <param name="product"></param>
        private void GetInfo(Product product)
        {
            txtProductName.Text = product.GetName();
            txtNutrition.Text = product.GetIngredients();
            txtPrice.Text = product.GetPrice().ToString();
        }
        /// <summary>
        /// Creates and sets image of the product
        /// Sets UriSource of the product impPath.
        /// </summary>
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

