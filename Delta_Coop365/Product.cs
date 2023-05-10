using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Delta_Coop365
{
    /// <summary>
    ///  Model class for Product.
    ///  - contains all the things needed for the Product object.
    /// </summary>
    public class Product : IBakeOff
    {
        private string productName;
        private int productID;
        private int stock;
        private double price;
        private string ingredients;

        /// <summary>
        /// The values tied to the Product such as name, id, stock, price and nutritional information.
        /// Below is the constructor.
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="productName"></param>
        /// <param name="stock"></param>
        /// <param name="price"></param>
        /// <param name="ingredients"></param>
        
        public Product(int productID, string productName, int stock, double price, string ingredients)
        {
            this.productID = productID;
            this.productName = productName;
            this.stock = stock;
            this.price = price;
            this.ingredients = ingredients;
        }
        /// <summary>
        /// Returns the name of the Product.
        /// </summary>
        public string GetName()
        {
            return productName;
        }
        /// <summary>
        /// Returns the ID of the Product.
        /// </summary>
        public int GetID()
        {
            return productID;
        }
        /// <summary>
        /// Returns the number of the given Product left.
        /// </summary>
        public int GetStock()
        {
            return stock;
        }
        /// <summary>
        /// Returns the Price of the Product
        /// </summary>
        public double GetPrice()
        {
            return price;
        }
        /// <summary>
        /// Returns the nutritional information about the Product.
        /// </summary>
        public string GetIngredients()
        {
           return ingredients;
        }
    }
}
