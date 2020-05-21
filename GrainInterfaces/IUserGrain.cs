using DataModels;
using Orleans;
using System;
using System.Threading.Tasks;

namespace OrleansBasics
{
    public interface IUserGrain : IGrainWithGuidKey
    {
        Task<Guid> CreateUser();

        Task<bool> RemoveUser();

        Task<User> GetUser();

        Task<decimal> GetCredit();

        Task<bool> ChangeCredit(decimal amount);

     
    }
}
