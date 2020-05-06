using DataModels;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrleansBasics
{
    public interface IStockGrain : IGrainWithGuidKey
    {
        Task<Stock> GetStock();

        Task ChangeAmount(int amount);
        Task<int> GetAmount();
        


    }
}
