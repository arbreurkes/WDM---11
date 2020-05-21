using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class Stock
    {
        [JsonProperty(PropertyName = "item_id")]
        public Guid ID { get; set; }
        [JsonProperty(PropertyName = "quantity")]
        public int? Quantity { get; set; } = null;
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        [JsonIgnore]
        public bool Exists => Quantity != null;
    }
}
