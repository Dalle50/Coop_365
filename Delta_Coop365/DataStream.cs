using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Delta_Coop365
{
    internal class DataStream
    {
        private string api;
        public DataStream(string api) { this.api = api; }

        public IEnumerable<XElement> getData(string elementNames)
        {
            XDocument document = XDocument.Load(this.api);
            IEnumerable<XElement> results = document.Descendants(elementNames);
            foreach (var element in results)
            {
                int productid = (int)element.Element("Varenummer");
                string name = (string)element.Element("Name");
                string ingredients = (string)element.Element("Ingredience");
                double price = (double)element.Element("Pris");
                Console.WriteLine("____________________");
                Console.WriteLine(productid);
                Console.WriteLine(name);
                Console.WriteLine(ingredients);
                Console.WriteLine(price);
            }
            return results;
        }
    }

}
