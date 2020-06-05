using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    [Serializable]
    public class OrderItem
    {
        [JsonProperty(PropertyName = "item")]
        public Stock Item { get; set; } //The available quantity should not be returned. Right ?
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        public void IncQuantity()
        {
            Quantity += 1;
        }

        public void DecQuantity()
        {
            if (Quantity - 1 < 0)
            {
                throw new InvalidQuantityException();
            }
            
            Quantity -= 1;
        }
    }
}
