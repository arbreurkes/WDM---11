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

        public UserGrain([PersistentState("user", "userStore")] IPersistentState<User> user)
        {
            _user = user;
          
        }

        public async Task<User> CreateUser()
        {
            _user.State.Create(this.GetPrimaryKey());
             await _user.WriteStateAsync();
            return _user.State;
        }

        public async Task RemoveUser()
        {

            if (!_user.State.Exists)
            {
                throw new UserDoesNotExistsException();
            }

            //Remove user from database.
            await _user.ClearStateAsync();
            this.DeactivateOnIdle();
            
        }

        public Task<User> GetUser()
        {
            if (!_user.State.Exists)
            {
                throw new UserDoesNotExistsException();
            }

            return Task.FromResult(_user.State);
        }

        public Task<decimal> GetCredit()
        {
            if (!_user.State.Exists)
            {
                throw new UserDoesNotExistsException();
            }

            return Task.FromResult(_user.State.Credit);
        }

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
                
            _user.State.Credit += amount;
            await _user.WriteStateAsync();
        }
    }
}