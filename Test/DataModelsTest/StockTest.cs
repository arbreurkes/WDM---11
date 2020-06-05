using System;
using DataModels;
using NUnit.Framework;

namespace Test.DataModelsTest
{
    public class StockTest
    {
        private Stock _stock;
        
        /// <summary>
        /// Set up an order object for testing.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _stock = new Stock();
        }

        /// <summary>
        /// Test method for ID.
        /// </summary>
        [Test]
        public void GuidTestSetGet()
        {
            Guid guid = new Guid();
            _stock.ID = guid;
            
            Assert.AreEqual(guid, _stock.ID);
        }

        /// <summary>
        /// Test method for Quantity.
        /// </summary>
        [Test]
        public void QuantityTestSetGet()
        {
            _stock.Quantity = 42;
            
            Assert.AreEqual(42, _stock.Quantity);
        }

        /// <summary>
        /// Test method for Price.
        /// </summary>
        [Test]
        public void PriceTestSetGet()
        {
            _stock.Price = 42;
            
            Assert.AreEqual(42, _stock.Price);
        }

        /// <summary>
        /// Test method for Exists being false.
        /// </summary>
        [Test]
        public void ExistsTestFalse()
        {
            Assert.False(_stock.Exists);
        }
        
        /// <summary>
        /// Test method for Exists being true.
        /// </summary>
        [Test]
        public void ExistsTestTrue()
        {
            _stock.ID = new Guid();
            
            Assert.True(_stock.Exists);
        }
    }
}