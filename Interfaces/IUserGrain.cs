using DataModels;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUserGrain : IGrainWithGuidKey
    {
        [Transaction(TransactionOption.CreateOrJoin)]
        Task<User> CreateUser();

        Task RemoveUser();

        [Transaction(TransactionOption.CreateOrJoin)]
        Task<User> GetUser();

        [Transaction(TransactionOption.CreateOrJoin)]
        Task<decimal> GetCredit();

        [Transaction(TransactionOption.CreateOrJoin)]
        Task ChangeCredit(decimal amount);


    }
}
