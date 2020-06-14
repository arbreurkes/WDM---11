using DataModels;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUserGrain : IGrainWithGuidKey
    {
        Task<User> CreateUser();

        Task RemoveUser();

        Task<User> GetUser();

        Task<decimal> GetCredit();

        Task ChangeCredit(decimal amount);


    }
}
