using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Delta_Coop365
{
    internal class TestData
    {
        /// <summary>
        /// [Author] Daniel
        /// Generates test data for visualisation purpose
        /// </summary>
        DbAccessor dbAccessor = new DbAccessor();
        DateTime date = DateTime.Now;

        public void GenerateTestData()
        {
            GenerateCustomers();
            GenerateOrdersAndOrderLines();
            
        }
        public void GenerateOrdersAndOrderLines()
        {
            List<Product> products = dbAccessor.GetProducts();
            Order o1 = new Order();
            o1.AddOrderLine(new OrderLine(products[0], 1, date));
            o1.AddOrderLine(new OrderLine(products[3], 1, date));
            o1.AddOrderLine(new OrderLine(products[5], 2, date));
            o1.SetCustomerId(100000);
            Order o2 = new Order();
            o2.AddOrderLine(new OrderLine(products[4], 6, date));
            o2.SetCustomerId(100001);
            Order o3 = new Order();
            o3.AddOrderLine(new OrderLine(products[2], 1, date));
            o3.AddOrderLine(new OrderLine(products[8], 2, date));
            o3.AddOrderLine(new OrderLine(products[3], 1, date));
            o3.SetCustomerId(100002);
            Order o4 = new Order();
            o4.AddOrderLine(new OrderLine(products[10], 1, date));
            o4.AddOrderLine(new OrderLine(products[11], 2, date));
            o4.SetCustomerId(100003);
            Order o5 = new Order();
            o5.AddOrderLine(new OrderLine(products[10], 1, date));
            o5.SetCustomerId(100004);
            Order o6 = new Order();
            o6.AddOrderLine(new OrderLine(products[1], 3, date));
            o6.AddOrderLine(new OrderLine(products[2], 1, date));
            o6.AddOrderLine(new OrderLine(products[3], 2, date));
            o6.SetCustomerId(100005);
            Order o7 = new Order();
            o7.AddOrderLine(new OrderLine(products[12], 3, date));
            o7.AddOrderLine(new OrderLine(products[2], 1, date));
            o7.AddOrderLine(new OrderLine(products[1], 2, date));
            o7.SetCustomerId(100006);
            Order o8 = new Order();
            o8.AddOrderLine(new OrderLine(products[2], 3, date));
            o8.AddOrderLine(new OrderLine(products[5], 1, date));
            o8.AddOrderLine(new OrderLine(products[6], 2, date));
            o8.SetCustomerId(100007);
            PerformInsertIntoOrders(new Order[] {o1,o2,o3,o4,o5,o6,o7,o8});
            
        }
        public void GenerateCustomers()
        {
            dbAccessor.InsertIntoKunder("Jens Hansen", "Møllevej 22", 8800, "Viborg", "jhansen@yahoo.dk", 33421122, 0);
            dbAccessor.InsertIntoKunder("Bent Ingvarsen", "Julevej 1", 2000, "Frederiksberg", "bingo@outlook.dk", 12332442, 0);
            dbAccessor.InsertIntoKunder("Gertrud Sand", "Dank 11", 8600, "Silkeborg", "julekalender@yahoo.dk", 88221133, 0);
            dbAccessor.InsertIntoKunder("Peter Nielsen", "Skovvej 8", 8000, "Aarhus", "pnielsen@gmail.com", 99887766, 0);
            dbAccessor.InsertIntoKunder("Lise Jensen", "Bakkevej 5", 2000, "Frederiksberg", "ljensen@hotmail.com", 11223344, 0);
            dbAccessor.InsertIntoKunder("Morten Andersen", "Søndergade 15", 5000, "Odense", "mandersen@live.dk", 55667788, 0);
            dbAccessor.InsertIntoKunder("Karen Pedersen", "Nørregade 3", 9000, "Aalborg", "kpedersen@yahoo.dk", 88990011, 0);
            dbAccessor.InsertIntoKunder("Thomas Møller", "Vestergade 12", 6700, "Esbjerg", "tmoller@outlook.dk", 22334455, 0);
            dbAccessor.InsertIntoKunder("Sofie Larsen", "Østergade 6", 8900, "Randers", "slarsen@gmail.com", 66778899, 0);
            dbAccessor.InsertIntoKunder("Michael Christensen", "Kirkevej 9", 6000, "Kolding", "mchristensen@hotmail.com", 44556677, 0);
        }
        public void PerformInsertIntoOrders(Order[] orders)
        {
            foreach(Order order in orders)
            {
                int orderId = dbAccessor.InsertIntoOrders(order.GetPrice(), date, order.GetCustomerId());
                PerformInsertIntoOrderLines(order.GetOrderLines(),orderId);
                Customer customer = dbAccessor.GetCustomerById(order.GetCustomerId());
                double pointsToAdd = CheckOut.ConvertItemsToPoints(order.GetOrderLines());
                dbAccessor.UpdateCustomerPoints(customer.phoneNumber, customer.coopPoints + pointsToAdd);
            }
        }
        public void PerformInsertIntoOrderLines(List<OrderLine> orderLines, int OrderId)
        {
            foreach(OrderLine ol in orderLines)
            {
                dbAccessor.InsertIntoOrderLines(OrderId, ol);
            }
        }

    }
}
