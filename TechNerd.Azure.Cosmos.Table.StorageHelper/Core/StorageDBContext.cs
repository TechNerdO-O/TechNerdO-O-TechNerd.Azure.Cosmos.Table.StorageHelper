using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;
using TechNerd.Azure.Cosmos.Table.StorageHelper.DTO;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Interfaces;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Core
{
    public class StorageDBContext : IStorageDBContext
    {
        private StorageConfig _config;
        public StorageDBContext(StorageConfig config)
        {
            _config = config;
        }
        public async Task<CloudTable> GetTableAsync(string tableName)
        {
            //Account  
            CloudStorageAccount storageAccount = new CloudStorageAccount(
                new StorageCredentials(_config.StorageAccount, _config.StorageKey), false);

            //Client  
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            //Table  
            CloudTable table = tableClient.GetTableReference(tableName);
            //await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}
