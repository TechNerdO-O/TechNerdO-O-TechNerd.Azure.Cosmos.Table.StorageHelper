namespace TechNerd.Azure.Cosmos.Table.StorageHelper.DTO
{
    public class ReadActionResult : BaseResponse
    {
        public int MyProperty { get; set; }
        public ReadActionResult(bool isSuccess, Error error = null)
            : base(isSuccess, error)
        {
        }
    }
}
