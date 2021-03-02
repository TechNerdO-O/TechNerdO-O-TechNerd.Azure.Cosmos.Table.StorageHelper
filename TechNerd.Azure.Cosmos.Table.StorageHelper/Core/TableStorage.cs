using Microsoft.Azure.Cosmos.Table;
using System;
using System.Net;
using System.Threading.Tasks;
using TechNerd.Azure.Cosmos.Table.StorageHelper.DTO;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Helpers;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Interfaces;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Core
{
    public class TableStorage<TKey, TEntity> :
        ITableStorage<TKey, TEntity> where TEntity : Entity<TKey>, new()
    {
        private readonly IStorageDBContext _dbContext;
        private readonly string _tableName;
        public TableStorage(string tableName, IStorageDBContext dBContext)
        {
            _dbContext = dBContext;
            _tableName = tableName;
        }
        private StorageActionResult ValidateEntityModel(TEntity entity)
        {
            if (entity.Id == null)
                return new StorageActionResult(false, new Error(HttpStatusCode.BadRequest, Constants.ErrorMessges.NullReferenceForId));
            if (entity.RowKey == null)
                entity.RowKey = entity.Id.ToString();
            if (entity.PartitionKey == null)
                entity.PartitionKey = entity.Id.ToString();
            return new StorageActionResult(true);
        }
        public async Task<StorageActionResult> CreateAsync(TEntity entity)
        {
            StorageActionResult storageActionResult;
            try
            {
                var validationResult = ValidateEntityModel(entity);
                if (!validationResult.IsSuccess)
                    return validationResult;
                DBContextResult dBContextResult = await _dbContext.GetTableAsync(_tableName);
                if (!dBContextResult.IsSuccess)
                    return new StorageActionResult(false, dBContextResult.Error);
                CloudTable cloudTable = dBContextResult.Table;
                var tableOperation =
                   TableOperation.Insert(entity);
                var tableResult =
                   await cloudTable.ExecuteAsync(tableOperation);
                storageActionResult = tableResult.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                storageActionResult = new StorageActionResult(false,
                    new Error(HttpStatusCode.BadRequest,
                    string.Format(Constants.ErrorMessges.TableOperationFailure, ex.Message)));
            }
            return storageActionResult;
        }

        public async Task<StorageActionResult> DeleteByIdAsync(TKey id)
        {
            var entity =
                await this.ReadByIdAsync(id);

            if (entity == null)
                return new StorageActionResult(false,
                new Error(HttpStatusCode.NotFound, "Id not found"));
            DBContextResult dBContextResult = await _dbContext.GetTableAsync(_tableName);
            if (!dBContextResult.IsSuccess)
                return new StorageActionResult(false, dBContextResult.Error);
            CloudTable cloudTable = dBContextResult.Table;
            var tableOperation =
               TableOperation.Delete(entity);
            var tableResult =
               await cloudTable.ExecuteAsync(tableOperation);

            return tableResult.EnsureSuccessStatusCode();
        }

        public async Task<TEntity> ReadByIdAsync(TKey id)
        {
            DBContextResult dBContextResult = await _dbContext.GetTableAsync(_tableName);
            if (!dBContextResult.IsSuccess)
                return null;
            CloudTable cloudTable = dBContextResult.Table;
            var tableOperation =
                TableOperation.Retrieve<TEntity>(id.ToString(), id.ToString());

            var tableResult =
                await cloudTable.ExecuteAsync(tableOperation);

            if (tableResult.HttpStatusCode == (int)HttpStatusCode.NotFound)
            {
                return null;
            }

            tableResult.EnsureSuccessStatusCode();

            return tableResult.Result as TEntity;
        }

        public async Task<StorageActionResult> Update(TEntity entity)
        {
            DBContextResult dBContextResult = await _dbContext.GetTableAsync(_tableName);
            if (!dBContextResult.IsSuccess)
                return new StorageActionResult(false, dBContextResult.Error);
            CloudTable cloudTable = dBContextResult.Table;
            var tableOperation =
                 TableOperation.InsertOrReplace(entity);

            var tableResult =
                await cloudTable.ExecuteAsync(tableOperation);

            return tableResult.EnsureSuccessStatusCode();
        }
    }
}
