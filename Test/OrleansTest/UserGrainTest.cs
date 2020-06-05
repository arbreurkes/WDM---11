using Infrastructure.Interfaces;
using NUnit.Framework;
using Orleans.TestingHost;
using System;
using System.Threading.Tasks;

namespace Test.OrleansTest
{
    class UserGrainTest 
    {
        private TestCluster _cluster;
 
        [SetUp]
        public void Setup()
        {
            var builder = new TestClusterBuilder();
            builder.AddSiloBuilderConfigurator<TestSiloConfigurations>();
            this._cluster = builder.Build();
            
        }
        [Test]
        public async Task CreateUserCorrectly()
        {

            this._cluster.Deploy();
            Guid id = Guid.NewGuid();
            var user_grain = _cluster.GrainFactory.GetGrain<IUserGrain>(id);
            var user = await user_grain.CreateUser();

            _cluster.StopAllSilos();

            Assert.AreEqual(id, user.ID);
            Assert.AreEqual(0, user.Credit);

            this._cluster.Deploy();
            user_grain = _cluster.GrainFactory.GetGrain<IUserGrain>(id);
            try
            {
                //User shouldnt exist because the silo was killed and the user is not begin stored persistently.
                user_grain.GetUser().Wait();
                Assert.True(false);
              }
            catch
            {
                Assert.True(true);
            }
        }
    }
}
