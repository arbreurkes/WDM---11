using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class Order
    {
        [JsonIgnore] //Ignore this or send it too ?
        public Guid userId { get; set; } //FK of user

        public List<Stock> Items { get; } = new List<Stock>(); //Bit weird. If 3 items of N in the list, then the order has 3 items of these. This should be grouped probably in another model.

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
        public decimal Total => Items.Sum(i => i.Price);



   
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
