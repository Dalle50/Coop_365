using System;
using System.Collections.Generic;

namespace Delta_Coop365
{
    public class Report
    {
        private List<OrderLine> orders;
        private DateTime date;

        public Report(List<OrderLine> orders)
        {
            this.orders = orders;
            this.date = DateTime.Now;
        }
        public DateTime GetTimeStamp()
        {
            return date;
        }

        public void GeneratePDF()
        {
            /// Implement code that generates the PDF report
        }
    }
}
