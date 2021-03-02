using System.Threading.Tasks;
using TechNerd.Azure.Cosmos.Table.StorageHelper.DTO;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Interfaces
{
    public interface IStorageDBContext
    {
        Task<DBContextResult> GetTableAsync(string tableName);
    }
}
