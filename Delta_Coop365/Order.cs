using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta_Coop365
{
    public class Order : IBakeOff
    {
        private List<OrderLine> orderLines = new List<OrderLine>();
        private int OrderID;
        private double TotalPrice;

        public int GetID()
        {
            return OrderID;
        }

        public double GetPrice()
        {
            /// Implement the code to calculate the total price
            /// remember that you need to get the price from the Product within the OrderLine
            /// but the amount from OrderLine itself
            return TotalPrice;
        }

        public int ProductsLeft(Product p)
        {
            int productsLeft = 0; // Implement the actual calculation of the productsleft after order made

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
            /// Adds the OrderLine to the database
            /// Use of SQL commands
        }

        public void DeleteOrderLine(OrderLine ol)
        {
            /// Removes the OrderLine from the database
            /// Use of SQL commands
        }

        public void UpdateOrderLine(OrderLine ol, int amount)
        {
            /// Updates the OrderLine in the Database
            /// takes the amount fromt he interface screen (i.e. TextBox.text)
            /// Use of SQL commands
        }
    }
}
