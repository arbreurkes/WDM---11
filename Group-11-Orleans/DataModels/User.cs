using Newtonsoft.Json;
using System;

namespace DataModels
{
    public class User
    {
        [JsonProperty(PropertyName = "user_id")]
        public Guid ID { get; set; }
        [JsonProperty(PropertyName = "credit")]
        public decimal Credit { get; set; } = 0;

        public DateTime? CreatedAt { get; private set; } = null;


        [JsonIgnore]
        public bool Exists => CreatedAt != null;
        
        public Boolean Create(Guid id)
        {
            if (!Exists)
            {
                ID = id;
                CreatedAt = DateTime.Now;
                return true;
            }

            return false;
        }
    }
}
