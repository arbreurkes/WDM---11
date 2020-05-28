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
    public class OrdersController : ControllerBase
    {
        private readonly IClusterClient _client;

        public OrdersController(IClusterClient client)
        {
            _client = client;
        }

        [HttpPost("create/{id}")]
        public async Task<Guid> CreateOrder(Guid id)
        {
            var orderId = Guid.NewGuid();
            var order = _client.GetGrain<IOrderGrain>(orderId);
            return await order.CreateOrder(id);
        }

        [HttpDelete("remove/{id}")]
        public async Task<bool> RemoveOrder(Guid id)
        {
            //Delete order -> Remove order from user // For now user doesn't have orders
            var order = _client.GetGrain<IOrderGrain>(id);
            return await order.RemoveOrder();
        }

        [HttpGet("find/{id}")]
        public async Task<Order> GetOrderDetails(Guid id)
        {
            var order = _client.GetGrain<IOrderGrain>(id);
            return await order.GetOrder();
        }

        [HttpPost("additem/{order_id}/{item_id}")]
        public async Task AddItem(Guid order_id, Guid item_id)
        {
            var order = _client.GetGrain<IOrderGrain>(order_id);
            //Should receive the item_id ? The item itself or the grain?
            var item = _client.GetGrain<IStockGrain>(item_id);

            order.AddItem(await item.GetStock());

        }
        [HttpDelete("removeitem/{order_id}/{item_id}")]
        public async Task RemoveItem(Guid order_id, Guid item_id)
        {
            var order = _client.GetGrain<IOrderGrain>(order_id);
            //Should receive the item_id ? The item itself or the grain?
            var item = _client.GetGrain<IStockGrain>(item_id);
            order.RemoveItem(await item.GetStock());


        }
        [HttpPost("checkout/{id}")]
        public async Task<bool> Checkout(Guid id)
        {
            var order = _client.GetGrain<IOrderGrain>(id);

            var result = await order.Checkout();
            //Cancel checkout if something goes wrong.
            
            if (result)
            {
                var user_id = await order.GetUser();

                //pay
                await _client.GetGrain<IUserGrain>(user_id).ChangeCredit(-await order.GetTotalCost());

                //remove from stock
            }
            return result;
        }

    }
}
