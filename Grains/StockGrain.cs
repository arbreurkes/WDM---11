using DataModels;
using Orleans;
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
            if (!stock.Exists)
            {
                throw new StockDoesNotExistsException(); 
            }
            if(stock.Quantity + amount > 0)
            {
                stock.Quantity += amount;
            }
            else
            {
                throw new Exception("Not enough stock."); //Change to proper exceptions
            }
            return Task.FromResult(0);
        }

        public Task<Stock> GetStock()
        {
            if (!stock.Exists)
            {
                throw new StockDoesNotExistsException();
            }
            return Task.FromResult(stock);
        }

        public Task<int> GetAmount()
        {
            if(!stock.Exists)
            {
                throw new Exception("Item does not exist");
            }
            return Task.FromResult(stock.Quantity.Value);
        }

        public void Create(decimal price)
        {
            //What if the item already exists ? Are updates allowed ?
            stock.Price = price;
            stock.ID = this.GetPrimaryKey();
        }
    }
}
