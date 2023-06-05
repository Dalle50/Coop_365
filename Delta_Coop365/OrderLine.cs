using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Delta_Coop365
{
    /// <summary>
    /// [ Author: Pernille ]
    /// </summary>
    public class OrderLine
    {
        private Product product;
        public int amount { get; set; }
        private DateTime date;

        public OrderLine(Product product, int amount, DateTime date)
        {
            this.product= product;
            this.amount= amount;
            this.date = date;
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

        public DateTime GetDate()
        {
            return date;
        }
        public DateTime SetDate(DateTime date)
        {
            return date;
        }
        //----- These are needed to return the various products that you find in CheckOut.xaml.
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
