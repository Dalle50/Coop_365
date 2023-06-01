using System;
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
        public BitmapImage imagePath { get; set; }

        /// <summary>
        /// The values tied to the Product such as name, id, stock, price and nutritional information.
        /// Below is the constructor.
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="productName"></param>
        /// <param name="stock"></param>
        /// <param name="price"></param>
        /// <param name="ingredients"></param>

        public Product(int productID, string productName, int stock, double price, string ingredients, string imagePath)
        {
            this.productID = productID;
            this.productName = productName;
            this.stock = stock;
            this.price = price;
            this.ingredients = ingredients;
            this.imagePath = MakeBitmapImage(imagePath);
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

        public void SetStock(int newStock)
        {
            stock = newStock;
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

        public BitmapImage MakeBitmapImage(string path)
        {
            string imagePath = path;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath);
            Console.WriteLine(path);
            bitmap.EndInit();
            return bitmap;
        }

        public string Price
        {
            get { return "Pris: " + GetPrice().ToString() + " kr."; }
        }
    }
}
