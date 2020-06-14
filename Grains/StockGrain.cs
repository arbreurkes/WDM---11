using System.Threading.Tasks;
using DataModels;
using Infrastructure.Interfaces;
using Orleans;
using Orleans.Runtime;
using Orleans.Transactions.Abstractions;


namespace Grains
{
    public class StockGrain : Grain, IStockGrain
    {
        private readonly IPersistentState<Stock> _stock;

        public StockGrain([PersistentState("stock", "stockStore")] IPersistentState<Stock> stock)
        {
            _stock = stock;
        }

        public async Task ChangeAmount(int amount)
        {
            if (!_stock.State.Exists)
            {
                throw new StockDoesNotExistsException();
            }

            if (_stock.State.Quantity + amount < 0 )
            {
                throw new InvalidQuantityException();
                //If stock == 0, remove from database? 
            }

            _stock.State.Quantity += amount;
           await _stock.WriteStateAsync();
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
            
            _stock.State.Price = price;
            _stock.State.ID = this.GetPrimaryKey();
            return Task.FromResult(_stock.State);
        }

        public async Task Clear()
        {
            await _stock.ClearStateAsync();
        }
    }
}
