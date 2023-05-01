using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta_Coop365
{
    public class Report
    {
        private List<OrderLine> orders;
        private DateTime date;

        public DateTime GetTimeStamp()
        {
            return DateTime.Now;
        }
        
        public void GeneratePDF()
        {
            /// Implement code that generates the PDF report
        }
    }
}
