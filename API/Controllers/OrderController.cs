using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using OrleansBasics;
using DataModels;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IClusterClient _client;
        public OrderController(IClusterClient client)
        {
            _client = client;
        }
        [HttpPost("create/{id}")]
        public string CreateOrder(Guid id)
        {

            var user = _client.GetGrain<IUserGrain>(id);
            return user.NewOrder() +""; //Not the most elegant way to convert
        }

        [HttpDelete("remove/{id}")]
        public void DeletesOrder(Guid id)
        {
            
            //Delete order -> Remove order from user
       
        }
        [HttpGet("find/{id}")]
        public Task<Order> GetOrderDetails(Guid id)
        {
            return _client.GetGrain<IOrderGrain>(id).GetOrder();
        }
        [HttpPost("additem/{order_id}/{item_id}")]
        public void AddItem(Guid orderId, Guid itemId)
        {
            var order = _client.GetGrain<IOrderGrain>(orderId);
            //Should receive the item_id ? The item itself or the grain?
            order.AddItem();

        }
        [HttpDelete("removeitem/{order_id}/{item_id}")]
        public void RemoveItem(Guid order_id, Guid item_id)
        {
            var order = _client.GetGrain<IOrderGrain>(order_id);
            //Should receive the item_id ? The item itself or the grain?
            order.RemoveItem();

        }
        [HttpPost("checkout/{id}")]
        public Task<bool> Checkout(Guid id)
        {
            //Call payment service, should it call the service itself or a grain ? Should payments be grains?
            return Task.FromResult(true);
        }

    }
}
