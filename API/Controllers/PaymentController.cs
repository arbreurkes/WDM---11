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
            await user.GetUser();
            var order = _client.GetGrain<IOrderGrain>(order_id);
            var total = await order.GetTotalCost();

            if (await order.CancelComplete())
            {
                return await user.ChangeCredit(total);
            }

            return false;
        }

        [HttpGet("status/{order_id}")]
        public async Task<Payment> GetStatus(Guid order_id)
        {
            var order = _client.GetGrain<IOrderGrain>(order_id);

            return await order.GetStatus();
        }
    }
}