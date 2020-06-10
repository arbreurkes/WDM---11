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
        private readonly IClusterClient _client;
        public UsersController(IClusterClient client)
        {
            _client = client;
        }

        [HttpPost("create")]
        [Produces("application/json")]
        public async Task<User> CreateUser()
        {
            var id = Guid.NewGuid();
            var user = _client.GetGrain<IUserGrain>(id);

            var new_user = await user.CreateUser();
            return new_user;
        }

        [HttpDelete("remove/{user_id}")]
        public async Task RemoveUser(Guid user_id)
        {
            var user = _client.GetGrain<IUserGrain>(user_id);
            await user.RemoveUser();
        }

        [HttpGet("find/{user_id}")]
        [Produces("application/json")]
        public async Task<User> GetUser(Guid user_id)
        {
            var user = _client.GetGrain<IUserGrain>(user_id);
            return await user.GetUser();
        }

        [HttpGet("credit/{id}")]
        public async Task<decimal> GetCredit(Guid id)
        {
            var user = _client.GetGrain<IUserGrain>(id);
            return await user.GetCredit();
        } 

        [HttpPost("credit/substract/{id}/{amount}")]
        public async Task SubstractCredit(Guid id, decimal amount)
        {
            var user = _client.GetGrain<IUserGrain>(id);
            await user.ChangeCredit(-amount);
        }

        [HttpPost("credit/add/{user_id}/{amount}")]
        public async Task AddCredit(Guid user_id, decimal amount)
        {
            var user = _client.GetGrain<IUserGrain>(user_id);
            await user.ChangeCredit(amount);
        }
       
    }
}
