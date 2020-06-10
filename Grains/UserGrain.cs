using System;
using System.Threading.Tasks;
using DataModels;
using Infrastructure.Interfaces;
using Orleans;
using Orleans.Runtime;
using Orleans.Transactions.Abstractions;


namespace Grains
{
    
    public class UserGrain : Grain, IUserGrain
    {
        private readonly ITransactionalState<User> _tuser;

        public UserGrain([TransactionalState("tuser", "transactionStore")]  ITransactionalState<User> tuser)
        {
        
            _tuser = tuser;
        }

        public async Task<User> CreateUser()
        {
            await _tuser.PerformUpdate(i => i.Create(this.GetPrimaryKey()));

            return await _tuser.PerformRead(i => i);
        }

        public async Task RemoveUser()
        {
            if (!(await _tuser.PerformRead(i => i.Exists)))
            {
                throw new UserDoesNotExistsException();
            }

    
           await _tuser.PerformUpdate(i => i.Reset());
            
        }

        public async Task<User> GetUser()
        {
            if (!(await _tuser.PerformRead(i => i.Exists)))
            {
                throw new UserDoesNotExistsException();
            }

            return await _tuser.PerformRead(i => i);
        }

        public async Task<decimal> GetCredit()
        {
            if (!(await _tuser.PerformRead(i => i.Exists)))
            {
                throw new UserDoesNotExistsException();
            }

            return await _tuser.PerformRead(i=>i.Credit);
        }

        public async Task ChangeCredit(decimal amount)
        {
            if (!(await _tuser.PerformRead(i => i.Exists)))
            {
                throw new UserDoesNotExistsException();
            }


            var credit = await _tuser.PerformRead(i => i.Credit);
            if (credit + amount < decimal.Zero)
            {
                throw new NotEnoughCreditException();
            }

           
           
            await _tuser.PerformUpdate(i => i.Credit += amount);
            
            
        }
    }
}