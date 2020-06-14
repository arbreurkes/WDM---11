using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataModels
{
    [Serializable]
    public class Order
    {
        public Guid UserId { get; private set; } //FK of use
        public Guid ID { get; private set; }

       
        
        public Dictionary<Guid, OrderItem> Items { get; private set; } = new Dictionary<Guid, OrderItem>();
        //Serialization and API

        [IgnoreProperty]
        [JsonIgnore]
        public decimal Total => Items.Values.Sum(i => i.Quantity * i.Item.Price);

        public DateTime? CreatedAt { get; private set; } = null;
        public DateTime? CheckedOutAt { get; private set; } = null;
        //When the order is completed (the user can not cancel checkout at this point)
        public DateTime? CompletedAt { get; private set; } = null;

        //Non serializable
        [JsonIgnore]
        public bool Exists => CreatedAt != null;
        //Non serializable
        [JsonIgnore]
        public bool CheckedOut => CheckedOutAt != null;
        //Non serializable
        [JsonIgnore]
        public bool Completed => CompletedAt != null;
        [JsonIgnore]
        public bool CanCheckout => Exists && !CheckedOut && !Completed;
        [JsonIgnore]
        public bool CanComplete => Exists && CheckedOut && !Completed;


        public List<Guid> GetItems()
        {
            var list = new List<Guid>();
            foreach(var item in Items)
            {
                int qtd = item.Value.Quantity;
                for(int i = 0; i < qtd; i++)
                {
                    list.Add(item.Key);
                }
            }
            return list;
        }
        public void Create(Guid userId, Guid orderId)
        {
            this.UserId = userId;
            this.ID = orderId;
            CreatedAt = DateTime.Now;
        }

        public bool Checkout()
        {
            if (CanCheckout)
            {
                CheckedOutAt = DateTime.Now;
                return true;
            }

            return false;
        }

        public bool Complete()
        {
            if (CanComplete)
            {
                CompletedAt = DateTime.Now;
                return true;
            }

            return false;
        }

        //Delete the order?
        public bool CancelCheckout()
        {
            if (CheckedOut && !Completed)
            {
                CheckedOutAt = null;
                return true;
            }

            return false;
        }

        public bool CancelComplete()
        {
            if (Completed)
            {
                CompletedAt = null;
                return true;
            }
            return false;
        }

        public void IncQuantity(Guid id)
        {
            Items[id].IncQuantity();
        }

        public void DecQuantity(Guid id)
        {
            Items[id].DecQuantity();
        }

        public void RemoveItem(Guid id)
        {
            Items.Remove(id);
        }

        
        public OrderFormatted GetOrder()
        {
            return new OrderFormatted
            {
                UserId = UserId,
                ID = ID,
                Items = GetItems(),
                Paid = Completed,
                Total = Total
            };
        }
    }


    public class Payment
    {
        [JsonProperty("order_id")]
        public Guid ID { get; set; }
        [JsonProperty("paid")]
        public bool Paid { get; set; }
    }

    public class OrderFormatted
    {
        [JsonProperty(PropertyName = "user_id")]
        public Guid UserId { get; set; } //FK of use

        [JsonProperty(PropertyName = "order_id")]
        public Guid ID { get; set; }

        [JsonProperty(PropertyName = "items")]
        public List<Guid> Items { get; set; }

        [JsonProperty(PropertyName ="total")]
        public decimal Total { get; set; }

        [JsonProperty(PropertyName = "Paid")]
        public bool Paid { get; set; }
    
    }
}
