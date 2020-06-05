using System.Threading.Tasks;
using DataModels;
using Infrastructure.Interfaces;
using Orleans;
using Orleans.Runtime;
using Orleans.Transactions.Abstractions;
using System.Threading.Tasks;


namespace Grains
{
    public class UserGrain : Grain, IUserGrain
    {
        private readonly IPersistentState<User> _user;
        private readonly ITransactionalState<User> _tuser;
        public UserGrain([PersistentState("user", "userStore")] IPersistentState<User> user,[TransactionalState("tuser", "transactionStore")]  ITransactionalState<User> tuser)
        {
            _user = user;
            _tuser = tuser;
        }
        public Task<User> CreateUser()
        {

            _user.State.Create(this.GetPrimaryKey());
           
            return Task.FromResult(_user.State);
        }

        public Task<bool> RemoveUser()
        {
            bool result = true;

            if (!_user.State.Exists)
            {
                throw new UserDoesNotExistsException();
            }

            //Remove user from database.

            return Task.FromResult(result);
        }

        public Task<decimal> GetCredit()
        {
            if (!_user.State.Exists)
            {
                throw new UserDoesNotExistsException();
            }

            return Task.FromResult(_user.State.Credit);
        }

        public Task<User> GetUser()
        {
            if (!_user.State.Exists)
            {
                throw new UserDoesNotExistsException();
            }

            return Task.FromResult(_user.State);
        }
        public Task<bool> ChangeCredit(decimal amount)
        {
            if (!_user.State.Exists)
            {
                throw new UserDoesNotExistsException();
            }

            if (_user.State.Credit + amount > 0)
            {
                _user.State.Credit += amount;
               
                result = true;
            }


            if (_user.State.Credit + amount < decimal.Zero) return Task.FromResult(false);
            _user.State.Credit += amount;

            return Task.FromResult(true);
        }
    }
}