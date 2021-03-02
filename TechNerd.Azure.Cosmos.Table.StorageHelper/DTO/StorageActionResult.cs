namespace TechNerd.Azure.Cosmos.Table.StorageHelper.DTO
{
    public class StorageActionResult : BaseResponse
    {
        public StorageActionResult(bool isSuccess, Error error = null)
            : base(isSuccess, error)
        {
        }
    }
}
