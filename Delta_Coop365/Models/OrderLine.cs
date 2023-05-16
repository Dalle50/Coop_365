using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Delta_Coop365
{
    public class OrderLine
    {
        private Product product;
        public int amount { get; set; }

        public OrderLine(Product product, int amount)
        {
            this.product= product;
            this.amount= amount;
        }
        
        public Product GetProduct()
        {
            return product;
        }

        public int GetAmount()
        {
            return amount;
        }

        public void SetAmount(int amount)
        {
            this.amount = amount;

        }

        //----- These are needed to return the bindings for the UI in checkout.
        public BitmapImage ImageUrl
        {
            get { return GetProduct().imgPath; }
        }
        public string productName
        {
            get { return GetProduct().productName; }
        }
        public double price
        {
            get { return GetProduct().price; }
        }
    }
}
