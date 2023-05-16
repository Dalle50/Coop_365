using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta_Coop365
{
    public class Order : IBakeOff
    {
        public List<OrderLine> orderLines;
        private int OrderID;
        private double TotalPrice;

        public Order()
        {
            orderLines = new List<OrderLine>();
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
            return TotalPrice;
        }

        public int ProductsLeft(OrderLine ol)
        {
            Product p = ol.GetProduct();
            int productsLeft = p.GetStock() - ol.GetAmount(); // Implement the actual calculation of the productsleft after order made
            if (productsLeft == 0) Notify(p);

            return productsLeft;
        }

        public void Notify(Product p)
        {
            /// Implement code that sends a notice to bakery and store administration
            /// when the bakeoff has an item that's sold out
            Email e = new Email();
            e.SendNotice(p);
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
        public void UpdateTotalPrice()
        {
            double tempTotal = 0.0;
            foreach (OrderLine line in orderLines)
            {
                tempTotal += (line.GetProduct().GetPrice() * line.GetAmount());
            }
            TotalPrice = tempTotal;
        }
        public List<OrderLine> GetOrderLines() 
        {
            return orderLines;
        }
        public void ClearOrderLines()
        {
            orderLines.Clear();
        }
    }
}
