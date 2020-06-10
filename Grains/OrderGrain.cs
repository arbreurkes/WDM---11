using System;
using System.Threading.Tasks;
using DataModels;
using Infrastructure.Interfaces;
using Orleans;
using Orleans.Runtime;
using System.Collections.Generic;
using Orleans.Transactions;
using Orleans.Transactions.Abstractions;

namespace Grains
{
    public class OrderGrain : Grain, IOrderGrain
    {
        private readonly IClusterClient _client;
        private readonly ITransactionalState<Order> _torder;
        public OrderGrain( IClusterClient client,[TransactionalState("torder", "transactionStore")]ITransactionalState<Order> torder)
        {
            _client = client;
            _torder = torder;
        }

        /// <summary>
        /// Create and order and save it to Azure Table Storage provider (defined in program.cs)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Order> CreateOrder(Guid userId)
        {

            await _client.GetGrain<IUserGrain>(userId).GetUser();
           

            await _torder.PerformUpdate(i => i.Create(userId, this.GetPrimaryKey()));
            return await _torder.PerformRead(i => i);
        }

        public async Task RemoveOrder()
        {
            if (!(await _torder.PerformRead(i=> i.Exists)))
            {
                throw new OrderDoesNotExistsException();
            }
             
           
            this.DeactivateOnIdle();
            await _torder.PerformUpdate(i => i.Reset());
        }

        /// <summary>
        /// If the grain can be found in the memory, this method returns it
        /// </summary>
        /// <returns></returns>
        public async Task<Order> GetOrder()
        {
            if (!(await _torder.PerformRead(i=> i.Exists)))
            { 
                throw new OrderDoesNotExistsException();
            }

            return await _torder.PerformRead(i => i);
        }

        public async Task AddItem(Stock item)
        {
            if (!(await _torder.PerformRead(i => i.Exists)))
            {
                throw new OrderDoesNotExistsException();
            }

            if (!item.Exists)
            {
                throw new ItemDoesNotExistException();
            }

            Guid id = item.ID;
           
            if (await _torder.PerformRead(i => i.Items.ContainsKey(id)))
            {
                await _torder.PerformUpdate(i => i.IncQuantity(id));
            }

            else
            {
                OrderItem oi = new OrderItem() { Item = item, Quantity = 1 };
                await _torder.PerformUpdate( i=> i.Items.Add(id, oi));
            }

           
         
        }

        public async Task RemoveItem(Stock item)
        {
            if (!(await _torder.PerformRead(i => i.Exists)))
            {
                throw new OrderDoesNotExistsException();
            }

            if (!item.Exists)
            {
                throw new ItemDoesNotExistException();
            }


            Guid id = item.ID;

            if (await _torder.PerformRead(i => i.Items.ContainsKey(id))) 
            { 

                throw new ItemNotInOrderException();
            }

            try
            {
                await _torder.PerformUpdate(i => i.DecQuantity(id));
            }

            catch (InvalidQuantityException)
            {
                await _torder.PerformUpdate(i => i.RemoveItem(id));
            }

           
          
        }

        public async Task<decimal> GetTotalCost()
        {
            if (!(await _torder.PerformRead(i => i.Exists)))
            {
                throw new OrderDoesNotExistsException();
            }


            return await _torder.PerformRead(i => i.Total);
        }

        public async Task<Payment> GetStatus()
        {
            if(!(await _torder.PerformRead(i => i.Exists)))
            {
                throw new OrderDoesNotExistsException();
            }


            return new Payment { ID = this.GetPrimaryKey(), Paid = await _torder.PerformRead(i=>i.Completed) };
        }
        public async Task<bool> Checkout()
        {
            var user_id = await _torder.PerformRead(i => i.UserId); 
            var total = await _torder.PerformRead(i => i.Total);
            await _client.GetGrain<IUserGrain>(user_id).ChangeCredit(-total);

            var items = await _torder.PerformRead(i => i.Items);
            foreach (var item in items)
            {
                    var id = item.Key;
                    var qtd = item.Value.Quantity;
                    await _client.GetGrain<IStockGrain>(id).ChangeAmount(-qtd);
            }
            var result =  _torder.PerformUpdate(i => i.Checkout());

            
            this.DeactivateOnIdle();

            return await result;
        }

        //Complete === Checkout && Paid
        public Task<bool> Complete()
        {
           
            this.DeactivateOnIdle();

            return _torder.PerformUpdate(i =>i.Complete());
        }

        public async Task<bool> CancelCheckout()
        {
            if (!(await _torder.PerformRead(i => i.Exists)))
            {
                throw new OrderDoesNotExistsException();
            }

            //What should this do

            return true;
        }

        public async Task<Guid> GetUser()
        {
            if (!(await _torder.PerformRead(i => i.Exists)))
            {
                throw new OrderDoesNotExistsException();
            }


            return await _torder.PerformRead(i => i.UserId);
        }

        public async Task<List<OrderItem>> GetItems()
        {
            return new List<OrderItem>(await _torder.PerformRead(i =>i.Items.Values));
        }

        public Task<bool> CancelComplete()
        {
            return _torder.PerformRead(i =>i.CancelComplete());
        }

        
    }
}