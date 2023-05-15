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
        Product product;
        public ViewingProduct(Product p)
        {
            InitializeComponent();
            product = p;
            getImg();
            getInfo(p);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            int temp = Int32.Parse(txtAmount.Text);
            temp++;
            txtAmount.Text = temp.ToString();
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

        }
        private void getInfo(Product product)
        {
            txtProductName.Text = product.GetName();
            txtNutrition.Text = product.GetIngredients();
        }
        private void getImg() 
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = product.imgPath.UriSource;
            bitmap.EndInit();
            image.Source = bitmap;
        }
    }
}
