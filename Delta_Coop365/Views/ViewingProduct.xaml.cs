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
        DbAccessor dbAccessor;
        Product product;
        public ViewingProduct(Product product)
        {
            InitializeComponent();
            dbAccessor.picturesUrl + id + ".jpeg";
            this.product = product;
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
    }
}
