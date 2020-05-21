using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataModels
{
    public class Order
    {
        [JsonIgnore] //Ignore this or send it too ?
        public Guid userId { get; set; } //FK of use

        public Dictionary<Guid, OrderItem> Items { get; } = new Dictionary<Guid, OrderItem>();

        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? CheckedOutAt { get; set; } = null;
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
