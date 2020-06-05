using DataModels;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IOrderGrain : IGrainWithGuidKey
    {
        Task<Order> CreateOrder(Guid userId);

        Task<bool> RemoveOrder();

        Task<Order> GetOrder();

        Task<bool> AddItem(Stock item);

        Task<bool> RemoveItem(Stock item);

        Task<decimal> GetTotalCost();

        Task<bool> GetStatus();

        Task<bool> Checkout();

        Task<bool> Complete();

        Task<bool> CancelCheckout();

        Task<Guid> GetUser();

        Task<bool> ClearOrder();
    }
}
