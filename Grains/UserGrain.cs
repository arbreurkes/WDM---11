using DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrleansBasics
{
    public class UserGrain : Orleans.Grain, IUserGrain
    {
        User user = new User();

        public Task<bool> ChangeCredit(decimal amount)
        {
            bool result = false;
            if(user.Credit + amount > 0)
            {

                user.Credit += amount;
                result = true;
            }
            return Task.FromResult(result);
        }

        public Task<decimal> GetCredit()
        {
            return Task.Factory.StartNew(() => user.Credit);
        }

        public Task<User> GetUser()
        {
            return Task.FromResult(user);
        }

        public Task<Guid> NewOrder()
        {
            //Add order to user
            var guid = Guid.NewGuid();
            return Task.FromResult(guid);
        }
    }
}
