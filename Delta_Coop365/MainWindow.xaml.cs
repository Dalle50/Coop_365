using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Delta_Coop365
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DbAccessor dbAccessor = new DbAccessor();
        private DataStream dataStream = new DataStream("https://coop365.junoeuro.dk/api/Coop365/BakeOffVare");
        public static ObservableCollection<Product> products;
        private Product product;
        private ViewingProduct viewingProduct;        
        public static Order theOrder;
        public static TextBlock textBlock;

        public MainWindow()
        {
            InitializeComponent();
            products = new ObservableCollection<Product>();
            theOrder = new Order();
            textBlock = textBlockTotalAmount;
            UpdateDatebase();
            GetProducts();
            ShowProducts();
            SetStock();
        }

        public IEnumerable<XElement> GetData()
        {
            return dataStream.getData("BakeOffVare");
        }
        public void UpdateDatebase()
        {
            if (dbAccessor.IsDatabasePopulated("Products"))
            {
                dbAccessor.updateProductsDaily(GetData());
            }
            else
            {
                foreach (XElement e in GetData())
                {
                    int productid = (int)e.Element("Varenummer");
                    double price = (double)e.Element("Pris");
                    string name = (string)e.Element("Name");
                    string ingredients = (string)e.Element("Ingredience");
                    dbAccessor.InsertIntoProducts(productid, name, ingredients, price);

                }
            }
        }
        public void GetProducts()
        {
            foreach (Product product in dbAccessor.GetProducts())
            {
                Console.WriteLine(product.GetID());
                Console.WriteLine(product.GetPrice());
                Console.WriteLine(product.GetName());
                Console.WriteLine(product.GetIngredients());
                products.Add(product);
            }
        }
        public void ShowProducts()
        {
            ICProducts.ItemsSource = products;
        }

        private void BasketClick(object sender, MouseButtonEventArgs e)
        {
            CheckOut checkout = new CheckOut(theOrder);
            checkout.Show();

        }
        private void ReturnClick(object sender, MouseButtonEventArgs e)
        {

        }
        private void ProductClick(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            BitmapImage clickedImagepath = (BitmapImage)image.Source;

            foreach (Product product in products)
            {
                if (product.imagePath == clickedImagepath)
                {
                    this.product = new Product(product.GetID(), product.productName, product.GetStock(), product.GetPrice(), product.GetIngredients(), clickedImagepath.UriSource.AbsolutePath);
                    break;
                }
            }
            ViewingProduct viewProduct = new ViewingProduct(product);
            viewingProduct = viewProduct;
            if (product.GetStock() == 0)
            {
                //Grafik til at vise det her, aner ikke om vi kan opdatere tekst til at sige det.
                Console.WriteLine("Produktet er udsolgt.");
                MessageBox.Show("Produktet er udsolgt.");
            }
            else
            {
                viewingProduct.Show();
            }
        }

        public static void UpdateTotalPriceText(string text)
        {
            App.Current.Dispatcher.Invoke(delegate { textBlock.Text = text; });
        }

        private void SetStock()
        {
            Random rand = new Random();
            foreach (Product product in products)
            {
                int temp = rand.Next(5, 25);
                if (product.GetStock() == 0 || product.GetStock() == null)
                {
                    product.SetStock(temp);
                }
                dbAccessor.updateStock(product.GetID(), temp);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<OrderLine> ols = dbAccessor.GetDailyOrderLines(DateTime.Now.Date);
            PrintPreview printer = new PrintPreview();
            printer.CreateDailyPDF(ols);
        }
    }
}
