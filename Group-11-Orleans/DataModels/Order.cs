using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataModels
{
    [Serializable]
    public class Order
    {
        [JsonProperty(PropertyName = "user_id")]
        public Guid userId { get; set; } //FK of use

        //Should be saved and returned as a list.
        [JsonIgnore]
        public Dictionary<Guid, OrderItem> Items { get; } = new Dictionary<Guid, OrderItem>();

        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? CheckedOutAt { get; set; } = null;
        //When the order is completed (the user can not cancel checkout at this point)
        public DateTime? CompletedAt { get; set; } = null;

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

        [JsonProperty(PropertyName = "total_cost")]
        public decimal Total => Items.Values.Sum(i => i.Quantity * i.Item.Price);
 
        public void Create(Guid userId)
        {
            this.userId = userId;
            CreatedAt = DateTime.Now;
        }

        public void Checkout()
        {
            CheckedOutAt = DateTime.Now;
        }

        //Not used for now
        public void Complete()
        {
            CompletedAt = DateTime.Now;
        }
        
        public void CancelCheckout()
        {
            CheckedOutAt = null;
        }
    }
}
