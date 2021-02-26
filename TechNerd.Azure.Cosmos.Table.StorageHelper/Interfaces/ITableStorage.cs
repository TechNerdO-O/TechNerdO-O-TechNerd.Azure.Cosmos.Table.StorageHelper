using System.Threading.Tasks;
using TechNerd.Azure.Cosmos.Table.StorageHelper.DTO;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Interfaces
{
    public interface ITableStorage<TKey, TEntity> where TEntity : IEntity<TKey>
    {
        Task<StorageActionResult> CreateAsync(TEntity entity);
        Task<TEntity> ReadByIdAsync(TKey id);
        Task<StorageActionResult> Update(TEntity entity);
        Task<StorageActionResult> DeleteByIdAsync(TKey id);
    }
}
