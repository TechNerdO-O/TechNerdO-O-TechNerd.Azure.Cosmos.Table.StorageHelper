using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Core;
using TechNerd.Azure.Cosmos.Table.StorageHelper.DTO;
using Xunit;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Tests.CoreTests
{
    public class StorageDBContextTests
    {
        StorageConfig storageConfig = new StorageConfig()
        {
            StorageAccount = "mockAccount",
            StorageKey = "mockKey"
        };
        [Fact]
        public async Task Should_Throw_SameException_From_CloudStorage_For_InvalidConfig()
        {
            StorageDBContext storageDBContext = new StorageDBContext(storageConfig);
            await Assert.ThrowsAsync<StorageException>(() => storageDBContext.GetTableAsync("mockTable"));
        }
    }
}
