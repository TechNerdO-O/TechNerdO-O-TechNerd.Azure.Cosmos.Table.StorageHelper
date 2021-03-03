using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
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

        public async Task<ReadActionResult<TKey, TEntity>> ReadByIdAsync(string partitionKey, TKey id)
        {
            TableResult tableResult;
            try
            {
                DBContextResult dBContextResult = await _dbContext.GetTableAsync(_tableName);
                if (!dBContextResult.IsSuccess)
                    return new ReadActionResult<TKey, TEntity>(null, false, dBContextResult.Error);
                CloudTable cloudTable = dBContextResult.Table;
                var tableOperation =
                    TableOperation.Retrieve<TEntity>(partitionKey, id.ToString());
                tableResult =
                   await cloudTable.ExecuteAsync(tableOperation);
                if (tableResult.HttpStatusCode == (int)HttpStatusCode.NotFound)
                {
                    return new ReadActionResult<TKey, TEntity>(null, false, new Error(HttpStatusCode.NotFound,
                        Constants.ErrorMessges.EntityIdNotFound));
                }
                tableResult.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                return new ReadActionResult<TKey, TEntity>(null, false, new Error(HttpStatusCode.BadRequest,
                    string.Format(Constants.ErrorMessges.TableOperationFailure, ex.Message)));
            }
            return new ReadActionResult<TKey, TEntity>((TEntity)tableResult.Result, true);
        }

        public async Task<ReadByQueryActionResult<TKey, TEntity>> ReadByQueryAsync(string query = null)
        {
            var entityList = new List<TEntity>();
            try
            {
                DBContextResult dBContextResult = await _dbContext.GetTableAsync(_tableName);
                if (!dBContextResult.IsSuccess)
                    return new ReadByQueryActionResult<TKey, TEntity>(null, false, dBContextResult.Error);
                CloudTable cloudTable = dBContextResult.Table;
                var tableQuery = new TableQuery<TEntity>();
                if (!string.IsNullOrWhiteSpace(query))
                {
                    tableQuery = new TableQuery<TEntity>().Where(query);
                }
                var continuationToken = default(TableContinuationToken);
                do
                {
                    var tableQuerySegement = await cloudTable.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);
                    continuationToken = tableQuerySegement.ContinuationToken;
                    entityList.AddRange(tableQuerySegement.Results);
                }
                while (continuationToken != null);
            }
            catch (Exception ex)
            {
                return new ReadByQueryActionResult<TKey, TEntity>(null, false, new Error(HttpStatusCode.BadRequest,
                       string.Format(Constants.ErrorMessges.TableOperationFailure, ex.Message)));
            }
            return new ReadByQueryActionResult<TKey, TEntity>(entityList, true);
        }
        public async Task<StorageActionResult> DeleteByIdAsync(string partitionKey, TKey id)
        {
            StorageActionResult storageActionResult;
            try
            {
                var readActionResult = await this.ReadByIdAsync(partitionKey, id);
                if (!readActionResult.IsSuccess)
                    return new StorageActionResult(false, readActionResult.Error);
                DBContextResult dBContextResult = await _dbContext.GetTableAsync(_tableName);
                if (!dBContextResult.IsSuccess)
                    return new StorageActionResult(false, dBContextResult.Error);
                CloudTable cloudTable = dBContextResult.Table;
                var tableOperation = TableOperation.Delete(readActionResult.Entity);
                var tableResult = await cloudTable.ExecuteAsync(tableOperation);
                storageActionResult = tableResult.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                return new StorageActionResult(false, new Error(HttpStatusCode.BadRequest, string.Format(
                    Constants.ErrorMessges.TableOperationFailure, ex.Message)));
            }
            return storageActionResult;
        }


        public async Task<StorageActionResult> UpdateAsync(TEntity entity)
        {
            StorageActionResult storageActionResult;
            try
            {
                DBContextResult dBContextResult = await _dbContext.GetTableAsync(_tableName);
                if (!dBContextResult.IsSuccess)
                    return new StorageActionResult(false, dBContextResult.Error);
                CloudTable cloudTable = dBContextResult.Table;
                var tableOperation = TableOperation.InsertOrReplace(entity);
                var tableResult = await cloudTable.ExecuteAsync(tableOperation);
                storageActionResult = tableResult.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                return new StorageActionResult(false, new Error(HttpStatusCode.BadRequest, string.Format(
                        Constants.ErrorMessges.TableOperationFailure, ex.Message)));
            }
            return storageActionResult;
        }


    }
}
