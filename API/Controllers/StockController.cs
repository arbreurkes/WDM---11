using DataModels;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OData.UriParser;
using Orleans;
using System;
using System.Threading.Tasks;

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
            var stock = _client.GetGrain<IStockGrain>(id);
            return await stock.GetAmount();
        }

        [HttpGet("find/{item_id}")]
        public Task<Stock> GetStock(Guid item_id)
        {
            var stock = _client.GetGrain<IStockGrain>(item_id);
            return stock.GetStock();
        }

        [HttpPost("substract/{id}/{number}")]
        public async Task SubstractAvailability(Guid id, int number)
        {
            
            var stock = _client.GetGrain<IStockGrain>(id);
            await stock.ChangeAmount(-number);
        }

        [HttpPost("add/{id}/{number}")]
        public async Task AddAvailability(Guid id, int number)
        {
            //Call grain, add number
            var stock = _client.GetGrain<IStockGrain>(id);
            await stock.ChangeAmount(number);
        }

        [HttpPost("item/create/{price}")]
        public async Task<Stock> AddItem(decimal price)
        {
            var item = _client.GetGrain<IStockGrain>(Guid.NewGuid());
            return await item.Create(price);
        }

    }
}
