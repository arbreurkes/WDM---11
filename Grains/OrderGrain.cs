using System;
using System.Threading.Tasks;
using DataModels;
using Infrastructure.Interfaces;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grains
{
    public class OrderGrain : Grain, IOrderGrain
    {
        private readonly IPersistentState<Order> _order;

        public OrderGrain([PersistentState("order", "orderStore")]
            IPersistentState<Order> order)
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
            //_order.WriteStateAsync();

            return Task.FromResult(_order.State);
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

        public Task<bool> AddItem(Stock item)
        {
            if (!_order.State.Exists)
            {
                throw new OrderDoesNotExistsException();
            }
            
            if (item.Exists)
            {
                Guid id = item.ID;

                if (_order.State.Items.ContainsKey(id))
                {
                    _order.State.Items[id].IncQuantity();
                }
                else // catch exception and remove if?
                {
                    OrderItem oi = new OrderItem() {Item = item, Quantity = 1}; // like this? or change constructor
                    _order.State.Items.Add(id, oi);
                }
                
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> RemoveItem(Stock item)
        {
            if (!_order.State.Exists)
            {
                throw new OrderDoesNotExistsException();
            }
            
            if (item.Exists)
            {
                Guid id = item.ID;

                if (_order.State.Items.ContainsKey(id))
                {
                    try
                    {
                        _order.State.Items[id].DecQuantity();
                        return Task.FromResult(true);
                    }
                    catch (InvalidQuantityException)
                    {
                        _order.State.Items.Remove(id);
                        return Task.FromResult(false);
                    }
                }
            }

            return Task.FromResult(false);
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
            // foreach (Stock item in order.Items)
            // {
            //     //ToDo: revert stock transaction.
            // }
            return Task.FromResult(_order.State.CancelCheckout());
        }

        public Task<Guid> GetUser()
        {
            if (_order.State.Exists)
            {
                return Task.FromResult(this.GetPrimaryKey());
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