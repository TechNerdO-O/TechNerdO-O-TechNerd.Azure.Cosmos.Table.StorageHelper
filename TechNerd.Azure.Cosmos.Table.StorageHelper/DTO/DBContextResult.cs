using Microsoft.Azure.Cosmos.Table;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.DTO
{
    public class DBContextResult : BaseResponse
    {
        public CloudTable Table { get; }
        public DBContextResult(CloudTable table, bool isSuccess, Error error = null)
            : base(isSuccess, error)
        {
            Table = table;
        }
    }
}
