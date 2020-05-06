using DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrleansBasics
{
    public class OrderGrain : Orleans.Grain, IOrderGrain
    {
        Order order = new Order();

        public Task AddItem()
        {
            throw new NotImplementedException();
        }
        public Task RemoveItem()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrder()
        {
            return Task.Factory.StartNew(() => order);
        }
    }
}
