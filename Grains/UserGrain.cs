﻿using DataModels;
using Orleans;
using System;
using System.Threading.Tasks;

namespace OrleansBasics
{
    public class UserGrain : Orleans.Grain, IUserGrain
    {
        //This object should be changed to persistentstate/transactionalstate to allow persistence or transactions. 
        User user = new User();

        public Task<Guid> CreateUser()
        {
            user.Create();
            return Task.FromResult(this.GetPrimaryKey());
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
                return null;
            }

            return Task.FromResult(user);

        }

        public Task<bool> ChangeCredit(decimal amount)
        {
            bool result = false;

            if (!user.Exists)
            {
                return null;
            }
            if(user.Credit + amount > 0)
            {
                user.Credit += amount;
                result = true;
            }

            return Task.FromResult(result);
        }

        // TODO: Necessary?
        //Should receive the order
        public Task<Guid> NewOrder()
        {
            //Add order to user
            var guid = Guid.NewGuid();
            return Task.FromResult(guid);
        }
    }
}
