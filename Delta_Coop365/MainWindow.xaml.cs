using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Delta_Coop365;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using QRCoder;

namespace Delta_Coop365
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DbAccessor dbAccessor = new DbAccessor();
        private DataStream dataStream = new DataStream("https://coop365.junoeuro.dk/api/Coop365/BakeOffVare");
        public static ObservableCollection<Product> productsCollection;
        private Product product;
        private ViewingProduct viewingProduct;
        public static Order theOrder;
        public static TextBlock textBlock;
        /// <summary>
        /// [Author] Daniel
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            MakePaths();
            productsCollection = new ObservableCollection<Product>();
            theOrder = new Order();
            textBlock = tbTotalAmount;
            UpdateDataBase();
            GetProducts();
            SetStock();
            ShowProducts();
            if (!dbAccessor.isDatabasePopulated("Orders"))
            {
                TestData testDataGenerator = new TestData();
                testDataGenerator.GenerateTestData();
            }
        }
        /// <summary>
        /// [Author] Daniel
        /// Generates folders to save Receipts and qr codes
        /// </summary>
        public void MakePaths()
        {
            string primaryPath = DbAccessor.GetSolutionPath();
            string receiptPath = primaryPath + "\\Receipts\\";
            try
            {
                if (!Directory.Exists(receiptPath))
                {
                    Directory.CreateDirectory(receiptPath);
                    Console.WriteLine("Receipts folder has been made");
                }
            }
            catch (System.Exception)
            {
                Console.WriteLine("Failed to generate path to Receipts");
            }
        }
        /// <summary>
        /// [Author] Daniel
        /// </summary>
        /// <returns></returns>
        public IEnumerable<XElement> GetData()
        {
            return dataStream.getData("BakeOffVare");
        }
        /// <summary>
        /// [Author] Daniel
        /// </summary>
        public void UpdateDataBase()
        {
            if (dbAccessor.isDatabasePopulated("Products"))
            {
                dbAccessor.updateProductsDaily(GetData());
            }
            else
            {
                foreach(XElement e in GetData())
                {
                    int productid = (int)e.Element("Varenummer");
                    double price = (double)e.Element("Pris");
                    string name = (string)e.Element("Name");
                    string ingredients = (string)e.Element("Ingredience");
                    dbAccessor.InsertIntoProducts(productid, name, ingredients, price);

                }
            }
            
        }
        /// <summary>
        /// [Author] Daniel
        /// </summary>
        public void GetProducts()
        {
            foreach(Product product in dbAccessor.GetProducts())
            {
                Console.WriteLine(product.GetID());
                Console.WriteLine(product.GetPrice());
                Console.WriteLine(product.GetName());
                Console.WriteLine(product.GetIngredients());
                productsCollection.Add(product);
            }
        }

        /// <summary>
        /// [ Author: Rebecca ]
        /// </summary>
        public void ShowProducts()
        {
            ICProducts.ItemsSource = productsCollection;
        }

        /// <summary>
        /// [ Author: Rebecca ]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BasketClick(object sender, MouseButtonEventArgs e)
        {
            CheckOut checkout = new CheckOut(theOrder);
            checkout.Show();

        }

        private void ReturnClick(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// [ Author: Rebecca ]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductClick(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            BitmapImage clickedImagepath = (BitmapImage)img.Source;

            foreach (Product product in productsCollection)
            {
                if (product.imgPath == clickedImagepath)
                {
                    this.product = new Product(product.GetID(), product.productName, product.GetStock(), product.GetPrice(), product.GetIngredients(), clickedImagepath.UriSource.AbsolutePath);
                    break;
                }
            }
            viewingProduct = new ViewingProduct(product);
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

        /// <summary>
        /// [ Author: Rebecca ]
        /// </summary>
        /// <param name="text"></param>
        public static void UpdateTotalPriceText(string text)
        {
            App.Current.Dispatcher.Invoke(delegate { textBlock.Text = text; });
        }

        /// <summary>
        /// [ Author: Rebecca ]
        /// </summary>
        private void SetStock()
        {
            Random rand = new Random();
            foreach (Product product in productsCollection)
            {
                int temp = rand.Next(5, 25);
                if (product.GetStock() == 0 || product.GetStock() == null)
                {
                    product.SetStock(temp);
                }
                dbAccessor.updateStock(product.GetID(), temp);
            }
        }

        /// <summary>
        /// [Author] Daniel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateDailyPDF(object sender, RoutedEventArgs e)
        {
            List<OrderLine> orderLines = dbAccessor.GetDailyOrderLines(DateTime.Now.Date);
            PrintPreview printer = new PrintPreview();
            printer.CreateDailyPDF(orderLines);
        }

        /// <summary>
        /// [Author] Daniel
        /// </summary>
        public static void ResetOrder()
        {
            theOrder = new Order();
            UpdateTotalPriceText(" ");
            
        }
    }
}
