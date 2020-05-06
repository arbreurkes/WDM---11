using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using OrleansBasics;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /*
         * /users/create/
             POST - returns an ID
           /users/remove/{user_id}
            DELETE - return success/failure
           /users/find/{user_id}
            GET - returns a set of users with their details (id, and credit)
           /users/credit/{user_id}
            GET - returns the current credit of a user
           /users/credit/subtract/{user_id}/{amount}  
            POST - subtracts the amount from the credit of the user (e.g., to buy an order). Returns success or failure, depending on the credit status. 
           /users/credit/add/{user_id}/{amount}  
            POST - subtracts the amount from the credit of the user. Returns success or failure, depending on the credit status. 

         */

        private readonly IClusterClient _client;
        public UserController(IClusterClient client)
        {
            _client = client;
        }
      
      
        [HttpGet("find/{id}", Name = "Get")]
        public Task<User> Get(Guid id)
        {
            var user = _client.GetGrain<IUserGrain>(id);
            return user.GetUser();
        }

        // POST: User/create
        [HttpPost("create")]
        public string Post()
        {
            var newguid = Guid.NewGuid();
            //Do something..

            //Convert to string needed..
            return newguid + "";
        }

        [HttpGet("credit/{id}")]
        public Task<decimal> GetCredit(Guid id)
        {
            var user = _client.GetGrain<IUserGrain>(id);
            return user.GetCredit();
        } 

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            
        }
        [HttpPut("credit/substract/{id}/{amount}")]
        public Task<bool> SubstractCredit(Guid id,decimal amount)
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
