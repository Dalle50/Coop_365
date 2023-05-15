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
        
        public MainWindow()
        {
            InitializeComponent();
            products = new ObservableCollection<Product>();
            updateDateBase();
            GetProducts();
            ShowProducts();
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
            //CheckOut checkout = new CheckOut();
            //checkout.Show();
        }
        private void ProductClick(object sender, MouseButtonEventArgs e)
        {
            Product p;
            for(int i = 0; i < products.Count; i++)
            {
                
            }
            //ViewingProduct vp = new ViewingProduct();
            //vp.Show();
        }
    }
}
