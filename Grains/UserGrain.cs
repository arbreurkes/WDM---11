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
        private readonly IPersistentState<User> _user;
        private readonly ITransactionalState<User> _tuser;

        public UserGrain([PersistentState("user", "userStore")] IPersistentState<User> user, [TransactionalState("tuser", "transactionStore")]  ITransactionalState<User> tuser)
        {
            _user = user;
            _tuser = tuser;
        }

        public Task<User> CreateUser()
        {
            _user.State.Create(this.GetPrimaryKey());
            _user.WriteStateAsync();
            return Task.FromResult(_user.State);
        }

        public async Task RemoveUser()
        {

            if (!_user.State.Exists)
            {
                throw new UserDoesNotExistsException();
            }

            //Remove user from database.
           await _user.ClearStateAsync();
           await _tuser.PerformUpdate(i => i.Reset());
            
        }

        public Task<User> GetUser()
        {
            if (!_user.State.Exists)
            {
                throw new UserDoesNotExistsException();
            }

            return _tuser.PerformRead(i => i);
        }

        public Task<decimal> GetCredit()
        {
            if (!_user.State.Exists)
            {
                throw new UserDoesNotExistsException();
            }

            return _tuser.PerformRead(i=>i.Credit);
        }

        [Transaction(TransactionOption.CreateOrJoin)]
        public async Task ChangeCredit(decimal amount)
        {
            if (!_user.State.Exists)
            {
                throw new UserDoesNotExistsException();
            }

            if (_user.State.Credit + amount < decimal.Zero)
            {
                throw new NotEnoughCreditException();
            }
                
            await _tuser.PerformUpdate(i => i.Credit += amount);
        }
    }
}