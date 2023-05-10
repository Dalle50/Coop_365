using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta_Coop365
{
    public class Order : IBakeOff
    {
        private List<OrderLine> orderLines;
        private int OrderID;
        private double TotalPrice;
        private DbAccessor DBA;

        public Order()
        {
            orderLines = new List<OrderLine>();
            DBA = new DbAccessor();
        }


        public int GetID()
        {
            return OrderID;
        }

        public double GetPrice()
        {
            /// Implement the code to calculate the total price
            /// remember that you can take the price from OrderLines, since the total amount of products price are already added up on the OrderLine
            int counter = 0;
            while (counter < orderLines.Count)
            {
                TotalPrice += orderLines[counter].Getprice();
                counter++;
            }

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
    }
}
