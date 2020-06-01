using System;
using System.Collections.Generic;
using NUnit.Framework;
using DataModels;

namespace DataModelsTest
{
    public class OrderTest
    {
        private Order _order;
        
        /// <summary>
        /// Set up an order object for testing.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _order = new Order();
        }

        /// <summary>
        /// Test method for Create and get Guid.
        /// </summary>
        [Test]
        public void TestGuidSetGet()
        {
            Guid guid = Guid.NewGuid();
            _order.Create(guid);
            
            Assert.AreEqual(guid, _order.userId);
        }
        
        /// <summary>
        /// Test method for getItems.
        /// </summary>
        [Test]
        public void TestGetItems ()
        {
            Assert.AreEqual(new Dictionary<Guid, OrderItem>(), _order.Items);
        }
        
        /// <summary>
        /// Test method for gert CreatedAt.
        /// </summary>
        [Test]
        public void TestCreatedAtTrue ()
        {
            _order.Create(new Guid());

            Assert.NotNull(_order.CreatedAt);
        }
        
        /// <summary>
        /// Test method for get CreatedAt.
        /// </summary>
        [Test]
        public void TestCreatedAtFalse ()
        {
            Assert.Null(_order.CreatedAt);
        }

        /// <summary>
        /// Test method for Exists.
        /// </summary>
        [Test]
        public void TestExistsFalse ()
        {
            Assert.False(_order.Exists);
        }
        
        /// <summary>
        /// Test method for Exists.
        /// </summary>
        [Test]
        public void TestExistsTrue ()
        {
            _order.Create(new Guid());
            Assert.True(_order.Exists);
        }
        
        /// <summary>
        /// Test method for CheckedOut being false.
        /// </summary>
        [Test]
        public void TestCheckedOutFalse ()
        {
            Assert.False(_order.CheckedOut);
        }
        
        /// <summary>
        /// Test method for CheckedOut being false.
        /// </summary>
        [Test]
        public void TestCanCheckOutFalse ()
        {
            Assert.False(_order.CheckedOut);
        }
        
        /// <summary>
        /// Test method for CheckedOut being true.
        /// </summary>
        [Test]
        public void TestCheckedOutTrue ()
        {
            _order.Create(new Guid());
            _order.Checkout();
            
            Assert.True(_order.CheckedOut);
        }
        
        /// <summary>
        /// Test method for Completed being false.
        /// </summary>
        [Test]
        public void TestCompletedFalse ()
        {
            Assert.False(_order.Completed);
        }
        
        /// <summary>
        /// Test method for Completed being false when CheckedOut is true.
        /// </summary>
        [Test]
        public void TestCompletedFalseCheckedOutTrue ()
        {
            _order.Checkout();

            Assert.False(_order.Completed);
        }
        
        /// <summary>
        /// Test method for Completed being true.
        /// </summary>
        [Test]
        public void TestCompletedTrue ()
        {
            _order.Create(new Guid());
            _order.Checkout();
            _order.Complete();
            
            Assert.True(_order.Completed);
        }

        /// <summary>
        /// Test method for CanCheckout already checked out.
        /// </summary>
        [Test]
        public void TestCanCheckoutAlreadyCheckedOut()
        {
            _order.Checkout();
            
            Assert.False(_order.CanCheckout);
        }
        
        /// <summary>
        /// Test method for CanCheckout already completed.
        /// </summary>
        [Test]
        public void TestCanCheckoutAlreadyCompleted()
        {
            _order.Checkout();
            _order.Complete();
            
            Assert.False(_order.CanCheckout);
        }
        
        /// <summary>
        /// Test method for CanCheckout under right conditions.
        /// </summary>
        [Test]
        public void TestCanCheckoutTrue()
        {
            _order.Create(new Guid());
            
            Assert.True(_order.CanCheckout);
        }
        
        /// <summary>
        /// Test method for CanComplete not checked out.
        /// </summary>
        [Test]
        public void TestCanCompleteNotCheckedOut()
        {
            Assert.False(_order.CanComplete);
        }
        
        /// <summary>
        /// Test method for CanComplete already completed.
        /// </summary>
        [Test]
        public void TestCanCompleteAlreadyCompleted()
        {
            _order.Checkout();
            _order.Complete();
            
            Assert.False(_order.CanComplete);
        }
        
        /// <summary>
        /// Test method for CanComplete under right conditions.
        /// </summary>
        [Test]
        public void TestCanCompleteTrue()
        {
            _order.Create(new Guid());
            _order.Checkout();
            
            Assert.True(_order.CanComplete);
        }
        
        /// <summary>
        /// Test method for Total.
        /// </summary>
        [Test]
        public void TestTotal()
        {
            OrderItem orderItem = new OrderItem();
            Stock item = new Stock();
            orderItem.Quantity = 2;
            item.Price = 21;
            orderItem.Item = item;
            
            _order.Items.Add(new Guid(), orderItem);
            
            Assert.AreEqual(_order.Total, 42);
        }
        
        /// <summary>
        /// Test method for Create.
        /// </summary>
        [Test]
        public void TestCreate()
        {
            Guid guid = new Guid();
            _order.Create(guid);
            
            Assert.AreEqual(guid, _order.userId);
        }
        
        /// <summary>
        /// Test method for Checkout False.
        /// True already tested above.
        /// </summary>
        [Test]
        public void TestCheckoutFalse()
        {
            Assert.False(_order.Checkout());
        }
        
        /// <summary>
        /// Test method for Complete False.
        /// True already tested above.
        /// </summary>
        [Test]
        public void TestCompleteFalse()
        {
            _order.Create(new Guid());
            
            Assert.False(_order.Complete());
        }
        
        /// <summary>
        /// Test method for CancelCheckout False.
        /// </summary>
        [Test]
        public void TestCancelCheckoutNotCheckedOut()
        {
            _order.Create(new Guid());

            Assert.False(_order.CancelCheckout());
        }
        
        /// <summary>
        /// Test method for CancelCheckout False.
        /// </summary>
        [Test]
        public void TestCancelCheckoutAlreadyCompleted()
        {
            _order.Create(new Guid());
            _order.Checkout();
            _order.Complete();

            Assert.False(_order.CancelCheckout());
        }
        
        /// <summary>
        /// Test method for CancelCheckout True.
        /// </summary>
        [Test]
        public void TestCancelCheckoutTrue()
        {
            _order.Create(new Guid());
            _order.Checkout();

            Assert.True(_order.CancelCheckout());
        }
    }
}