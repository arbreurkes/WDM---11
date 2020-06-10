﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataModels
{
    [Serializable]
    public class Order
    {
        [JsonProperty(PropertyName = "user_id")]
        public Guid UserId { get; private set; } //FK of use
        
        [JsonProperty(PropertyName = "order_id")]
        public Guid ID { get; private set; }
        [JsonProperty(PropertyName = "items")]
        public Dictionary<Guid, OrderItem> Items { get; } = new Dictionary<Guid, OrderItem>();

        [JsonIgnore] //Deprecated
        public List<Guid> ItemsList => Items.Keys.ToList();

        [JsonProperty(PropertyName = "total_cost")]
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
        [JsonProperty(PropertyName = "paid")]
        public bool Completed => CompletedAt != null;
        [JsonIgnore]
        public bool CanCheckout => Exists && !CheckedOut && !Completed;

        public void Reset()
        {
            CreatedAt = CheckedOutAt = CompletedAt = null;
            this.UserId = this.ID = Guid.Empty;
            this.Items.Clear();
        }

        [JsonIgnore]
        public bool CanComplete => Exists && CheckedOut && !Completed;

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
                CompletedAt = DateTime.Now;
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
    }


    public class Payment
    {
        [JsonProperty("order_id")]
        public Guid ID { get; set; }
        [JsonProperty("paid")]
        public bool Paid { get; set; }
    }
}
