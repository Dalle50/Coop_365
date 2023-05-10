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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Delta_Coop365;
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
        
        public MainWindow()
        {
            InitializeComponent();
            updateDateBase();
            updateDateBase();

        }
        public IEnumerable<XElement> getData()
        {
            return data.getData("BakeOffVare");
        }
        public void updateDateBase()
        {
            if (dbAccessor.isDatabasePopulated("Products"))
            {
                dbAccessor.updateProductsDaily(getData())
            }
            else
            {
                foreach(XElement e in getData())
                {
                    int productid = (int)e.Element("Varenummer");
                    double price = (double)e.Element("Pris");
                    string name = (string)e.Element("Name");
                    string ingredients = (string)e.Element("Ingredience");

                    object x = productid.GetType();
                    object y = price.GetType();
                    object u = name.GetType();
                    object i = ingredients.GetType();
                    Console.WriteLine(x);
                    Console.WriteLine(y);
                    Console.WriteLine(u);
                    Console.WriteLine(i);
                    dbAccessor.insertIntoProducts(productid, name, ingredients, price);
                }
            }
            
        }
    }
}
