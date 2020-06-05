using System;
using DataModels;
using NUnit.Framework;

namespace Test.DataModelsTest
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

        [Test]
        public void IDTestGet()
        {
            Guid guid = new Guid();
            _user.Create(guid);
            
            Assert.AreEqual(guid, _user.ID);
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
        public void CreatedAtTestCreated()
        {
            _user.Create(new Guid());
            
            Assert.NotNull(_user.CreatedAt);
        }
        
        /// <summary>
        /// Test method for Create user already created.
        /// </summary>
        [Test]
        public void CreatedAtTestNotCreated()
        {
            Assert.Null(_user.CreatedAt);
        }

        /// <summary>
        /// Test method for Create under right conditions.
        /// </summary>
        [Test]
        public void CreateTestTrue()
        {
            Assert.True(_user.Create(new Guid()));
        }

        /// <summary>
        /// Test method for Create user already created.
        /// </summary>
        [Test]
        public void CreateTestUserAlreadyCreated()
        {
            _user.Create(new Guid());
            
            Assert.False(_user.Create(new Guid()));
        }
    }
}