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
         
        }
        
        /// <summary>
        /// Test method for getItems.
        /// </summary>
        [Test]
        public void TestGetItems ()
        {
        }
        
        /// <summary>
        /// Test method for gert CreatedAt.
        /// </summary>
        [Test]
        public void TestCreatedAtTrue ()
        {
          
        }
        
        /// <summary>
        /// Test method for get CreatedAt.
        /// </summary>
        [Test]
        public void TestCreatedAtFalse ()
        {
            
        }

        /// <summary>
        /// Test method for Exists.
        /// </summary>
        [Test]
        public void TestExistsFalse ()
        {
            
        }
        
        /// <summary>
        /// Test method for Exists.
        /// </summary>
        [Test]
        public void TestExistsTrue ()
        {
           
        }
        
        /// <summary>
        /// Test method for CheckedOut being false.
        /// </summary>
        [Test]
        public void TestCheckedOutFalse ()
        {
        }
        
        /// <summary>
        /// Test method for CheckedOut being false.
        /// </summary>
        [Test]
        public void TestCanCheckOutFalse ()
        {
        }
        
        /// <summary>
        /// Test method for CheckedOut being true.
        /// </summary>
        [Test]
        public void TestCheckedOutTrue ()
        {
          
            
        }
        
        /// <summary>
        /// Test method for Completed being false.
        /// </summary>
        [Test]
        public void TestCompletedFalse ()
        {
        }
        
        /// <summary>
        /// Test method for Completed being false when CheckedOut is true.
        /// </summary>
        [Test]
        public void TestCompletedFalseCheckedOutTrue ()
        {
           
        }
        
        /// <summary>
        /// Test method for Completed being true.
        /// </summary>
        [Test]
        public void TestCompletedTrue ()
        {
         
        }

        /// <summary>
        /// Test method for CanCheckout already checked out.
        /// </summary>
        [Test]
        public void TestCanCheckoutAlreadyCheckedOut()
        {
         
        }
        
        /// <summary>
        /// Test method for CanCheckout already completed.
        /// </summary>
        [Test]
        public void TestCanCheckoutAlreadyCompleted()
        {
           
        }
        
        /// <summary>
        /// Test method for CanCheckout under right conditions.
        /// </summary>
        [Test]
        public void TestCanCheckoutTrue()
        {
            
        }
        
        /// <summary>
        /// Test method for CanComplete not checked out.
        /// </summary>
        [Test]
        public void TestCanCompleteNotCheckedOut()
        {
        }
        
        /// <summary>
        /// Test method for CanComplete already completed.
        /// </summary>
        [Test]
        public void TestCanCompleteAlreadyCompleted()
        {
         
        }
        
        /// <summary>
        /// Test method for CanComplete under right conditions.
        /// </summary>
        [Test]
        public void TestCanCompleteTrue()
        {
          
        }
        
        /// <summary>
        /// Test method for Total.
        /// </summary>
        [Test]
        public void TestTotal()
        {
          
        }
        
        /// <summary>
        /// Test method for Create.
        /// </summary>
        [Test]
        public void TestCreate()
        {
        }
        
        /// <summary>
        /// Test method for Checkout False.
        /// True already tested above.
        /// </summary>
        [Test]
        public void TestCheckoutFalse()
        {
         
        }
        
        /// <summary>
        /// Test method for Complete False.
        /// True already tested above.
        /// </summary>
        [Test]
        public void TestCompleteFalse()
        {
           
        }
        
        /// <summary>
        /// Test method for CancelCheckout False.
        /// </summary>
        [Test]
        public void TestCancelCheckoutNotCheckedOut()
        {
           
        }
        
        /// <summary>
        /// Test method for CancelCheckout False.
        /// </summary>
        [Test]
        public void TestCancelCheckoutAlreadyCompleted()
        {
           
        }
        
        /// <summary>
        /// Test method for CancelCheckout True.
        /// </summary>
        [Test]
        public void TestCancelCheckoutTrue()
        {
            
        }
    }
}