using Delta_Coop365;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Delta_Coop365
{
    internal class DbAccessor
    {
        /// <summary>
        /// This class main focus is handling the database
        /// connString variable is the source to the database(our case being local)
        /// </summary>
        string connString;
        string picturesUrl;
        public DbAccessor(string connString) { this.connString = connString; this.picturesUrl = "C:\\Users\\danie\\source\\repos\\ConsoleApp2\\ConsoleApp2\\productPictures\\"; }
        public DbAccessor(string connString, string picturesUrl) { this.connString = connString; this.picturesUrl = picturesUrl; }
        /// <summary>
        /// This functions is made to check if database is populated with data. 
        /// This controls if we have to created the data or just update it.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>Boolean value true if the database is populated false if not</returns>
        public bool isDatabasePopulated(string tableName)
        {
            string query = "SELECT COUNT(*) FROM " + tableName;
            bool returnBool = false;
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int count = (int)command.ExecuteScalar();
                    if (count == 0)
                    {
                        Console.WriteLine("The table is empty.");
                    }
                    else
                    {
                        returnBool = true;
                        Console.WriteLine($"The table has {count} rows.");
                    }
                }
                connection.Close();
            }
            return returnBool;
        }
        /// <summary>
        /// Inserts object data of Products into the database.
        /// </summary>
        /// <param name="productid"></param>
        /// <param name="name"></param>
        /// <param name="ingredients"></param>
        /// <param name="price"></param>
        public void insertIntoProducts(int productid, string name, string ingredients, double price)
        {
            string pictureUrl = this.picturesUrl + productid.ToString();
            string query = "INSERT INTO Products (ProductID, ProductName, Price, Description, Url) VALUES ('" + productid + "','" + name + "','" + price + "','" + ingredients + "','" + pictureUrl + "')";
            sqlQuery(query);
        }
        /// <summary>
        /// Updates the stock of the product with the given ProductID
        /// </summary>
        /// <param name="productid"></param>
        /// <param name="stock"></param>
        public void updateStock(int productid, int stock)
        {
            string query = "UPDATE Products SET Stock = " + stock + " WHERE ProductID = " + productid;
            sqlQuery(query);
        }
        /// <summary>
        /// This function updates all the products with the newest data from the file.
        /// </summary>
        public void updateProductsDaily()
        {
            DataStream data = new DataStream("https://coop365.junoeuro.dk/api/Coop365/BakeOffVare");
            var results = data.getData("BakeOffVare");
            foreach (var element in results)
            {
                int productid = (int)element.Element("Varenummer");
                double price = (double)element.Element("Pris");
                updateProduct(productid, price);
                Console.WriteLine("____________________");
                Console.WriteLine("Product with the id: " + productid + " has been updated with the price: " + price);

            }

        }
        /// <summary>
        /// Updates every unique product id with a new price
        /// Includes queryStatement and calls the queryFunction
        /// </summary>
        /// <param name="productid"></param>
        /// <param name="price"></param>
        public void updateProduct(int productid, double price)
        {
            string query = "UPDATE Products SET Price = " + price + " WHERE ProductID = " + productid;
            sqlQuery(query);

        }
        /// <summary>
        /// Main query function that takes the string query as a param.
        /// </summary>
        /// <param name="query"></param>
        public void sqlQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"Rows affected: {rowsAffected}");
                    command.Connection.Close();
                }
            }
        }
    }
}
