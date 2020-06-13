using System;
using DataModels;
using NUnit.Framework;
using Orleans.TestingHost;
using System.Threading.Tasks;
using Infrastructure.Interfaces;

namespace Test.GrainsTest
{
    public class UserGrainTest
    {
        private TestCluster _testCluster;

        [SetUp]
        public void SetUp()
        {
            var builder = new TestClusterBuilder();
            builder.AddSiloBuilderConfigurator<TestSiloConfigurations>();
            _testCluster = builder.Build();

            _testCluster.Deploy();
        }

        [Test]
        public async Task CreateUserTestTrue()
        {
            Guid guid = new Guid();
            var userGrain = _testCluster.GrainFactory.GetGrain<IUserGrain>(guid);
            var user = await userGrain.CreateUser();

            Assert.AreEqual(guid, user.ID);
        }

        [Test]
        public async Task RemoveUserTestTrue()
        {
            var userGrain = _testCluster.GrainFactory.GetGrain<IUserGrain>(new Guid());
            await userGrain.CreateUser();
            
            Assert.IsInstanceOf<Task>(userGrain.RemoveUser());
        }

        [Test]
        public void RemoveUserTestException()
        {
            var userGrain = _testCluster.GrainFactory.GetGrain<IUserGrain>(new Guid());

            Assert.ThrowsAsync<UserDoesNotExistsException>(() => userGrain.RemoveUser());
        }

        [Test]
        public async Task ChangeCreditAndGetCreditTestTrue()
        {
            var userGrain = _testCluster.GrainFactory.GetGrain<IUserGrain>(new Guid());
            await userGrain.CreateUser();

            decimal credit = new decimal(42.42);
            await userGrain.ChangeCredit(credit);

            Assert.AreEqual(credit, await userGrain.GetCredit());
        }

        [Test]
        public void GetCreditTestNotExists()
        {
            var userGrain = _testCluster.GrainFactory.GetGrain<IUserGrain>(new Guid());

            Assert.ThrowsAsync<UserDoesNotExistsException>(() => userGrain.GetCredit());
        }

        [Test]
        public async Task GetUserTestTrue()
        {
            var userGrain = _testCluster.GrainFactory.GetGrain<IUserGrain>(new Guid());
            await userGrain.CreateUser();

            Assert.IsInstanceOf<User>(await userGrain.GetUser());
        }

        [Test]
        public void GetUserTestUserNotExists()
        {
            var userGrain = _testCluster.GrainFactory.GetGrain<IUserGrain>(new Guid());

            Assert.ThrowsAsync<UserDoesNotExistsException>(() => userGrain.GetUser());
        }
        
        [Test]
        public async Task ChangeCreditTestTrue()
        {
            var userGrain = _testCluster.GrainFactory.GetGrain<IUserGrain>(new Guid());
            await userGrain.CreateUser();
            
            Assert.IsInstanceOf<Task>(userGrain.ChangeCredit(new decimal(42)));
        }
        
        [Test]
        public async Task ChangeCreditTestNotEnoughCredit()
        {
            var userGrain = _testCluster.GrainFactory.GetGrain<IUserGrain>(new Guid());
            await userGrain.CreateUser();
            await userGrain.ChangeCredit(new decimal(42));
            
            Assert.IsInstanceOf<Task>(userGrain.ChangeCredit(new decimal(-42.42)));
        }
        
        [Test]
        public void ChangeCreditTestUserNotExists()
        {
            var userGrain = _testCluster.GrainFactory.GetGrain<IUserGrain>(new Guid());

            Assert.ThrowsAsync<UserDoesNotExistsException>(() => userGrain.ChangeCredit(decimal.MinusOne));
        }

        [TearDown]
        public void TearDown()
        {
            _testCluster.StopAllSilos();
        }
    }
}