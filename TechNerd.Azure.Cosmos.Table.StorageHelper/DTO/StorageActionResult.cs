namespace TechNerd.Azure.Cosmos.Table.StorageHelper.DTO
{
    public class StorageActionResult
    {
        public bool IsSuccess { get; }
        public Error Error { get; set; }
        public StorageActionResult(bool isSuccess, Error error = null)
        {
            IsSuccess = isSuccess;
            Error = error;
        }
    }
}
