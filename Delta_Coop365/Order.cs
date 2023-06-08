using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta_Coop365
{
    public class Order
    {
        public List<OrderLine> orderLines;
        private int OrderID;
        private double TotalPrice;
        private double discount = 0.0;
        private int CustomerId;
        public Order()
        {
            orderLines = new List<OrderLine>();
            TotalPrice = 0.0;
        }

        public void SetId(int OrderID)
        {
            this.OrderID = OrderID;
        }
        public int GetID()
        {
            return OrderID;
        }

        public double GetPrice()
        {
            UpdateTotalPrice();
            return TotalPrice;
        }

        public int ProductsLeft(OrderLine ol)
        {
            Product p = ol.GetProduct();
            int productsLeft = p.GetStock() - ol.GetAmount(); // Implement the actual calculation of the productsleft after order made
            if (productsLeft == 0) Notify(p);

            return productsLeft;
        }
        /// <summary>
        /// [Author] Daniel
        /// </summary>
        /// <param name="p"></param>
        public void Notify(Product p)
        {
            Email e = new Email();
            string pathToOrderReciept = DbAccessor.GetSolutionPath() + "\\Receipts\\" + OrderID + ".pdf";
            e.SendNotice("daniel.htc.jacobsen@gmail.com", p.productName + " is sold out at the time: " + DateTime.Now.ToString(), "The product " + p.productName
                + "has been sold out at: " + DateTime.Now + "\n Attached is the order, that has sold out the item.", new string[] { } );
        }
        
        public void AddOrderLine(OrderLine ol)
        {
            /// Adds the OrderLine to the list
            orderLines.Add(ol);
        }

        public void DeleteOrderLine(OrderLine ol)
        {
            /// Removes the OrderLine from the list
            orderLines.Remove(ol);
        }

        public void UpdateOrderLine(int productID, int amount)
        {
            var ol = orderLines.Find(o => o.GetProduct().GetID() == productID);
            ol.SetAmount(amount);
        }
        /// <summary>
        /// [Author] Daniel
        /// </summary>
        public void UpdateTotalPrice()
        {
            double tempTotal = 0.0;
            foreach (OrderLine line in orderLines)
            {
                double total = (double) line.GetProduct().GetPrice() * line.GetAmount();
                tempTotal += total;
            }
            this.TotalPrice = tempTotal - discount;
        }
        /// <summary>
        /// [Author] Daniel
        /// </summary>
        /// <param name="amount"></param>
        public void AddDiscount(double amount)
        {
            this.discount = amount;
            UpdateTotalPrice();
        }
        public List<OrderLine> GetOrderLines()
        {
            return orderLines;
        }
        public void ClearOrderLines()
        {
            orderLines.Clear();
        }
        /// <summary>
        /// [Author] Daniel
        /// </summary>
        /// <param name="KundeId"></param>
        public void SetCustomerId(int KundeId)
        {
            CustomerId = KundeId;
        }
        /// <summary>
        /// [Author] Daniel
        /// </summary>
        /// <returns></returns>
        public int GetCustomerId()
        {
            return CustomerId;
        }
    }
}
