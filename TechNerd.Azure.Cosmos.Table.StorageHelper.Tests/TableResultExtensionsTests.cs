using Microsoft.Azure.Cosmos.Table;
using System.Net;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Helpers;
using Xunit;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Tests
{
    public class TableResultExtensionsTests
    {
        [Fact]
        public void Should_Return_Success_For_OK_StatusCode()
        {
            TableResult mockTableResult = new TableResult()
            { HttpStatusCode = (int)HttpStatusCode.OK };
            var result = TableResultExtensions.EnsureSuccessStatusCode(mockTableResult);
            Assert.True(result.IsSuccess);
        }
        [Fact]
        public void Should_Return_Success_For_Created_StatusCode()
        {
            TableResult mockTableResult = new TableResult()
            { HttpStatusCode = (int)HttpStatusCode.Created };
            var result = TableResultExtensions.EnsureSuccessStatusCode(mockTableResult);
            Assert.True(result.IsSuccess);
        }
        [Fact]
        public void Should_Return_Success_For_NoContent_StatusCode()
        {
            TableResult mockTableResult = new TableResult()
            { HttpStatusCode = (int)HttpStatusCode.NoContent };
            var result = TableResultExtensions.EnsureSuccessStatusCode(mockTableResult);
            Assert.True(result.IsSuccess);
        }
        [Fact]
        public void Should_Return_Failure_For_Other_StatusCode()
        {
            TableResult mockTableResult = new TableResult()
            { HttpStatusCode = (int)HttpStatusCode.BadRequest };
            var result = TableResultExtensions.EnsureSuccessStatusCode(mockTableResult);
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.ErrorCode);
            Assert.Equal(Constants.ErrorMessges.TableOperationFailure, result.Error.Message);
        }
    }
}
