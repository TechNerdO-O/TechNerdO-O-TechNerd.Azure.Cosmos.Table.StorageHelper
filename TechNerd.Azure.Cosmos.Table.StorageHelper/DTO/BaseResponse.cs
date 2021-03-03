using System;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.DTO
{
    public abstract class BaseResponse
    {
        public bool IsSuccess { get; }
        public Error Error { get; set; }
        public BaseResponse(bool isSuccess, Error error = null)
        {
            IsSuccess = isSuccess;
            if (!isSuccess && error == null)
                throw new ArgumentNullException("Error detail can't be empty for failure response.");
            Error = error;
        }
    }
}
