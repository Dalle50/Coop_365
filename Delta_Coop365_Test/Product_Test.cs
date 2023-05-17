using Delta_Coop365;
using System.Windows.Media.Imaging;

namespace Delta_Coop365_Test
{
    public class Product_Test
    {
        public Product p = new Product(1, "Kanelsnegl"
            , 20, 200, 
            "Mel, vand, eddike", 
            "C:\\Users\\danie\\source\\repos\\Delta_Coop365\\Delta_Coop365\\ProductPictures\\1007.jpeg");
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_Constructor()
        {
            var tempProduct = p;
            //ASSERT
            Assert.IsNotNull(p);
            Assert.That(tempProduct, Is.EqualTo(p));
        }
        [Test]
        public void Test_GetName()
        {
            string name = p.GetName();
            //ASSERT
            Assert.That("Kanelsnegl", Is.EqualTo(name));
        }
        [Test]
        public void Test_GetID()
        {
            int id = p.GetID();
            //ASSERT
            Assert.That(1, Is.EqualTo(id));
        }
        [Test]
        public void Test_GetStock()
        {
            int stock = p.GetStock();
            //ASSERT
            Assert.That(20, Is.EqualTo(stock));
        }
        [Test]
        public void Test_GetPrice()
        {
            double price = p.GetPrice();
            //ASSERT
            Assert.That(200, Is.EqualTo(price));
        }
        [Test]
        public void Test_GetIngredients()
        {
            string ingredients = p.GetIngredients();
            //ASSERT
            Assert.That("Mel, vand, eddike", Is.EqualTo(ingredients));
        }
        [Test]
        public void Test_Makebitmap()
        {
            BitmapImage image = new BitmapImage(new Uri("C:\\Users\\danie\\source\\repos\\Delta_Coop365\\Delta_Coop365\\ProductPictures\\1007.jpeg"));
            //ASSERT
            Assert.That(p.imgPath.UriSource, Is.EqualTo(image.UriSource));
        }
    }
}