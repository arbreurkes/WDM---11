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
        Task<Guid> CreateOrder(Guid userId);

        Task<bool> RemoveOrder();

        Task<Order> GetOrder();

        void AddItem(Stock item);
       
        void RemoveItem(Stock item);

        Task<decimal> GetTotalCost();
        
        Task<bool> GetStatus();

        Task<bool> Checkout();

        Task<bool> Complete();
        
        Task<bool> CancelCheckout();

        Task<Guid> GetUser();
    }
}
