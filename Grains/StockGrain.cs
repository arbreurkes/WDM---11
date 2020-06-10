using System.Runtime.InteropServices;
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
        private readonly ITransactionalState<Stock> _tstock;

        public StockGrain([TransactionalState("tstock", "transactionStore")] ITransactionalState<Stock> tstock)
        {
            _tstock = tstock;
        }

        public async Task ChangeAmount(int amount)
        {
            var exists = await _tstock.PerformRead(i => i.Exists);
            if (!exists)
            {
                throw new StockDoesNotExistsException();
            }

            var quantity = await _tstock.PerformRead(i => i.Quantity);
            if (quantity + amount < 0 )
            {
                throw new InvalidQuantityException();
            }

            await _tstock.PerformUpdate(i => i.Quantity += amount);
            
        }

        public async Task<Stock> GetStock()
        {
            var exists = await _tstock.PerformRead(i => i.Exists);

            if (!exists)
            {
                throw new StockDoesNotExistsException();
            }

            return await _tstock.PerformRead(i => i);
        }

        public async Task<int> GetAmount()
        {
            var exists = await _tstock.PerformRead(i => i.Exists);

            if (!exists)
            {
                throw new StockDoesNotExistsException();
            }

            return await _tstock.PerformRead(i => i.Quantity);
        }

        public Task<Stock> Create(decimal price)
        {
            
            _tstock.PerformUpdate(i => i.Create(this.GetPrimaryKey(), price));
            return _tstock.PerformRead(i => i);
        }
    }
}
