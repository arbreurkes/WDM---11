﻿using System;
using System.Threading.Tasks;
using DataModels;
using Orleans;

namespace Infrastructure.Interfaces
{
    //NOT USED
    public interface IPaymentGrain : IGrainWithGuidKey
    {
        Task<Guid> CreatePayment(Guid userId, Guid orderId, decimal total);

        Task<bool> CancelPayment();

        Task<bool> CompletePayment();

        Task<Payment> GetPayment();
    }
}