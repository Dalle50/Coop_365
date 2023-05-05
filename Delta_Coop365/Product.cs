using System;
using System.Collections.Generic;
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
        //private string ingredients;

        Product(string productName, int productID, int stock, double price /*, string ingredients*/)
        {
            this.productName = productName;
            this.productID = productID;
            this.stock = stock;
            this.price = price;
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

        public void GetIngredients() //Ændret fra string til void.
        {
            /// get ingredients from the XML file
            /// should it be returned as an array of ingredients instead?
            //ingredients = String.Format();???

            try
            {
                SqlConnection myConnection = new SqlConnection("user id=" + UserID + "; password=" + Password + "; server=" + Server + "; database=" + Database + ";");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n StackTrace: " + e.StackTrace);
            }
            finally 
            { 

            }
        }
    }
}
