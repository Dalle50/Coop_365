using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Delta_Coop365
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DbAccessor dbAccessor = new DbAccessor();
        DataStream data = new DataStream("https://coop365.junoeuro.dk/api/Coop365/BakeOffVare");
        ObservableCollection<Product> products;
        Product p;
        ViewingProduct vp;
        public static Order theOrder;
        public static TextBlock tb;

        public MainWindow()
        {
            InitializeComponent();
            products = new ObservableCollection<Product>();
            theOrder = new Order();
            tb = tbTotalAmount;
            updateDateBase();
            GetProducts();
            ShowProducts();
            SetStock();
        }
        
        public IEnumerable<XElement> getData()
        {
            return data.getData("BakeOffVare");
        }
        public void updateDateBase()
        {
            if (dbAccessor.isDatabasePopulated("Products"))
            {
                dbAccessor.updateProductsDaily(getData());
            }
            else
            {
                foreach(XElement e in getData())
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
            foreach(Product product in dbAccessor.GetProducts())
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
            Image img = (Image)sender;
            BitmapImage clickedImagepath = (BitmapImage)img.Source;

            foreach (Product product in products)
            {
                if (product.imgPath == clickedImagepath)
                {
                    p = new Product(product.GetID(), product.productName, product.GetStock(), product.GetPrice(), product.GetIngredients(), clickedImagepath.UriSource.AbsolutePath);
                    break;
                }
            }
            ViewingProduct viewProduct = new ViewingProduct(p);
            vp = viewProduct;
            if (this.p.GetStock() == 0)
            {
                //Grafik til at vise det her, aner ikke om vi kan opdatere tekst til at sige det.
                Console.WriteLine("Produktet er udsolgt.");
                MessageBox.Show("Produktet er udsolgt.");
            }
           else
            {
                vp.Show();
            }
        }

        public static void UpdateTotalPriceText(string text)
        {
            App.Current.Dispatcher.Invoke(delegate { tb.Text = text; });
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
