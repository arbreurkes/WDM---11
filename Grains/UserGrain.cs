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
            //What if user already exists ? 

            user.Create(this.GetPrimaryKey());
            return Task.FromResult(user);
        }

        public Task<bool> RemoveUser()
        {
            bool result = true;

            if (!user.Exists)
            {
                throw new UserDoesNotExistsException();
            }

            //Remove user from database.

            return Task.FromResult(result);
        }

        public Task<decimal> GetCredit()
        {
            if (!user.Exists)
            {
                throw new UserDoesNotExistsException();
            }
            return Task.FromResult(user.Credit);
        }

        public Task<User> GetUser()
        {
            if (!user.Exists)
            {
                throw new UserDoesNotExistsException();
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
