using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataModels
{
    [Serializable]
    public class Order
    {
        [JsonProperty(PropertyName = "user_id")]
        public Guid userId { get; private set; } //FK of use
        
        [JsonProperty(PropertyName = "order_id")]
        public Guid ID { get; private set; }
        //Should be saved and returned as a list.
        public Dictionary<Guid, OrderItem> Items { get; } = new Dictionary<Guid, OrderItem>();

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
        [JsonIgnore]
        public bool CanComplete => Exists && CheckedOut && !Completed;


        [JsonProperty(PropertyName = "total_cost")]
        public decimal Total => Items.Values.Sum(i => i.Quantity * i.Item.Price);




        public void Create(Guid userId, Guid orderId)
        {
            this.userId = userId;
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
    }

    public class Payment
    {
        [JsonProperty("order_id")]
        public Guid ID { get; set; }
        [JsonProperty("paid")]
        public bool Paid { get; set; }
    }
}
