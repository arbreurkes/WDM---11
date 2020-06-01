using DataModels;
using Infrastructure.Interfaces;
using Orleans;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace OrleansBasics
{
    public class UserGrain : Grain, IUserGrain
    {
        private readonly IPersistentState<User> _user;

        public UserGrain([PersistentState("user", "userStore")] IPersistentState<User> user)
        {
            _user = user;
        }
        //This object should be changed to persistentstate/transactionalstate to allow persistence or transactions. 

        public Task<User> CreateUser()
        {
            //What if user already exists ? 

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
            bool result = false;

            if (!_user.State.Exists)
            {
                throw new UserDoesNotExistsException();
            }
            if (_user.State.Credit + amount > 0)
            {
                _user.State.Credit += amount;
                result = true;
            }

            return Task.FromResult(result);
        }
    }
}
