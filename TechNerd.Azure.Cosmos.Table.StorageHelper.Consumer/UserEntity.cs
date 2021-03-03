using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Core;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Consumer
{
    public class UserEntity : Entity<string>
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
