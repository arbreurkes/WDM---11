using System;
using System.Threading.Tasks;
using DataModels;
using Infrastructure.Interfaces;
using NUnit.Framework;
using Orleans.TestingHost;
using OrleansBasics;

namespace Test.GrainsTest
{
    public class StockGrainTest
    {
        private readonly decimal _price = new decimal(42.42);
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
        public async Task CreateTestTrue()
        {
            Guid guid = new Guid();
            var stockGrain = _testCluster.GrainFactory.GetGrain<IStockGrain>(guid);
            var stock = await stockGrain.Create(_price);

            Assert.AreEqual(guid, stock.ID);
        }
        
        [Test]
        public async Task ChangeAndGetAmountTestTrue()
        {
            var stockGrain = _testCluster.GrainFactory.GetGrain<IStockGrain>(new Guid());
            await stockGrain.Create(_price);
            await stockGrain.ChangeAmount(42);

            Assert.AreEqual(42, await stockGrain.GetAmount());
        }
        
        [Test]
        public async Task ChangeAmountTestCannotHaveLessThanZero()
        {
            var stockGrain = _testCluster.GrainFactory.GetGrain<IStockGrain>(new Guid());
            await stockGrain.Create(_price);

            Assert.ThrowsAsync<InvalidQuantityException>( () => stockGrain.ChangeAmount(-42));
        }
        
        [Test]
        public void ChangeAmountTestStockNotExists()
        {
            Guid guid = new Guid();
            var stockGrain = _testCluster.GrainFactory.GetGrain<IStockGrain>(guid);

            Assert.ThrowsAsync<StockDoesNotExistsException>( () => stockGrain.ChangeAmount(-42));
        }
        
        [Test]
        public async Task GetStockTestTrue()
        {
            var stockGrain = _testCluster.GrainFactory.GetGrain<IStockGrain>(new Guid());
            await stockGrain.Create(_price);

            Assert.IsInstanceOf<Stock>(await stockGrain.GetStock());
        }

        [Test]
        public void GetStockTestStockNotExists()
        {
            var stockGrain = _testCluster.GrainFactory.GetGrain<IStockGrain>(new Guid());

            Assert.ThrowsAsync<StockDoesNotExistsException>(() => stockGrain.GetStock());
        }
        
        [Test]
        public void getAmountTeststockNotExists()
        {
            var stockGrain = _testCluster.GrainFactory.GetGrain<IStockGrain>(new Guid());

            Assert.ThrowsAsync<StockDoesNotExistsException>(() => stockGrain.GetAmount());
        }

        [TearDown]
        public void TearDown()
        {
            _testCluster.StopAllSilos();
        }
    }
}