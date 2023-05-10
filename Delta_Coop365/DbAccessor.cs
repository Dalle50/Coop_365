using Delta_Coop365;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Configuration;
using Microsoft.VisualStudio.Setup.Configuration;


namespace Delta_Coop365
{
    internal class DbAccessor
    {
        /// <summary>
        /// This class main focus is handling the database
        /// connString variable is the source to the database(our case being local)
        /// </summary>
        string connString;
        public string picturesUrl;
        /// <summary>
        /// "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\danie\\source\\repos\\Delta_Coop365\\Delta_Coop365\\Database1.mdf;Integrated Security=True"
        /// </summary>
        /// <param name="connString"></param>
        public DbAccessor() 
        {
            this.connString = ConfigurationManager.ConnectionStrings["post"].ConnectionString;
            this.picturesUrl = 
                GetSolutionPath() + "\\productPictures\\"; }
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
        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            string query = "Select * FROM Products";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Connection.Open();
                    SqlDataReader sqlReader = command.ExecuteReader();  

                    while (sqlReader.Read())
                    {
                        Product temp = new Product(Int32.Parse(sqlReader.GetValue(0).ToString()), sqlReader.GetValue(1).ToString(), 0, Double.Parse(sqlReader.GetValue(3).ToString()), sqlReader.GetValue(4).ToString());
                        products.Add(temp);
                    }

                    command.Connection.Close();
                }
            }
            return products;

        }
        /// <summary>
        /// Inserts object data of Products into the database.
        /// </summary>
        /// <param name="productid"></param>
        /// <param name="name"></param>
        /// <param name="ingredients"></param>
        /// <param name="price"></param>
        //public void insertIntoProducts(int productid, string name, string ingredients, double price)
        //{
        //    string pictureUrl = this.picturesUrl + productid.ToString();
        //    string query = "INSERT INTO Products (ProductID, ProductName, Price, Description, Url) VALUES ('" + productid + "','" + name + "','" + price + "','" + ingredients + "','" + pictureUrl + "')";
        //    sqlQuery(query);
        //}

        public void InsertIntoProducts(int productid, string name, string ingredients, double price)
        {
            string pictureUrl = this.picturesUrl + productid.ToString();

            string query = "INSERT INTO Products (ProductID, ProductName, Price, Description, Url) VALUES (@productid,@name,@price,@ingredients,@pictureUrl)";
            SqlParameter productIdParam = new SqlParameter("@productid", productid);
            SqlParameter nameParam = new SqlParameter("@name", name);
            SqlParameter priceParam = new SqlParameter("@price", price);
            SqlParameter ingredientsParam = new SqlParameter("@ingredients", ingredients);
            SqlParameter pictureUrlParam = new SqlParameter("@pictureUrl", pictureUrl);
            sqlQuery(query, productIdParam, nameParam, priceParam, ingredientsParam, pictureUrlParam);

        }


        /// <summary>
        /// Updates the stock of the product with the given ProductID
        /// </summary>
        /// <param name="productid"></param>
        /// <param name="stock"></param>
        public void updateStock(int productid, int stock)
        {
            SqlParameter productIdParam = new SqlParameter("@productid", productid);
            SqlParameter nameParam = new SqlParameter("@stock", stock);
            string query = "UPDATE Products SET Stock = @stock WHERE ProductID = @productid";
            sqlQuery(query, productIdParam);
        }
        /// <summary>
        /// This function updates all the products with the newest data from the file.
        /// </summary>
        public void updateProductsDaily(IEnumerable<XElement> results)
        {

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
            string query = "UPDATE Products SET Price = @price WHERE ProductID = @productid";
            SqlParameter priceParam = new SqlParameter("@productid", price);
            SqlParameter productIdParam = new SqlParameter("@price", productid);
            sqlQuery(query, priceParam, productIdParam);
        }
        public void sqlQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    command.Connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"Rows affected: {rowsAffected}");
                    command.Connection.Close();
                }
            }
        }
        /// <summary>
        /// Remove when finishing up
        /// </summary>
        /// <returns></returns>
        public static string GetSolutionPath1()
        {
            var query = new SetupConfiguration();
            var e = query.EnumAllInstances();

            int fetched;
            var instances = new ISetupInstance[1];
            e.Next(1, instances, out fetched);

            string visualStudioDirectory = instances[0].GetInstallationPath();
            string solutionDirectory = Path.Combine(visualStudioDirectory, "Delta_Coop365");

            string solutionFilePath = Path.Combine(solutionDirectory, "Delta_Coop365.sln");

            return solutionFilePath;
        }
        public static string GetSolutionPath()
        {
            // This will get the current PROJECT directory
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            return projectDirectory;
        }

    }
}
