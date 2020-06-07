using System;
using System.Threading.Tasks;
using DataModels;
using Grains;
using Infrastructure.Interfaces;
using NUnit.Framework;
using Orleans.TestingHost;

namespace Test.GrainsTest
{
    public class OrderGrainTest
    {
        private readonly decimal _price = new decimal(42.42);
        private TestCluster _testCluster;
        private IOrderGrain _orderGrain;
        private Guid _guid;
        
        [SetUp]
        public void SetUp()
        {
            var builder = new TestClusterBuilder();
            builder.AddSiloBuilderConfigurator<TestSiloConfigurations>();
            _testCluster = builder.Build();

            _testCluster.Deploy();

            _guid = new Guid();
            _orderGrain = _testCluster.GrainFactory.GetGrain<IOrderGrain>(_guid);
        }
        
        [Test]
        public async Task CreateTestTrue()
        {
            var order = await _orderGrain.CreateOrder(_guid);

            Assert.AreEqual(_guid, order.ID);
        }

        [Test]
        public async Task RemoveOrderTestTrue()
        {
            await _orderGrain.CreateOrder(_guid);

            Assert.True(await _orderGrain.RemoveOrder());
        }
        
        [Test]
        public async Task RemoveOrderTestOrderNotExists()
        {
            Assert.False(await _orderGrain.RemoveOrder());
        }
        
        [Test]
        public async Task GetOrderTestTrue()
        {
            await _orderGrain.CreateOrder(_guid);

            Assert.IsInstanceOf<Order>(await _orderGrain.GetOrder());
        }
        
        [Test]
        public void GetOrderTestOrderNotExists()
        {
            Assert.ThrowsAsync<OrderDoesNotExistsException>(() => _orderGrain.GetOrder());
        }
        
        [Test]
        public async Task AddItemTestNotYetInOrder()
        {
            await _orderGrain.CreateOrder(_guid);

            var item = new Stock {ID = _guid, Price = _price};
            await _orderGrain.AddItem(item);

            Assert.True(await _orderGrain.AddItem(item));
        }
        
        [Test]
        public async Task AddItemGetCostTestAlreadyInOrder()
        {
            await _orderGrain.CreateOrder(_guid);
            var item = new Stock {ID = _guid, Price = _price};
            await _orderGrain.AddItem(item);
            await _orderGrain.AddItem(item);

            Assert.AreEqual(2 * _price, await _orderGrain.GetTotalCost());
        }
        
        [Test]
        public async Task AddItemTestItemNotExists()
        {
            await _orderGrain.CreateOrder(_guid);
            var item = new Stock();

            Assert.False(await _orderGrain.AddItem(item));
        }
        
        [Test]
        public void AddItemTestOrderNotExists()
        {
            Assert.ThrowsAsync<OrderDoesNotExistsException>(() => _orderGrain.AddItem(new Stock()));
        }
        
        [Test]
        public async Task RemoveItemGetCostTestTrue()
        {
            await _orderGrain.CreateOrder(_guid);
            var item = new Stock {ID = _guid};
            await _orderGrain.AddItem(item);

            Assert.True(await _orderGrain.RemoveItem(item));
        }
        
        [Test]
        public async Task RemoveItemGetCostTestNotEnoughInOrder()
        {
            await _orderGrain.CreateOrder(_guid);
            var item = new Stock {ID = _guid};
            await _orderGrain.AddItem(item);
            await _orderGrain.RemoveItem(item);

            Assert.False(await _orderGrain.RemoveItem(item));
        }
        
        [Test]
        public async Task RemoveItemTestNotInOrder()
        {
            await _orderGrain.CreateOrder(_guid);
            var item = new Stock {ID = _guid};

            Assert.False(await _orderGrain.RemoveItem(item));
        }
        
        [Test]
        public async Task RemoveItemTestItemNotExists()
        {
            await _orderGrain.CreateOrder(_guid);
            var item = new Stock();

            Assert.False(await _orderGrain.RemoveItem(item));
        }
        
        [Test]
        public void RemoveItemTestOrderNotExists()
        {
            Assert.ThrowsAsync<OrderDoesNotExistsException>(() => _orderGrain.RemoveItem(new Stock()));
        }
        
        [Test]
        public void GetTotalCostTestOrderNotExists()
        {
            Assert.ThrowsAsync<OrderDoesNotExistsException>(() => _orderGrain.GetTotalCost());
        }
        
        [Test]
        public async Task GetStatusTestTrue()
        {
            await _orderGrain.CreateOrder(_guid);
            await _orderGrain.Checkout();
            await _orderGrain.Complete();

            Assert.True((await _orderGrain.GetStatus()).Paid);
        }
        
        [Test]
        public async Task GetStatusTestOrderNotExists()
        {
            Assert.False((await _orderGrain.GetStatus()).Paid);
        }
        
        [Test]
        public async Task GetStatusTestOrderNotCompleted()
        {
            await _orderGrain.CreateOrder(_guid);

            Assert.False((await _orderGrain.GetStatus()).Paid);
        }
        
        [Test]
        public async Task CheckoutTestTrue()
        {
            await _orderGrain.CreateOrder(_guid);

            Assert.True(await _orderGrain.Checkout());
        }
        
        [Test]
        public async Task CheckoutTestOrderNotExists()
        {
            Assert.False(await _orderGrain.Checkout());
        }
        
        [Test]
        public async Task CheckoutTestOrderAlreadyCheckedOut()
        {
            await _orderGrain.CreateOrder(_guid);
            await _orderGrain.Checkout();

            Assert.False(await _orderGrain.Checkout());
        }
        
        [Test]
        public async Task CheckoutTestOrderAlreadyCompleted()
        {
            await _orderGrain.CreateOrder(_guid);
            await _orderGrain.Checkout();
            await _orderGrain.Complete();

            Assert.False(await _orderGrain.Checkout());
        }

        [Test]
        public async Task CancelCheckoutTestTrue()
        {
            await _orderGrain.CreateOrder(_guid);
            await _orderGrain.Checkout();
            
            Assert.True(await _orderGrain.CancelCheckout());
        }
        
        [Test]
        public async Task CancelCheckoutNotCheckedOut()
        {
            await _orderGrain.CreateOrder(_guid);

            Assert.False(await _orderGrain.CancelCheckout());
        }
        
        [Test]
        public async Task CancelCheckoutAlreadyCompleted()
        {
            await _orderGrain.CreateOrder(_guid);
            await _orderGrain.Checkout();
            await _orderGrain.Complete();

            Assert.False(await _orderGrain.CancelCheckout());
        }
        
        [Test]
        public async Task CancelCheckoutOrderNotExists()
        {
            Assert.False(await _orderGrain.CancelCheckout());
        }
        
        [Test]
        public async Task GetUserTrue()
        {
            await _orderGrain.CreateOrder(_guid);

            Assert.AreEqual(_guid, await _orderGrain.GetUser());
        }
        
        [Test]
        public void GetUserTestOrderNotExists()
        {
            Assert.ThrowsAsync<OrderDoesNotExistsException>(() => _orderGrain.GetUser());
        }
        
        [TearDown]
        public void TearDown()
        {
            _orderGrain.ClearOrder();
            _testCluster.StopAllSilos();
        }
    }
}