using System.Collections.Generic;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Interfaces;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.DTO
{
    public class ReadByQueryActionResult<TKey, TEntity> : BaseResponse where TEntity : IEntity<TKey>
    {
        public IEnumerable<TEntity> Entity { get; }
        public ReadByQueryActionResult(IEnumerable<TEntity> entity, bool isSuccess, Error error = null)
            : base(isSuccess, error)
        {
            Entity = entity;
        }
    }
}
