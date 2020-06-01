using DataModels;
using NUnit.Framework;

namespace Test.DataModelsTest
{
    public class OrderItemTest
    {
        private OrderItem _orderItem;
        
        /// <summary>
        /// Set up an order object for testing.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _orderItem = new OrderItem(); 
        }

        /// <summary>
        /// Test method for Item getter and setter.
        /// </summary>
        [Test]
        public void ItemTestSetGet()
        {
            Stock item = new Stock();
            _orderItem.Item = item;

            Assert.AreEqual(item, _orderItem.Item);
        }

        /// <summary>
        /// Test method for quantity get and set.
        /// </summary>
        [Test]
        public void QuantityTestSetGet()
        {
            _orderItem.Quantity = 42;

            Assert.AreEqual(42, _orderItem.Quantity);
        }
        
        /// <summary>
        /// Test method for IncQuantity.
        /// </summary>
        [Test]
        public void IncQuantityTest()
        {
            _orderItem.Quantity = 41;
            _orderItem.IncQuantity();

            Assert.AreEqual(42, _orderItem.Quantity);
        }

        /// <summary>
        /// Test method for DecQuantity under right condition.
        /// </summary>
        [Test]
        public void DecQuantityTestTrue()
        {
            _orderItem.Quantity = 43;
            _orderItem.DecQuantity();
            
            Assert.AreEqual(42, _orderItem.Quantity);
        }
        
        /// <summary>
        /// Test method for DecQuantity throwing exception.
        /// </summary>
        [Test]
        public void DecQuantityTestFail()
        {
            _orderItem.Quantity = 1;
            
            Assert.Throws<InvalidQuantityException>(() => _orderItem.DecQuantity());
        }
    }
}