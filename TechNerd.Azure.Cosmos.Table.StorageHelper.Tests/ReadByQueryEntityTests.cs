using System;
using System.Reflection;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Tests
{
    public class ReadByQueryEntityTests
    {
        private T GetInternalMember<T>(object obj, string propertyName)
        {
            Type objType = obj.GetType();
            PropertyInfo propInfo = objType.GetProperty(propertyName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            return (T)propInfo.GetValue(obj, null);
        }
        //[Fact]
        //public async Task Should_Fail_When_RowKey_IsnotFound()
        //{
        //    //Arrange
        //    string mockExceptionMsg = "mockExceptionMsg";
        //    string existingKey = "mockExisting";
        //    Mock<CloudTable> mockAzureTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable")
        //              , null);
        //    mockAzureTable.Setup(op =>
        //        op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.Retrieve
        //        && (GetInternalMember<string>(x, "RetrievePartitionKey") != existingKey
        //        || GetInternalMember<string>(x, "RetrieveRowKey") != existingKey))))
        //            .ThrowsAsync(new Exception(mockExceptionMsg));
        //    Mock<IStorageDBContext> mockDBContext = new Mock<IStorageDBContext>();
        //    mockDBContext.Setup(op => op.GetTableAsync("users"))
        //                .Returns(Task.FromResult(new DBContextResult(mockAzureTable.Object, true)));
        //    ITableStorage<string, NullableEntity> testTable = new TableStorage<string, NullableEntity>("users", mockDBContext.Object);
        //    //Act
        //    var result = await testTable.ReadByQueryAsync("nonExistingKey", "nonExistingKey");
        //    //Assert
        //    Assert.False(result.IsSuccess);
        //    Assert.Null(result.Entity);
        //    Assert.Equal(string.Format(Constants.ErrorMessges.TableOperationFailure, mockExceptionMsg), result.Error.Message);
        //}
        //[Fact]
        //public async Task Should_Return_MatchingEntity_When_Query_IsValid()
        //{
        //    //Arrange
        //    string existingKey = "mockExisting";
        //    Mock<CloudTable> mockAzureTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable")
        //              , null);
        //    mockAzureTable.Setup(op =>
        //        op.ExecuteQuerySegmentedAsync(It.IsAny<TableQuery>(), It.IsAny<TableContinuationToken>()));
        //    Mock<IStorageDBContext> mockDBContext = new Mock<IStorageDBContext>();
        //    mockDBContext.Setup(op => op.GetTableAsync("users"))
        //                .Returns(Task.FromResult(new DBContextResult(mockAzureTable.Object, true)));
        //    ITableStorage<string, NullableEntity> testTable = new TableStorage<string, NullableEntity>("users", mockDBContext.Object);
        //    //Act
        //    var result = await testTable.ReadByQueryAsync("mockExisting", "mockExisting");
        //    //Assert
        //    Assert.True(result.IsSuccess);
        //    Assert.Equal("mockExisting", result.Entity.Id);
        //}
    }
}
