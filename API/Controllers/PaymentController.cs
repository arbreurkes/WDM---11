using DataModels;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Orleans;
using ShoppingCart;
using System;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
   
        private readonly IClusterClient _client;
        private readonly HttpClient _httpclient;
        public PaymentController(IClusterClient client)
        {
            _client = client;
            _httpclient = Program.HttpClient;
        }

        [HttpPost("pay/{user_id}/{order_id}/{amount}")]
        public async Task Pay(Guid user_id, Guid order_id,decimal amount)
        {
            var user = _client.GetGrain<IUserGrain>(user_id);
            
            

            await user.ChangeCredit(-amount);
        }

        [HttpPost("cancel/{user_id}/{order_id}")]
        public async Task CancelPayment(Guid user_id, Guid order_id)
        {
            //???
            var user = _client.GetGrain<IUserGrain>(user_id);
            await user.GetUser();
            var orderDetails = await _httpclient.GetAsync("http:localhost:5001//orders/find//order_id");
            if (!orderDetails.IsSuccessStatusCode)
            {
                throw new Exception(); //??
            }
            //get total from order
            var content = await orderDetails.Content.ReadAsStringAsync();
            Order order = (Order)JsonConvert.DeserializeObject(content);
            await user.ChangeCredit(order.Total);
            await _httpclient.PostAsync("localhost:5001//orders/complete//order_id",null);
            //complete order.

        }

        [HttpGet("status/{order_id}")]
        public async Task<Payment> GetStatus(Guid order_id)
        {
            var order = _client.GetGrain<IOrderGrain>(order_id);

            return await order.GetStatus();
        }
    }
}