using Microsoft.Azure.Cosmos.Table;
using System.Net;
using System.Net.Http;
using TechNerd.Azure.Cosmos.Table.StorageHelper.DTO;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Helpers
{
    internal static class TableResultExtensions
    {
        internal static StorageActionResult EnsureSuccessStatusCode(
            this TableResult tableResult)
        {
            StorageActionResult result;
            switch (tableResult.HttpStatusCode)
            {
                case (int)HttpStatusCode.Created:
                case (int)HttpStatusCode.OK:
                case (int)HttpStatusCode.NoContent:
                    result = new StorageActionResult(isSuccess: true);
                    break;
                default:
                    result = new StorageActionResult(isSuccess: false, error:
                        new Error((HttpStatusCode)tableResult.HttpStatusCode,
                        "Unable to complete the requested operation."));
                    throw new HttpRequestException(
                        $"Something went wrong in table operation, a {tableResult.HttpStatusCode} status code was returned.");
            }
            return result;
        }
    }
}
