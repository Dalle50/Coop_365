using Delta_Coop365;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using System.Printing;
using System.Windows.Media.Imaging;


namespace Delta_Coop365_Test
{
    public class Order_Test
    {
        private Order o;
        private OrderLine ol;
        private DateTime date = DateTime.Now;

        [SetUp]
        public void Setup()
        {
            o = new Order();
            ol = new OrderLine(
                                    new Product(
                                    1,
                                        "snegl",
                                            2,
                                                20,
                                                    "mel",
                                                            "C:\\Users\\danie\\source\\repos\\Delta_Coop365\\Delta_Coop365\\ProductPictures\\1007.jpeg"),
                                                                 2, date);
    }
        [TearDown]
        public void TearDown()
        {
            o = null;
        }
        [Test]
        public void Test_Constructor()
        {
            Assert.IsNotNull(o);
            o.AddOrderLine(ol);
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
            Assert.AreEqual(40, o.GetPrice());
            o.ClearOrderLines();
        }
        [Test]
        public void Test_GetPrice()
        {
            o.AddOrderLine(ol);
            o.UpdateTotalPrice();
            Assert.That(40.0, Is.EqualTo((double) o.GetPrice()));
            o.ClearOrderLines();
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
            o.ClearOrderLines();
        }
        [Test]
        public void Test_DeleteOrderLine()
        {
            o.DeleteOrderLine(ol);
            Assert.That(o.GetOrderLines().IsNullOrEmpty(), Is.True);
            o.ClearOrderLines();
        }
        [Test]
        public void Test_UpdateOrderLine()
        {
            o.AddOrderLine(ol);
            o.UpdateOrderLine(1, 4);
            Assert.That(o.GetOrderLines()[0].GetAmount(), Is.EqualTo(4));
            o.ClearOrderLines();
        }
        [Test]
        public void Test_GetOrderLines()
        {
            List<OrderLine> ols = new List<OrderLine>();
            ols.Add(ol);
            o.AddOrderLine(ol);
            CollectionAssert.AreEqual(ols, o.GetOrderLines());
            o.ClearOrderLines();
        }
        [Test]
        public void Test_ClearOrderLines()
        {
            o.AddOrderLine(ol);
            Assert.AreEqual(1, o.orderLines.Count());
            o.ClearOrderLines();
            Assert.AreEqual(0, o.orderLines.Count());
            o.ClearOrderLines();
        }

    }
}