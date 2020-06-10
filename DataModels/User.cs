using Newtonsoft.Json;
using System;
using System.Net;

namespace DataModels
{
    public class User
    {
        [JsonProperty(PropertyName = "user_id")]
        public Guid ID { get; private set; }
        [JsonProperty(PropertyName = "credit")]
        public decimal Credit { get; set; } = 0;

        public DateTime? CreatedAt { get; private set; } = null;


        [JsonIgnore]
        public bool Exists => CreatedAt != null;
        
        public void Create(Guid id)
        {
          
           
            ID = id;
            CreatedAt = DateTime.Now;
               
           
        }

        public void Reset()
        {
            CreatedAt = null;
            Credit = 0;
            
        }
    }
}
