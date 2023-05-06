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

        Product(string productName, int productID, int stock, double price , string ingredients)
        {
            this.productName = productName;
            this.productID = productID;
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

        public void GetIngredients() //Ændret fra string til void.
        {
            /// get ingredients from the XML file
            /// This should be constructed with the object, and not recieved by the xml file after object is constructed.
            SqlConnection connection = new SqlConnection();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT Description FROM Products WHERE ProductID = @productID", connection);
                command.Parameters.Add("@productID", SqlDbType.Int, GetID());
                SqlDataReader reader = command.ExecuteReader();
                //while (reader.Read())
                //{
                //    string ingredients = reader.GetString(0);
                //    Product product = new Product();
                //}
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n StackTrace: " + e.StackTrace);
            }
            finally 
            {
                connection.Close();
            }
        }
    }
}
