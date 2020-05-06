using DataModels;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrleansBasics
{
    public interface IOrderGrain : IGrainWithGuidKey
    {
        Task<Order> GetOrder();

        //TODO
        Task AddItem();
        //TODO
        Task RemoveItem();

    }
}
