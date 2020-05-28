using Infrastructure.Interfaces;
using DataModels;
using Orleans;
using System;
using System.Threading.Tasks;

namespace OrleansBasics
{
    public class UserGrain : Grain, IUserGrain
    {
        //This object should be changed to persistentstate/transactionalstate to allow persistence or transactions. 
        User user = new User();

        public Task<User> CreateUser()
        {
            user.Create();
            return Task.FromResult(user);
        }

        public Task<bool> RemoveUser()
        {
            bool result = false;

            if (user.Exists)
            {
                user = new User(); // resets timestamp
                result = true;
            }

            return Task.FromResult(result);
        }

        public Task<decimal> GetCredit()
        {
            if (!user.Exists)
            {
                return null; //NOT FOUND(404)
            }
            return Task.FromResult(user.Credit);
        }

        //Use this to check if user was created before, therefore if it exists in the other methods.
        public Task<User> GetUser()
        {
            if (!user.Exists)
            {
                throw new Exception();
            }

            return Task.FromResult(user);

        }

        public Task<bool> ChangeCredit(decimal amount)
        {
            bool result = false;

            if (!user.Exists)
            {
                throw new UserDoesNotExistsException();
            }
            if(user.Credit + amount > 0)
            {
                user.Credit += amount;
                result = true;
            }

            return Task.FromResult(result);
        }

   
    }
}
