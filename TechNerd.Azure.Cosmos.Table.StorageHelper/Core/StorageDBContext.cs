using Microsoft.Azure.Cosmos.Table;
using System;
using System.Net;
using System.Threading.Tasks;
using TechNerd.Azure.Cosmos.Table.StorageHelper.DTO;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Helpers;
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
        public async Task<DBContextResult> GetTableAsync(string tableName)
        {
            CloudTable table;
            try
            {
                //Account
                CloudStorageAccount storageAccount = new CloudStorageAccount(
                        new StorageCredentials(_config.StorageAccount, _config.StorageKey), true);
                //Client  
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                //Table  
                table = tableClient.GetTableReference(tableName);
                await table.CreateIfNotExistsAsync();
            }
            catch (Exception ex)
            {
                return new DBContextResult(null, false, new Error(HttpStatusCode.BadRequest,
                    string.Format(Constants.ErrorMessges.UnableToCreateTable, ex.Message)));
            }
            return new DBContextResult(table, true);
        }
    }
}
