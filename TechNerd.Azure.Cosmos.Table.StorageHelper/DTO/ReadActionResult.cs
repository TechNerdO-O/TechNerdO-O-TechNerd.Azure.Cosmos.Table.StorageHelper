using TechNerd.Azure.Cosmos.Table.StorageHelper.Interfaces;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.DTO
{
    public class ReadActionResult<TKey, TEntity> : BaseResponse where TEntity : IEntity<TKey>
    {
        public TEntity Entity { get; }
        public ReadActionResult(TEntity entity, bool isSuccess, Error error = null)
            : base(isSuccess, error)
        {
            Entity = entity;
        }
    }
}
