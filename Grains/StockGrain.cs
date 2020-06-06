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
        private readonly ITransactionalState<Stock> _tstock;

        public StockGrain([PersistentState("stock", "stockStore")] IPersistentState<Stock> stock,[TransactionalState("tstock", "transactionStore")] ITransactionalState<Stock> tstock)

        {
            _stock = stock;
            _tstock = tstock;
        }


        [Transaction(TransactionOption.CreateOrJoin)]
        public Task<bool> ChangeAmount(int amount)

        {
            if (!_stock.State.Exists)
            {
                throw new StockDoesNotExistsException();
            }
            if (_stock.State.Quantity + amount > 0)
            {
                _stock.State.Quantity += amount;
                //If stock == 0, remove from database? 
            }
            else
            {
                throw new InvalidQuantityException();
            }
            
            return Task.FromResult(true);
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
