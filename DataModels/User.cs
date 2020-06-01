using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DataModels
{
    public class User
    {
        [JsonProperty(PropertyName = "credit")]
        public decimal Credit { get; set; } = 0;

        public DateTime? CreatedAt { get; private set; } = null;
       

        [JsonIgnore]
        public bool Exists => CreatedAt != null;

        public Boolean Create()
        {
            if (!Exists)
            {
                CreatedAt = DateTime.Now;
                return true;
            }

            return false;
        }
    }
}
