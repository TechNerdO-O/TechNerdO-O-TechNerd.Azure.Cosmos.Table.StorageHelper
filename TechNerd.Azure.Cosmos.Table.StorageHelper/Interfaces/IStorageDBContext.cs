using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Interfaces
{
    public interface IStorageDBContext
    {
        Task<CloudTable> GetTableAsync(string tableName);
    }
}
