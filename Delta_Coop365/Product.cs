using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Delta_Coop365
{
    /// <summary>
    ///  Model class for Product.
    ///  - contains all the things needed for the Product object.
    /// </summary>
    public class Product : IBakeOff
    {
        public string productName { get; set; }
        private int productID;
        private int stock;
        public double price { get; set; }
        private string ingredients;
        public BitmapImage imgPath { get; set; }


        /// <summary>
        /// The values tied to the Product such as name, id, stock, price and nutritional information.
        /// Below is the constructor.
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="productName"></param>
        /// <param name="stock"></param>
        /// <param name="price"></param>
        /// <param name="ingredients"></param>
        
        public Product(int productID, string productName, int stock, double price, string ingredients, string imgPath)
        {
            this.productID = productID;
            this.productName = productName;
            this.stock = stock;
            this.price = price;
            this.ingredients = ingredients;
            this.imgPath = Makebitmap(imgPath);
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

        public BitmapImage Makebitmap(string path)
        {
            string imgPath = path;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imgPath);
            Console.WriteLine(path);
            bitmap.EndInit();
            return bitmap;
        }
    }
}
