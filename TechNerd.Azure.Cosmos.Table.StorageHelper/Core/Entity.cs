using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Interfaces;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Core
{
    public abstract class Entity<TKey> : TableEntity, IEntity<TKey>
    {
        [JsonProperty("id")]
        public TKey Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        protected Entity()
        {
            CreatedOn = DateTime.UtcNow;
        }
    }
}
