using Newtonsoft.Json;
using System;

namespace DataModels
{
    public class Stock
    {
        [JsonProperty(PropertyName = "item_id")]
        public Guid ID { get; set; }
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; } = 0;
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; } = -1;
        [JsonProperty(PropertyName = "item_id")]


        [JsonIgnore]
        public bool Exists => Price >=0;
    }
}
