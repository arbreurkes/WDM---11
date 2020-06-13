using System;
using System.Threading.Tasks;
using DataModels;
using Infrastructure.Interfaces;
using Orleans;
using Orleans.Runtime;
using System.Collections.Generic;

namespace Grains
{
    public class OrderGrain : Grain, IOrderGrain
    {
        private readonly IPersistentState<Order> _order;

        public OrderGrain([PersistentState("order", "orderStore")] IPersistentState<Order> order)
        {
            _order = order;
        }

        /// <summary>
        /// Create and order and save it to Azure Table Storage provider (defined in program.cs)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<Order> CreateOrder(Guid userId)
        {
            _order.State.Create(userId, this.GetPrimaryKey());
            _order.WriteStateAsync();
            return Task.FromResult(_order.State);
        }

        public Task<bool> RemoveOrder()
        {
            if (!_order.State.Exists)
            {
                throw new OrderDoesNotExistsException();
            }
             
            _order.State = new Order(); // resets timestamp
            _order.ClearStateAsync();
            this.DeactivateOnIdle(); //Deactive the grain.
            return Task.FromResult(true);
        }

        /// <summary>
        /// If the grain can be found in the memory, this method returns it
        /// </summary>
        /// <returns></returns>
        public Task<Order> GetOrder()
        {
            if (!_order.State.Exists)
            { 
                throw new OrderDoesNotExistsException();
            }

            return Task.FromResult(_order.State);
        }

        public Task<bool> AddItem(Stock item)
        {
            if (!_order.State.Exists)
            {
                throw new OrderDoesNotExistsException();
            }

            if (!item.Exists)
            {
                throw new ItemDoesNotExistException();
            }

            Guid id = item.ID;

            if (_order.State.Items.ContainsKey(id))
            {
                _order.State.IncQuantity(id);
            }

            else
            {
                OrderItem oi = new OrderItem() { Item = item, Quantity = 1 };
                _order.State.Items.Add(id, oi);
            }

            // _order.WriteStateAsync();
            return Task.FromResult(true);
        }

        public Task<bool> RemoveItem(Stock item)
        {
            if (!_order.State.Exists)
            {
                throw new OrderDoesNotExistsException();
            }

            if (!item.Exists)
            {
                throw new ItemDoesNotExistException();
            }

            Guid id = item.ID;

            if (!_order.State.Items.ContainsKey(id))
            {
                throw new ItemNotInOrderException();
            }

            try
            {
                _order.State.DecQuantity(id);
            }

            catch (InvalidQuantityException)
            {
                _order.State.RemoveItem(id);
            }

            // _order.WriteStateAsync();
            return Task.FromResult(true);
        }

        public Task<decimal> GetTotalCost()
        {
            if (!_order.State.Exists)
            {
                throw new OrderDoesNotExistsException();
            }

            _order.WriteStateAsync();

            return Task.FromResult(_order.State.Total);
        }

        public Task<Payment> GetStatus()
        {
            if (!_order.State.Exists)
            {
                throw new OrderDoesNotExistsException();
            }

            return Task.FromResult(new Payment { ID = this.GetPrimaryKey(), Paid = _order.State.Completed });
        }

        public Task<bool> Checkout()
        {
            // foreach (Stock item in order.Items)
            // {
            //     //ToDo: subtract stock.
            // }
            return Task.FromResult(_order.State.Checkout());
        }

        //Complete === Checkout && Paid
        public Task<bool> Complete()
        {
            return Task.FromResult(_order.State.Complete());
        }

        public Task<bool> CancelCheckout()
        {
            if (!_order.State.CancelCheckout())
            {
                throw new OrderDoesNotExistsException();
            }
            return Task.FromResult(true);
        }

        public Task<Guid> GetUser()
        {
            if (_order.State.Exists)
            {
                return Task.FromResult(this._order.State.UserId);
            }

            throw new OrderDoesNotExistsException();
        }

        public Task<List<OrderItem>> GetItems()
        {
            return Task.FromResult(new List<OrderItem>(_order.State.Items.Values));
        }

        public Task<bool> CancelComplete()
        {
            return Task.FromResult(_order.State.CancelComplete());
        }

        public Task<bool> ClearOrder()
        {
            _order.ClearStateAsync();
            return Task.FromResult(true);
        }
    }
}