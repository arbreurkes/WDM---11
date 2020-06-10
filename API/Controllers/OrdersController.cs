﻿using DataModels;
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

        [HttpPost("create/{user_id}")]
        public async Task<Order> CreateOrder(Guid user_id)
        {
            var orderId = Guid.NewGuid();
            await _client.GetGrain<IUserGrain>(user_id).GetUser(); //if user does not exist an exception is thrown
            var order = _client.GetGrain<IOrderGrain>(orderId);
            return await order.CreateOrder(user_id);
        }
    
        [HttpDelete("remove/{id}")]
        public async Task RemoveOrder(Guid id)
        {
            
            var order = _client.GetGrain<IOrderGrain>(id);
            await order.RemoveOrder();
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
            await order.Checkout();

         
        }

    }
}