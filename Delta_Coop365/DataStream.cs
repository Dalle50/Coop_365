using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Delta_Coop365
{
    /// <summary>
    /// [Author] Daniel
    /// </summary>
    internal class DataStream
    {
        private string api;
        public DataStream(string api) { this.api = api; }

        /// <summary>
        /// Loads XAML document from the given api url
        /// Takes descandants which is objects categorised by names
        /// Returns a IEnumerable of the type XELement which is the object we want from the XAML file 
        /// </summary>
        /// <param name="elementNames"></param>
        /// <returns></returns>
        public IEnumerable<XElement> getData(string elementNames)
        {
            XDocument document = XDocument.Load(this.api);
            IEnumerable<XElement> results = document.Descendants(elementNames);
            // Looping through the elements to visualize in console window that we're accessing it
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
