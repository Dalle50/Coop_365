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
        DbAccessor db;
        Product product;
        public ViewingProduct(Product product)
        {
            InitializeComponent();
            getImg();
            getInfo(product);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSubstract_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void getInfo(Product product)
        {
            this.product = product;
            txtProductName.Text = product.GetName();
            txtNutrition.Text = product.GetIngredients();
        }
        private void getImg() 
        {
            string imagePath = db.picturesUrl + product.GetID() + ".jpeg";
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath);
            bitmap.EndInit();
            pictureProduct.Source = bitmap;
        }
    }
}
