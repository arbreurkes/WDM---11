using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using OrleansBasics;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        // /payment/pay/{user_id}/{order_id}
        // /payment/cancel/{user_id}/{order_id}
        // /payment/status/{order_id}

        private readonly IClusterClient _client;

        public PaymentController(IClusterClient client)
        {
            _client = client;
        }

        [HttpPost("pay/{user_id}/{order_id}")]
        public async Task<bool> Pay(Guid user_id, Guid order_id)
        {
            var user = _client.GetGrain<IUserGrain>(user_id);
            var order = _client.GetGrain<IOrderGrain>(order_id);
            var total = await order.GetTotalCost();

            if (await user.ChangeCredit(-total)) return await order.Complete();

            return false;
        }

        [HttpPost("cancel/{user_id}/{order_id}")]
        public async Task<bool> CancelPayment(Guid user_id, Guid order_id)
        {
            var user = _client.GetGrain<IUserGrain>(user_id);
            var order = _client.GetGrain<IOrderGrain>(order_id);
            var total = await order.GetTotalCost();
           
            if(await order.CancelCheckout())
            {
                return await user.ChangeCredit(total);
            }
            
            return false;
        }

        [HttpGet("status/{order_id}")]
        public async Task<string> GetStatus(Guid order_id)
        {
            var order = _client.GetGrain<IOrderGrain>(order_id);
            bool status = await order.GetStatus();
            
            return await Task.FromResult("{'paid': '" + status + "'}"); //This is converting the string itself, "paid" is not being converted as an attribute.
        }
    }
}