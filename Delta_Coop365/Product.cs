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

        public string GetIngredients()
        {
           return ingredients;
        }
    }
}
