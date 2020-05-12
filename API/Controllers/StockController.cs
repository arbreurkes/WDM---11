﻿using System;
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
    public class StockController : ControllerBase
    {

        private readonly IClusterClient _client;
        public StockController(IClusterClient client)
        {
            _client = client;
        }
        [HttpGet("availability/{id}")]
        public async Task<int> GetAvailability(Guid id)
        {

            //Get item grain and ask for its availability ?
            //Should the item grain have a stock class that holds the item information and its quantity?

            //Is the availability the quantity or if its available or not?

            var stock = _client.GetGrain<IStockGrain>(id);

            return await stock.GetAmount();
        }

        [HttpPost("substract/{id}/{number}")]
        public void SubstractAvailability(Guid id,int number)
        {

            var stock = _client.GetGrain<IStockGrain>(id);

            stock.ChangeAmount(-number);
            //Call grain, substract number
       
        }

        [HttpPost("add/{id}/{number}")]
        public void AddAvailability(Guid id, int number)
        {

            //Call grain, add number
            var stock = _client.GetGrain<IStockGrain>(id);

            stock.ChangeAmount(number);
        }
        [HttpPost("item/create")]
        public Task<string> AddItem([FromBody] Stock body)
        {
            var item = _client.GetGrain<IStockGrain>(Guid.NewGuid());
            item.Create(body.Description);
            return Task.FromResult(item.GetPrimaryKey() + ""); //Again, not the most elegant way to convert the object.

        }
      
    }
}
