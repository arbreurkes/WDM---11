using System.Threading.Tasks;
using DataModels;
using Infrastructure.Interfaces;
using Orleans;
using Orleans.Runtime;
using OrleansBasics;

namespace Grains
{
    public class StockGrain : Grain, IStockGrain
    {
        private readonly IPersistentState<Stock> _stock;


        public StockGrain([PersistentState("order", "orderStore")] IPersistentState<Stock> stock)
        {
            _stock = stock;
        }

        public Task ChangeAmount(int amount)
        {
            if (!_stock.State.Exists)
            {
                throw new StockDoesNotExistsException();
            }
            if (_stock.State.Quantity + amount > 0)
            {
                _stock.State.Quantity += amount;
            }
            else
            {
                throw new InvalidQuantityException();
            }
            return Task.FromResult(0);
        }

        public Task<Stock> GetStock()
        {
            if (!_stock.State.Exists)
            {
                throw new StockDoesNotExistsException();
            }
            return Task.FromResult(_stock.State);
        }

        public Task<int> GetAmount()
        {
            if (!_stock.State.Exists)
            {
                throw new StockDoesNotExistsException();
            }
            return Task.FromResult(_stock.State.Quantity);
        }

        public Task<Stock> Create(decimal price)
        {
            //What if the item already exists ? Are updates allowed ?
            _stock.State.Price = price;
            _stock.State.ID = this.GetPrimaryKey();
            return Task.FromResult(_stock.State);
        }
    }
}
