using DataModels;
using Infrastructure.Interfaces;
using Orleans;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace OrleansBasics
{
    public class OrderGrain : Grain, IOrderGrain
    {
        private readonly IPersistentState<Order> _order;

        public OrderGrain([PersistentState("order", "orderStore")] IPersistentState<Order> order )
        {
            _order = order;
        }

        /// <summary>
        /// Create and order and save it to Azure Table Storage provider (defined in program.cs)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<Guid> CreateOrder(Guid userId) //userId or IUserGrain?
        {
            try
            {
                _order.State.Create(userId);
                _order.WriteStateAsync();

                return Task.FromResult(this.GetPrimaryKey());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public Task<bool> RemoveOrder()
        {
            bool result = false;

            if (_order.State.Exists)
            {
                _order.State = new Order(); // resets timestamp
                result = true;
            }

            return Task.FromResult(result);
        }

        /// <summary>
        /// If the grain can be found in the memory, this method returns it
        /// </summary>
        /// <returns></returns>
        public Task<Order> GetOrder()
        {
            if (_order.State.Exists)
            {
                return Task.FromResult(_order.State);
            }
            else
            {
                throw new OrderDoesNotExistsException();
            }
        }

        public void AddItem(Stock item)
        {
            Guid id = item.ID;

            if (_order.State.Items.ContainsKey(id))
            {
                _order.State.Items[id].IncQuantity(); // reference or copy?
            }
            else // catch exception and remove if?
            {
                OrderItem oi = new OrderItem() { Item = item }; // like this? or change constructor
                _order.State.Items.Add(id, oi);
            }

        }

        public void RemoveItem(Stock item)
        {
            Guid id = item.ID;

            if (_order.State.Items.ContainsKey(id))
            {
                try
                {
                    _order.State.Items[id].DecQuantity();
                }
                catch (InvalidQuantityException)
                {
                    _order.State.Items.Remove(id);
                }
            }

            // what if item was not ordered?
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

        public Task<bool> GetStatus()
        {
            return Task.FromResult(_order.State.Exists && _order.State.Completed);
        }

        public Task<bool> Checkout()
        {
            if (!_order.State.CanCheckout) return Task.FromResult(false);

            // foreach (Stock item in order.Items)
            // {
            //     //ToDo: subtract stock.
            // }

            _order.State.Checkout();

            return Task.FromResult(true);
        }

        //Complete === Checkout ?
        public Task<bool> Complete()
        {
            _order.State.Complete();

            return Task.FromResult(true);
        }

        public Task<bool> CancelCheckout()
        {
            if (!_order.State.CheckedOut) return Task.FromResult(false);

            // foreach (Stock item in order.Items)
            // {
            //     //ToDo: revert stock transaction.
            // }

            _order.State.CancelCheckout();

            return Task.FromResult(true);
        }

        public Task<Guid> GetUser()
        {
            if (_order.State.Exists)
            {
                return Task.FromResult(this.GetPrimaryKey());
            }
            throw new OrderDoesNotExistsException();
        }
    }
}