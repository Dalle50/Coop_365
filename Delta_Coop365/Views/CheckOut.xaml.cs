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
    /// Interaction logic for CheckOut.xaml
    /// </summary>
    public partial class CheckOut : Window
    {
        DbAccessor db;
        Product product;
        public CheckOut()
        {

        }

        private void removeItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSubstract_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
        private void getOrders()
        {
            //foreach (var item in /*collection???????*/)
            //{
                StackPanel itemPanel = new StackPanel();

                string imagePath = db.picturesUrl + product.GetID() + ".jpeg";
                Image itemImage = new Image();
                itemImage.Source = new BitmapImage(new Uri(imagePath));
                itemImage.Width = 100;
                itemImage.Height = 100;
                itemPanel.Children.Add(itemImage);

                TextBlock itemName = new TextBlock();
                itemName.Text = item.Name;
                itemName.FontSize = 14;
                itemName.Margin = new Thickness(10, 0, 0, 0);
                itemPanel.Children.Add(itemName);

                TextBlock itemPrice = new TextBlock();
                itemPrice.Text = item.Price.ToString();
                itemPrice.FontSize = 14;
                itemPrice.Margin = new Thickness(10, 0, 0, 0);
                itemPanel.Children.Add(itemPrice);
            //}
        }
    }
}
