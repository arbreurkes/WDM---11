using System;
using DataModels;
using NUnit.Framework;

namespace DataModelsTest
{
    public class UserTest
    {
        private User _user;

        /// <summary>
        /// Set up an order object for testing.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _user = new User();
        }

        /// <summary>
        /// Test method for Credit Set and Get.
        /// </summary>
        [Test]
        public void CreditTestSetGet()
        {
            _user.Credit = 42;

            Assert.AreEqual(42, _user.Credit);
        }
        
        /// <summary>
        /// Test method for Create user already created.
        /// </summary>
        [Test]
        public void CreatedAtTest()
        {
            _user.Create();
            
            Assert.NotNull(_user.CreatedAt);
        }

        /// <summary>
        /// Test method for Create under right conditions.
        /// </summary>
        [Test]
        public void CreateTestTrue()
        {
            Assert.True(_user.Create());
        }

        /// <summary>
        /// Test method for Create user already created.
        /// </summary>
        [Test]
        public void CreateTestUserAlreadyCreated()
        {
            _user.Create();
            
            Assert.False(_user.Create());
        }
    }
}