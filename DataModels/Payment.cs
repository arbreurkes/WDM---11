using System;
using Newtonsoft.Json;

namespace DataModels
{
    //NOT USED.
    public class Payment
    {
        public Guid userId { get; set; }
        public Guid orderId { get; set; }
        
        public decimal total { get; set; }
        
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? CompletedAt { get; set; } = null;
        
        //Non serializable
        [JsonIgnore]
        public bool Exists => CreatedAt != null;
        //Non serializable
        [JsonIgnore]
        public bool Completed => CompletedAt != null;

        public void Create(Guid userId, Guid orderId, decimal total)
        {
            this.userId = userId;
            this.orderId = orderId;
            this.total = total;
            CreatedAt = DateTime.Now;
        }

        public void Complete()
        {
            CompletedAt = DateTime.Now;
        }
    }
}
