using Newtonsoft.Json;
using System;

namespace DataModels
{
    [Serializable]
    public class Stock
    {
        [JsonProperty(PropertyName = "item_id")]
        public Guid ID { get; set; }
        [JsonProperty(PropertyName = "stock")]
        public int Quantity { get; set; } = 0;
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; } = -1;

        [JsonIgnore]
        public bool Exists => Price >= 0;
    }
}
