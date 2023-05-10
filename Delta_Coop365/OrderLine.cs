using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta_Coop365
{
    public class OrderLine
    {
        private Product product;
        private int amount;
        private DateTime date;




        public double Getprice()
        {
            double total = amount * product.GetPrice();
            return total;
        }
        
        public Product GetProduct()
        {
            return product; //.---
        }

        public int GetAmount()
        {
            /// WPF textblock? int32.Parse(TextBlock.text)
            return amount;
        }

        public void SetAmount(int amount)
        {
            this.amount = amount;
        }

        public DateTime GetDate() 
        {
            return DateTime.Today;
        }
    }
}
