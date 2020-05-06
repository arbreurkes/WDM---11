using DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrleansBasics
{
    public class StockGrain : Orleans.Grain, IStockGrain
    {
        Stock stock = new Stock();

        public Task ChangeAmount(int amount)
        {
            throw new NotImplementedException();
        }

        public Task<Stock> GetStock()
        {
            return Task.FromResult(stock);
        }

        public Task<int> GetAmount()
        {

            return Task.FromResult(stock.Amount);
        }
    }
}
