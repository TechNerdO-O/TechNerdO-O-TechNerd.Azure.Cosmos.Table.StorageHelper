namespace TechNerd.Azure.Cosmos.Table.StorageHelper.DTO
{
    public abstract class BaseResponse
    {
        public bool IsSuccess { get; }
        public Error Error { get; set; }
        public BaseResponse(bool isSuccess, Error error = null)
        {
            IsSuccess = isSuccess;
            Error = error;
        }
    }
}
