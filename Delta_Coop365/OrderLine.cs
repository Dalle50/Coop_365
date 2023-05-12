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
        private double price;

        public OrderLine(Product product, int amount, DateTime date)
        {
            this.product= product;
            this.amount= amount;
            this.date = date;
        }



        public double GetPrice()
        {
            double total = amount * product.GetPrice();
            price = total;
            return price;
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
            GetPrice();
        }
    }
}
