using Delta_Coop365;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using System.Windows.Media.Imaging;

namespace Delta_Coop365_Test
{
    public class Order_Test
    {
        public Order o = new Order();
        public OrderLine ol = new OrderLine(
                                    new Product(
                                    1,
                                        "snegl",
                                            2,
                                                20,
                                                    "mel",
                                                            "C:\\Users\\danie\\source\\repos\\Delta_Coop365\\Delta_Coop365\\ProductPictures\\1007.jpeg"),
                                                                2);
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void Test_Constructor()
        {
            Assert.IsNotNull(o);

            Assert.That(1, Is.EqualTo(o.GetOrderLines().Count));
        }
        [Test]
        public void Test_GetID()
        {
            o.SetId(2);
            Assert.That(2, Is.EqualTo(o.GetID()));
        }
        [Test]
        public void Test_SetID()
        {
            o.SetId(1);
            Assert.That(1, Is.EqualTo(o.GetID()));
        }
        [Test]
        public void Test_UpdateTotalPrice()
        {
            o.AddOrderLine(ol);
            o.UpdateTotalPrice();
            double x = o.GetPrice();
            Assert.AreEqual(40, x);
        }
        [Test]
        public void Test_GetPrice()
        {
            Assert.That(0, Is.EqualTo(o.GetPrice()));
        }
        [Test]
        public void Test_ProductsLeft()
        {

            Assert.That(o.ProductsLeft(ol), Is.EqualTo(0));
        }
        [Test]
        public void Test_AddOrderLine()
        {
            o.AddOrderLine(ol);
            Assert.That(o.GetOrderLines()[0], Is.EqualTo(ol));
        }
        [Test]
        public void Test_DeleteOrderLine()
        {
            o.DeleteOrderLine(ol);
            Assert.That(o.GetOrderLines().IsNullOrEmpty(), Is.True);
        }
        [Test]
        public void Test_UpdateOrderLine()
        {
            o.AddOrderLine(ol);
            o.UpdateOrderLine(1, 4);
            Assert.That(o.GetOrderLines()[0].GetAmount(), Is.EqualTo(4));
        }
        [Test]
        public void Test_GetOrderLines()
        {
            List<OrderLine> ols = new List<OrderLine>();
            ols.Add(ol);
            o.AddOrderLine(ol);
            CollectionAssert.AreEqual(ols, o.GetOrderLines());
        }
    }
}