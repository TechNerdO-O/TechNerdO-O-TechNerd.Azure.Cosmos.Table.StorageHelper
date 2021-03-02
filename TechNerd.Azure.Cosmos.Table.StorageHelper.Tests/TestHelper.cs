using System;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Core;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Tests
{
    public class TestHelper
    {
        public TestEntity GetEntity()
        {
            TestEntity testEntity = new TestEntity()
            {
                Id = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                Email = "sampleuser@gmail.com",
                Name = "sampleuser"
            };
            return testEntity;
        }

        public class NullableEntity : Entity<string>
        {

        }
        public class TestEntity : Entity<Guid>
        {
            public string Email { get; set; }
            public string Name { get; set; }
        }
    }
}
