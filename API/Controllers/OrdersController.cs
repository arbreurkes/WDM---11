using DataModels;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using ShoppingCart;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IClusterClient _client;
        private readonly HttpClient _httpClient;
        public OrdersController(IClusterClient client)
        {
            _client = client;
            _httpClient = Program.HttpClient;

        }

        [HttpPost("create/{user_id}")]
        public async Task<Order> CreateOrder(Guid user_id)
        {
            var orderId = Guid.NewGuid();
            await _client.GetGrain<IUserGrain>(user_id).GetUser(); //if user does not exist an exception is thrown
            var order = _client.GetGrain<IOrderGrain>(orderId);
            return await order.CreateOrder(user_id);
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
            // Should receive the item_id? The item itself or the grain?
            var item = _client.GetGrain<IStockGrain>(item_id);
            await order.AddItem(await item.GetStock());
        }

        [HttpDelete("removeitem/{order_id}/{item_id}")]
        public async Task RemoveItem(Guid order_id, Guid item_id)
        {
            var order = _client.GetGrain<IOrderGrain>(order_id);
            var item = _client.GetGrain<IStockGrain>(item_id);
            await order.RemoveItem(await item.GetStock());
        }

        [HttpPost("checkout/{order_id}")]
        public async Task Checkout(Guid order_id)
        {
            var order = _client.GetGrain<IOrderGrain>(order_id);
            var result = await order.Checkout();


            if (result)
            {
                var user_id = await order.GetUser();

                var total_cost = await order.GetTotalCost();
                var res = await _httpClient.PostAsync("http:localhost:5001//users/pay/" + user_id + "/" + total_cost,null);
                if (!res.IsSuccessStatusCode){
                    await order.CancelCheckout();
                    throw new NotEnoughCreditException();
                }
              
                var items = await order.GetItems();

                List<OrderItem> success = new List<OrderItem>();
                //remove stocks..
                foreach(var orderItem in items)
                {
                   

                        var rest = await _httpClient.PostAsync("http:localhost:5001//substract/"+orderItem.Item.ID + "/" + orderItem.Quantity, null);

                        if (!rest.IsSuccessStatusCode)
                        {

                            foreach(var item in success)
                            {
                                await _httpClient.PostAsync("localhost:5001//add/" + orderItem.Item.ID + "/" + orderItem.Quantity, null);
                            }

                            throw new StockDoesNotExistsException();
                        }
                        success.Add(orderItem);
                    
                   
                }
            }

        }

        [HttpPost("complete/{order_id}")]
        public async Task CompleteTask(Guid order_id)
        {
            var result = await _client.GetGrain<IOrderGrain>(order_id).Complete();
            if (!result)
            {
                throw new Exception();
            }
        }

    }
}