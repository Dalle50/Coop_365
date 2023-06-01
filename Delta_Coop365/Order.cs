using System;
using System.Collections.Generic;

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
            /// Implement the code to calculate the total price
            /// remember that you can take the price from OrderLines, since the total amount of products price are already added up on the OrderLine
            return TotalPrice;
        }

        public int ProductsLeft(OrderLine orderLine)
        {
            Product product = orderLine.GetProduct();
            int productsLeft = product.GetStock() - orderLine.GetAmount(); // Implement the actual calculation of the productsleft after order made
            if (productsLeft == 0) Notify(product);

            return productsLeft;
        }

        public void Notify(Product product)
        {
            /// Implement code that sends a notice to bakery and store administration
            /// when the bakeoff has an item that's sold out
            EmailService emailService = new EmailService();
            string pathToOrderReciept = DbAccessor.GetSolutionPath() + "\\Receipts\\" + OrderID + ".pdf";
            emailService.SendNotice("daniel.htc.jacobsen@gmail.com", product.productName + " is sold out.", "The product " + product.productName
                + "has been sold out at: " + DateTime.Now + "\n Attached is the order, that has sold out the item.", new string[] { pathToOrderReciept });
        }

        public void AddOrderLine(OrderLine orderLine)
        {
            orderLines.Add(orderLine);
        }

        public void DeleteOrderLine(OrderLine orderLine)
        {
            orderLines.Remove(orderLine);
        }

        public void UpdateOrderLine(int productID, int amount)
        {
            var orderLine = orderLines.Find(order => order.GetProduct().GetID() == productID);
            orderLine.SetAmount(amount);
        }
        public void UpdateTotalPrice()
        {
            double total = 0.0;

            foreach (OrderLine line in orderLines)
            {
                total += (double)line.GetProduct().GetPrice() * line.GetAmount();
            }

            TotalPrice = total;
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
