using System.Net;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.DTO
{
    public class Error
    {
        public HttpStatusCode ErrorCode { get; }
        public string Message { get; }
        public Error(HttpStatusCode errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}
