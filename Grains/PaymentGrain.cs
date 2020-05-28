using DataModels;
using Infrastructure.Interfaces;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Grains
{
    public class PaymentGrain : Orleans.Grain, IPaymentGrain
    {
        Payment payment = new Payment();

        public Task<Guid> CreatePayment(Guid userId, Guid orderId, decimal total)
        {
            payment.Create(userId, orderId, total);
            return Task.FromResult(this.GetPrimaryKey());
        }

        public Task<bool> CancelPayment()
        {
            bool result = false;

            if (payment.Exists)
            {
                payment = new Payment();
                result = true;
            }

            return Task.FromResult(result);
        }

        public Task<bool> CompletePayment()
        {
            bool result = false;
            
            if (payment.Exists)
            {
                payment.CompletedAt = DateTime.Now;
                result = true;
            }

            return Task.FromResult(result);
        }

        public Task<Payment> GetPayment()
        {
            if (payment.Exists)
            {
                return Task.FromResult(payment);
            } 
            
            return Task.FromResult<Payment>(null);
        }
    }
}