using System.Collections.Generic;
using System.Threading.Tasks;
using TechNerd.Azure.Cosmos.Table.StorageHelper.DTO;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Interfaces
{
    public interface ITableStorage<TKey, TEntity> where TEntity : IEntity<TKey>
    {
        Task<StorageActionResult> CreateAsync(TEntity entity);
        Task<ReadActionResult<TKey, TEntity>> ReadByIdAsync(string partitionKey, TKey id);
        Task<ReadByQueryActionResult<TKey, TEntity>> ReadByQueryAsync(string query = null);
        Task<StorageActionResult> UpdateAsync(TEntity entity);
        Task<StorageActionResult> DeleteByIdAsync(string partitionKey, TKey id);
    }
}
