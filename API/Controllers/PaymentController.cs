using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace API.Controllers
{
    [Route("api/[controller]")]
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
        public Task<bool> Pay(Guid user_id, Guid order_id)
        {
            //POST - subtracts the amount of the order from the user’s credit(returns failure if credit is not enough)
            //Payment grain  needed?
            return Task.FromResult(true);
        }
        [HttpPost("cancel/{user_id}/{order_id}")]
        public void CancelPayment(Guid user_id, Guid order_id)
        {
            //POST - cancels payment made by a specific user for a specific order.
            //How to "cancel" the payment? Change its status? Remove it?
            //Grain needed?
        }
        [HttpGet("status/{order_id}")]
        public Task<string> GetStatus(Guid order_id)
        {
            //GET - returns the status of the payment (paid or not)
            return Task.FromResult("paid");
        }
    }
}
