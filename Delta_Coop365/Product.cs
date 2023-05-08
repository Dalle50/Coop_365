using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Delta_Coop365
{
    public class Product : IBakeOff
    {
        private string productName;
        private int productID;
        private int stock;
        private double price;
        private string ingredients;

        Product( int productID, string productName, int stock, double price , string ingredients)
        {
            this.productID = productID;
            this.productName = productName;
            this.stock = stock;
            this.price = price;
            this.ingredients = ingredients;
        }

        public string GetName()
        {
            return productName;
        }

        public int GetID()
        {
            return productID;
        }

        public int GetStock()
        {
            return stock;
        }

        public double GetPrice()
        {
            return price;
        }

        public string GetIngredients() //Ændret fra string til void. 
        {
            /// get ingredients from the XML file
            /// This should be constructed with the object, and not recieved by the xml file after object is constructed.
           return ingredients;
        }
    }
}
