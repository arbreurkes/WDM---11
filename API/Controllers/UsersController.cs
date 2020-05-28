using DataModels;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /*
           /users/create/
            POST - returns an ID
           /users/remove/{user_id}
            DELETE - return success/failure
           /users/find/{user_id}
            GET - returns a user with his/her details (id, and credit)
           /users/credit/{user_id}
            GET - returns the current credit of a user
           /users/credit/subtract/{user_id}/{amount}  
            POST - subtracts the amount from the credit of the user (e.g., to buy an order). Returns success or failure, depending on the credit status. 
           /users/credit/add/{user_id}/{amount}  
            POST - subtracts the amount from the credit of the user. Returns success or failure, depending on the credit status. 
        */

        private readonly IClusterClient _client;
        public UsersController(IClusterClient client)
        {
            _client = client;
        }

        [HttpPost("create")]
        [Produces("application/json")]
        public Task<Guid> CreateUser()
        {
            var id = Guid.NewGuid();
            var user = _client.GetGrain<IUserGrain>(id);

            return user.CreateUser(); //Should it be user_id : {user_id} ?
        }

        [HttpDelete("remove/{id}")]
        public Task<bool> RemoveUser(Guid id)
        {
            var user = _client.GetGrain<IUserGrain>(id);
            return user.RemoveUser();
        }

        [HttpGet("find/{id}")]
        [Produces("application/json")]
        public Task<User> GetUser(Guid id)
        {
            //What if it doesnt exist?
            //When the grain is invoked should it check the db or something if the id exists? 
            //(e.g) use OnActivateAsync(?) to check if user exists ? Need a storage provider for that.
            var user = _client.GetGrain<IUserGrain>(id);
         
            //Send ok or not found.
            return user.GetUser();
        }

        [HttpGet("credit/{id}")]
        public Task<decimal> GetCredit(Guid id)
        {
            var user = _client.GetGrain<IUserGrain>(id);
            return user.GetCredit();
        } 

        [HttpPut("credit/substract/{id}/{amount}")]
        public Task<bool> SubstractCredit(Guid id, decimal amount)
        {
            var user = _client.GetGrain<IUserGrain>(id);
            return user.ChangeCredit(-amount);
        }

        [HttpPut("credit/add/{id}/{amount}")]
        public Task<bool> AddCredit(Guid id, decimal amount)
        {
            var user = _client.GetGrain<IUserGrain>(id);
            return user.ChangeCredit(amount);
        }
       
    }
}
