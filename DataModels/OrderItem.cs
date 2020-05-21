using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
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
            Quantity -= 1;

            if (Quantity < 1)
            {
                throw new InvalidQuantityException();
            }
        }
    }
}
