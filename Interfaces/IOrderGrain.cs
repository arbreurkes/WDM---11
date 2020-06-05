using DataModels;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IOrderGrain : IGrainWithGuidKey
    {
        Task<Order> CreateOrder(Guid userId);

        Task<bool> RemoveOrder();

        Task<Order> GetOrder();

        void AddItem(Stock item);

        void RemoveItem(Stock item);

        Task<decimal> GetTotalCost();

        Task<Payment> GetStatus();

        Task<bool> Checkout();

        Task<bool> Complete();

        Task<bool> CancelCheckout();

        Task<bool> CancelComplete();

        Task<Guid> GetUser();

        Task<List<OrderItem>> GetItems();
     
    }
}
