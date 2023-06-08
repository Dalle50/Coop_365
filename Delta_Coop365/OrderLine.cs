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
        /// <summary>
        /// This class is overall the link between Order and Product.
        /// It holds the products and the amount of each product etc.
        /// and more or less readys the information that Order needs.
        /// </summary>
        private Product product;
        public int amount { get; set; }
        private DateTime date;

        /// <summary>
        /// Constructor for OrderLine, it takes a product, a name and a date
        /// </summary>
        /// <param name="product"></param>
        /// <param name="amount"></param>
        /// <param name="date"></param>
        public OrderLine(Product product, int amount, DateTime date)
        {
            this.product= product;
            this.amount= amount;
            this.date = date;
        }
        /// <summary>
        /// Method for retriving the product
        /// </summary>
        /// <returns></returns>
        
        public Product GetProduct()
        {
            return product;
        }
        /// <summary>
        /// Method for retrieving the amount of products that is added to the cart.
        /// </summary>
        /// <returns></returns>

        public int GetAmount()
        {
            return amount;
        }
        /// <summary>
        /// Method for setting the amount of products in the given context.
        /// </summary>
        /// <param name="amount"></param>

        public void SetAmount(int amount)
        {
            this.amount = amount;

        }
        /// <summary>
        /// Methods for retrieving and setting the date.
        /// </summary>
        /// <returns></returns>

        public DateTime GetDate()
        {
            return date;
        }
        public DateTime SetDate(DateTime date)
        {
            return date;
        }
        /// <summary>
        /// These are needed to return the various information that that you find in CheckOut.xaml
        /// it returns the values to the Bindings in the UI window. It makes everything work smoothly and allows it to be dynamic.
        /// </summary>
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
