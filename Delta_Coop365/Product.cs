using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta_Coop365
{
    public class Product
    {
        private string productName;
        private int productID;
        private int stock;
        private double price;

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
            /// get ingredients from the XML file
            /// SQL call, get the ingredients from the database
            return;
        }
    }
}
