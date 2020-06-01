using Newtonsoft.Json;
using System;

namespace DataModels
{
    public class Stock
    {
        [JsonProperty(PropertyName = "item_id")]
        public Guid? ID { get; set; } = null;
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; } = 0;
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; } = 0;

        [JsonIgnore]
        public bool Exists => ID != null;
    }
}
