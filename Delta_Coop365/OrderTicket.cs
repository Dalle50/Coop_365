using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta_Coop365
{
    public class OrderTicket
    {
        private Order order;
        private DateTime date;

        public DateTime GetDate()
        {
            return DateTime.Today;
        }

        public void Print()
        {
            /// Implement code that prints the order ticket
            /// Use of SQL commands
            /// Do we make a get command for the order?
        }
    }
}
