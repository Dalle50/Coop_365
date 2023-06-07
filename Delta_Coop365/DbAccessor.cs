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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Collections.ObjectModel;
using MailKit.Search;
using static QRCoder.PayloadGenerator;


namespace Delta_Coop365
{
    internal class DbAccessor
    {
        /// <summary>
        /// This class main focus is handling the database
        /// connString variable is the source to the database(our case being local)
        /// </summary>
        private string connString;
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
                        int productId = sqlReader.GetInt32(sqlReader.GetOrdinal("ProductID"));
                        string productName = sqlReader.GetString(sqlReader.GetOrdinal("ProductName"));
                        int stock;
                        if(sqlReader.IsDBNull(sqlReader.GetOrdinal("Stock")))
                        {
                            stock = 0;
                        }
                        else
                        {
                            stock = sqlReader.GetInt32(sqlReader.GetOrdinal("Stock"));
                        }
                        double price = sqlReader.GetDouble(sqlReader.GetOrdinal("Price"));
                        string ingredients = sqlReader.GetString(sqlReader.GetOrdinal("Description"));
                        string pathToPicture = sqlReader.GetString(sqlReader.GetOrdinal("Url"));
       
                        Product temp = new Product(productId, productName, stock, price, ingredients, pathToPicture);
                        products.Add(temp);
                    }

                    command.Connection.Close();
                }
            }
            return products;

        }
        public Product GetProduct(int productId)
        {
            Product tempProduct;
            string query = "Select * FROM Products WHERE ProductID=@productId";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlParameter productIdParam = new SqlParameter("@productId", productId);
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Connection.Open();
                    SqlDataReader sqlReader = command.ExecuteReader();
                    if (sqlReader.Read())
                    {
                        string productName = sqlReader.GetString(sqlReader.GetOrdinal("ProductName"));
                        int stock = sqlReader.GetInt32(sqlReader.GetOrdinal("Stock"));
                        double price = sqlReader.GetDouble(sqlReader.GetOrdinal("Price"));
                        string ingredients = sqlReader.GetString(sqlReader.GetOrdinal("Description"));
                        string pathToPicture = sqlReader.GetString(sqlReader.GetOrdinal("Url"));
                        tempProduct = new Product(productId, productName, stock, price, ingredients, pathToPicture);

                    }
                    else
                    {
                        tempProduct = null;
                    }
                    command.Connection.Close();
                }
            }
            return tempProduct;

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
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(orderIdParam);
                    command.Connection.Open();
                    
                    SqlDataReader sqlReader = command.ExecuteReader();

                    var result  = sqlReader.Read();
                    tempOrder = new Order();
                    foreach(OrderLine ol in GetOrderLines(OrderId)) 
                    { 
                        tempOrder.AddOrderLine(ol);
                    }
                    tempOrder.UpdateTotalPrice();
                    command.Connection.Close();
                }
            }
            return tempOrder;
        }
        public int InsertIntoOrders(double TotalPrice, DateTime date, int? KundeID = null)
        {
            int OrderID;
            string query;
            SqlParameter TotalPriceParam = new SqlParameter("@TotalPrice", TotalPrice);
            SqlParameter DateParam = new SqlParameter("@Date", date);

            if (KundeID.HasValue)
            {
                query = "INSERT INTO Orders(TotalPrice, Date, KundeID) VALUES(@TotalPrice, @Date, @KundeId); SELECT CONVERT(int, SCOPE_IDENTITY()) AS OrderID;";
                SqlParameter KundeIdParam = new SqlParameter("@KundeId", KundeID.Value);

                using (SqlConnection connection = new SqlConnection(this.connString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(TotalPriceParam);
                        command.Parameters.Add(DateParam);
                        command.Parameters.Add(KundeIdParam);
                        connection.Open();
                        OrderID = (int)command.ExecuteScalar();
                    }
                }
            }
            else
            {
                query = "INSERT INTO Orders(TotalPrice, Date) VALUES(@TotalPrice, @Date); SELECT CONVERT(int, SCOPE_IDENTITY()) AS OrderID;";

                using (SqlConnection connection = new SqlConnection(this.connString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(TotalPriceParam);
                        command.Parameters.Add(DateParam);
                        connection.Open();
                        OrderID = (int)command.ExecuteScalar();
                    }
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
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(orderIdParam);
                    command.Connection.Open();

                    SqlDataReader sqlReader = command.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        int ProductId = sqlReader.GetInt32(sqlReader.GetOrdinal("ProductID"));
                        int amount = sqlReader.GetInt32(sqlReader.GetOrdinal("Amount"));
                        DateTime orderLineDate = sqlReader.GetDateTime(sqlReader.GetOrdinal("Date"));
                        tempOrderLines.Add(new OrderLine(GetProduct(ProductId),
                                                                        amount,
                                                                            orderLineDate));
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
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DateParam);
                    command.Connection.Open();
                    SqlDataReader sqlReader = command.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        DateTime orderLineDate = sqlReader.GetDateTime(sqlReader.GetOrdinal("Date"));
                        if (orderLineDate == date)
                        {
                            int ProductId = sqlReader.GetInt32(sqlReader.GetOrdinal("ProductID"));
                            int amount = sqlReader.GetInt32(sqlReader.GetOrdinal("Amount"));

                            tempOrderLines.Add(new OrderLine(GetProduct(ProductId),
                                                                            amount,
                                                                                orderLineDate));
                        }
                    }
                    command.Connection.Close();
                }

            }
            return tempOrderLines;
        }
        public bool IsCustomerExisting(int phoneNumber)
        {
            bool customerExists;
            string query = "SELECT COUNT(*) FROM Kunder WHERE PhoneNumber = " + phoneNumber;
            SqlParameter PhoneNumberParam = new SqlParameter("@PhoneNumber", phoneNumber);
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(PhoneNumberParam);
                    command.Connection.Open();
                    var count = (int)command.ExecuteScalar();

                    if(count > 0)
                    {
                        customerExists = true;
                    }
                    else
                    {
                        customerExists = false;
                    }
                    command.Connection.Close();
                }
            }
            return customerExists;
        }
        /// CREATE, READ, UPDATE <summary>
        /// </summary>
        public void InsertIntoKunder(string name, string address, int zipCode, string city, string email, int phoneNumber, double coopPoints)
        {
            string query = "Insert INTO Kunder(Name, Address, Zipcode, City, Email, PhoneNumber, CoopPoints) VALUES(@Name, @Address, @Zipcode, @City, @Email, @PhoneNumber, @CoopPoints)";
            SqlParameter NameParam = new SqlParameter("@Name", name);
            SqlParameter AddressParam = new SqlParameter("@Address", address);
            SqlParameter ZipcodeParam = new SqlParameter("@Zipcode", zipCode);
            SqlParameter CityParam = new SqlParameter("@City", city);
            SqlParameter EmailParam = new SqlParameter("@Email", email);
            SqlParameter PhoneNumberParam = new SqlParameter("@PhoneNumber", phoneNumber);
            SqlParameter CoopPointsParam = new SqlParameter("@CoopPoints", coopPoints);
            sqlQuery(query, NameParam, AddressParam, ZipcodeParam, CityParam, EmailParam, PhoneNumberParam, CoopPointsParam);
        }

        public Customer GetCustomer(int phoneNumber)
        {
            Customer tempC = null;
            string query = "Select * FROM Kunder WHERE PhoneNumber = @phoneNumber";
            SqlParameter phoneNumberParam = new SqlParameter("@phoneNumber", phoneNumber);
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(phoneNumberParam);
                    command.Connection.Open();

                    SqlDataReader sqlReader = command.ExecuteReader();

                    if (sqlReader.Read())
                    {

                        int KundeId = sqlReader.GetInt32(sqlReader.GetOrdinal("KundeID"));
                        string name = sqlReader.GetString(sqlReader.GetOrdinal("Name"));
                        string address = sqlReader.GetString(sqlReader.GetOrdinal("Address"));
                        int zipCode = sqlReader.GetInt32(sqlReader.GetOrdinal("zipCode"));
                        string city = sqlReader.GetString(sqlReader.GetOrdinal("City"));
                        string email = sqlReader.GetString(sqlReader.GetOrdinal("Email"));
                        double coopPoints = sqlReader.GetDouble(sqlReader.GetOrdinal("CoopPoints"));

                        tempC = new Customer(KundeId,name, address, zipCode, city, email, phoneNumber, coopPoints) ;
                    }
                    command.Connection.Close();
                }
            }
            return tempC;
        }
        public Customer GetCustomerById(int KundeId)
        {
            Customer tempC = null;
            string query = "Select * FROM Kunder WHERE KundeID = @KundeId";
            SqlParameter kundeIdParam = new SqlParameter("@KundeId", KundeId);
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(kundeIdParam);
                    command.Connection.Open();

                    SqlDataReader sqlReader = command.ExecuteReader();

                    if (sqlReader.Read())
                    {

                        int id = sqlReader.GetInt32(sqlReader.GetOrdinal("KundeID"));
                        string name = sqlReader.GetString(sqlReader.GetOrdinal("Name"));
                        string address = sqlReader.GetString(sqlReader.GetOrdinal("Address"));
                        int zipCode = sqlReader.GetInt32(sqlReader.GetOrdinal("zipCode"));
                        int phoneNumber = sqlReader.GetInt32(sqlReader.GetOrdinal("PhoneNumber"));
                        string city = sqlReader.GetString(sqlReader.GetOrdinal("City"));
                        string email = sqlReader.GetString(sqlReader.GetOrdinal("Email"));
                        double coopPoints = sqlReader.GetDouble(sqlReader.GetOrdinal("CoopPoints"));

                        tempC = new Customer(id, name, address, zipCode, city, email, phoneNumber, coopPoints);
                    }
                    command.Connection.Close();
                }
            }
            return tempC;
        }

        public void UpdateCustomer(Customer c, int phoneNumber)
        {
            string query = "UPDATE Kunder SET (Name, Address, Zipcode, City, Email, PhoneNumber) VALUES(@Name,@Address,@Zipcode,@City,@Email,@PhoneNumber) WHERE PhoneNumber = @oldPhoneNumber";
            SqlParameter nameParam = new SqlParameter("@Name", c.name);
            SqlParameter addressParam = new SqlParameter("@Address", c.address);
            SqlParameter zipCodeParam = new SqlParameter("@Zipcode", c.zipCode);
            SqlParameter cityParam = new SqlParameter("@City", c.city);
            SqlParameter emailParam = new SqlParameter("@Email", c.email);
            SqlParameter phoneNumberParam = new SqlParameter("@PhoneNumber", c.phoneNumber);
            SqlParameter oldPhoneNumberParam = new SqlParameter("@oldPhoneNumber", phoneNumber);
            sqlQuery(query, nameParam, addressParam, zipCodeParam, cityParam, emailParam, phoneNumberParam, oldPhoneNumberParam);
        }
        public void UpdateCustomerPoints(int phoneNumber, double points)
        {
            string query = "UPDATE Kunder SET CoopPoints = @points WHERE PhoneNumber = @phoneNumber";
            SqlParameter phoneNumberParam = new SqlParameter("@phoneNumber", phoneNumber);
            SqlParameter pointsParam = new SqlParameter("@points", points);
            sqlQuery(query, phoneNumberParam, pointsParam);
        }

        public double GetCustomerPoints(int phoneNumber)
        {
            double coopPoints = 0.0;
            string query = "SELECT CoopPoints FROM Kunder WHERE PhoneNumber = @PhoneNumber";
            SqlParameter phoneNumberParam = new SqlParameter("@PhoneNumber", phoneNumber);
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(phoneNumberParam);
                    command.Connection.Open();
                    SqlDataReader sqlReader = command.ExecuteReader();

                    if (sqlReader.Read())
                    {
                        coopPoints = sqlReader.GetDouble(sqlReader.GetOrdinal("CoopPoints"));
                    }

                    command.Connection.Close();
                }
            }
            return coopPoints;
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
    }
}
