using DataModels;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IOrderGrain : IGrainWithGuidKey
    {
        [Transaction(TransactionOption.CreateOrJoin)]
        Task<Order> CreateOrder(Guid userId);

        Task RemoveOrder();

        [Transaction(TransactionOption.CreateOrJoin)]
        Task<Order> GetOrder();

        [Transaction(TransactionOption.CreateOrJoin)]
        Task AddItem(Stock item);

        [Transaction(TransactionOption.CreateOrJoin)]
        Task RemoveItem(Stock item);

        Task<decimal> GetTotalCost();

        Task<Payment> GetStatus();

        [Transaction(TransactionOption.CreateOrJoin)]
        Task<bool> Checkout();

        Task<bool> Complete();

        Task<bool> CancelCheckout();

        Task<bool> CancelComplete();

        [Transaction(TransactionOption.CreateOrJoin)]
        Task<Guid> GetUser();

        [Transaction(TransactionOption.CreateOrJoin)]
        Task<List<OrderItem>> GetItems();
     


    }
}
