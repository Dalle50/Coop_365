using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.IO;
using System.Configuration;


namespace Delta_Coop365
{
    internal class DbAccessor
    {
        /// <summary>
        /// This class main focus is handling the database
        /// The connectionString variable is the source to the database(our case being local)
        /// </summary>
        private string connectionString;
        public string picturesUrl;
        /// <summary>
        /// "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\danie\\source\\repos\\Delta_Coop365\\Delta_Coop365\\Database1.mdf;Integrated Security=True"
        /// </summary>
        /// <param name="connString"></param>
        public DbAccessor()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["post"].ConnectionString;
            this.picturesUrl = GetSolutionPath() + "\\productPictures\\";
        }
        /// <summary>
        /// This functions is made to check if database is populated with data. 
        /// This controls if we have to created the data or just update it.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>Boolean value true if the database is populated false if not</returns>
        public bool IsDatabasePopulated(string tableName)
        {
            string query = "SELECT COUNT(*) FROM " + tableName;
            bool returnBool = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Connection.Open();
                    SqlDataReader sqlReader = command.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        Product product = new Product(int.Parse(sqlReader.GetValue(0).ToString()), sqlReader.GetValue(1).ToString(), 0, double.Parse(sqlReader.GetValue(3).ToString()), sqlReader.GetValue(4).ToString(), sqlReader.GetValue(5).ToString());
                        products.Add(product);
                    }

                    command.Connection.Close();
                }
            }
            return products;

        }
        public Product GetProduct(int productId)
        {
            Product product = null;
            string query = "Select * FROM Products WHERE ProductID=@productId";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlParameter productIdParam = new SqlParameter("@productId", productId);
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Connection.Open();
                    SqlDataReader sqlReader = command.ExecuteReader();

                    if (sqlReader.Read())
                    {
                        product = new Product(Int32.Parse(sqlReader.GetValue(0).ToString()), sqlReader.GetValue(1).ToString(), Int32.Parse(sqlReader.GetValue(2).ToString()), Double.Parse(sqlReader.GetValue(3).ToString()), sqlReader.GetValue(4).ToString(), sqlReader.GetValue(5).ToString());
                    }

                    command.Connection.Close();
                }
            }
            return product;

        }
        /// <summary>
        /// Inserts object data of Products into the database.
        /// </summary>
        /// <param name="productid"></param>
        /// <param name="name"></param>
        /// <param name="ingredients"></param>
        /// <param name="price"></param>
        public void InsertIntoProducts(int productid, string name, string ingredients, double price)
        {
            string pictureUrl = this.picturesUrl + productid.ToString() + ".jpeg";

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
            string query = "UPDATE Products SET Stock = @stock WHERE ProductID = @productid";
            SqlParameter productIdParam = new SqlParameter("@productid", productid);
            SqlParameter stockParam = new SqlParameter("@stock", stock);
            sqlQuery(query, productIdParam, stockParam);
        }
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

        /// Order starts here
        public Order GetOrder(int OrderId)
        {
            Order tempOrder;
            string query = "Select FROM Orders WHERE OrderID = @orderId";
            SqlParameter orderIdParam = new SqlParameter("@orderId", OrderId);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(orderIdParam);
                    command.Connection.Open();

                    SqlDataReader sqlReader = command.ExecuteReader();

                    var result = sqlReader.Read();
                    tempOrder = new Order();
                    foreach (OrderLine ol in GetOrderLines(OrderId))
                    {
                        tempOrder.AddOrderLine(ol);
                    }
                    tempOrder.UpdateTotalPrice();
                    command.Connection.Close();
                }
            }
            return tempOrder;
        }
        public int InsertIntoOrders(double TotalPrice, DateTime date)
        {
            int OrderID;
            string query = "Insert INTO Orders(TotalPrice, Date) VALUES(@TotalPrice, @Date); SELECT CONVERT(int, SCOPE_IDENTITY()) as OrderID;";
            SqlParameter TotalPriceParam = new SqlParameter("@TotalPrice", TotalPrice);
            SqlParameter DateParam = new SqlParameter("@Date", date);
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(TotalPriceParam);
                    command.Parameters.Add(DateParam);
                    connection.Open();
                    OrderID = (int)command.ExecuteScalar();

                }
            }
            return OrderID;
        }
        public void InsertIntoOrderLines(int OrderID, OrderLine ol)
        {
            string query = "Insert INTO OrderLines(Amount, ProductId, OrderId, Date) VALUES(@Amount, @ProductId, @OrderId, @Date)";
            SqlParameter AmountParam = new SqlParameter("@Amount", ol.GetAmount());
            SqlParameter ProductIdParam = new SqlParameter("@ProductId", ol.GetProduct().GetID());
            SqlParameter OrderIdParam = new SqlParameter("@OrderId", OrderID);
            SqlParameter DateParam = new SqlParameter("@Date", ol.GetDate());
            sqlQuery(query, AmountParam, ProductIdParam, OrderIdParam, DateParam);
        }
        public void UpdateOrderLine(OrderLine ol, int OrderId)
        {
            string query = "UPDATE OrderLines SET Amount = @amount WHERE ProductID = @ProductId AND OrderID = @OrderId";
            SqlParameter AmountParam = new SqlParameter("@Amount", ol.GetAmount());
            SqlParameter ProductIdParam = new SqlParameter("@ProductId", ol.GetProduct().GetID());
            SqlParameter OrderIdParam = new SqlParameter("@OrderId", OrderId);
            sqlQuery(query, AmountParam, ProductIdParam, OrderIdParam);
        }
        public List<OrderLine> GetOrderLines(int OrderId)
        {
            List<OrderLine> tempOrderLines = new List<OrderLine>();
            string query = "Select * FROM OrderLines WHERE OrderID = @orderId";
            SqlParameter orderIdParam = new SqlParameter("@orderId", OrderId);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(orderIdParam);
                    command.Connection.Open();

                    SqlDataReader sqlReader = command.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        tempOrderLines.Add(new OrderLine(GetProduct(int.Parse(sqlReader.GetValue(2).ToString())),
                                                                        int.Parse(sqlReader.GetValue(1).ToString()),
                                                                            DateTime.Parse(sqlReader.GetValue(4).ToString())));
                    }
                    command.Connection.Close();
                }
            }
            return tempOrderLines;
        }
        public List<OrderLine> GetDailyOrderLines(DateTime date)
        {
            List<OrderLine> tempOrderLines = new List<OrderLine>();
            string query = "Select * FROM OrderLines WHERE Date = @Date";
            SqlParameter DateParam = new SqlParameter("@Date", date);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DateParam);
                    command.Connection.Open();
                    SqlDataReader sqlReader = command.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        if (DateTime.Parse(sqlReader.GetValue(4).ToString()) == date)
                        {
                            tempOrderLines.Add(new OrderLine(GetProduct(int.Parse(sqlReader.GetValue(2).ToString())),
                                                                            int.Parse(sqlReader.GetValue(1).ToString()),
                                                                                DateTime.Parse(sqlReader.GetValue(4).ToString())));
                        }
                    }
                    command.Connection.Close();
                }

            }
            return tempOrderLines;
        }
        /// </summary>
        /// <returns></returns>
        public static string GetSolutionPath()
        {
            // This will get the current PROJECT directory
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            return projectDirectory;
        }
        public void sqlQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
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

        internal object GetOrders()
        {
            throw new NotImplementedException();
        }
    }
}
