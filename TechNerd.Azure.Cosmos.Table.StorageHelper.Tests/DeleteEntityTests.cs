using Microsoft.Azure.Cosmos.Table;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Helpers;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Core;
using TechNerd.Azure.Cosmos.Table.StorageHelper.DTO;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Interfaces;
using Xunit;
using static TechNerd.Azure.Cosmos.Table.StorageHelper.Tests.TestHelper;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Tests
{
    public class DeleteEntityTests
    {
        private readonly TestHelper _testHelper = new TestHelper();
        [Fact]
        public async Task Should_Delete_Entity_From_Table()
        {
            //Arrange
            string existingKey = "mockExisting";
            Mock<CloudTable> mockAzureTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable")
                      , null);
            mockAzureTable.Setup(op =>
               op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.Retrieve
               && _testHelper.GetInternalMember<string>(x, "RetrievePartitionKey") == existingKey
               && _testHelper.GetInternalMember<string>(x, "RetrieveRowKey") == existingKey)))
                   .ReturnsAsync(new TableResult()
                   {
                       HttpStatusCode = (int)HttpStatusCode.OK,
                       Result = new NullableEntity()
                       {
                           RowKey = existingKey,
                           PartitionKey = existingKey,
                           ETag = "mockTag"
                       }
                   });
            mockAzureTable.Setup(op =>
                op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.Delete
                && x.Entity.RowKey == existingKey
                && x.Entity.PartitionKey == existingKey)))
                .ReturnsAsync(new TableResult() { HttpStatusCode = (int)HttpStatusCode.OK });
            Mock<IStorageDBContext> mockDBContext = new Mock<IStorageDBContext>();
            mockDBContext.Setup(op => op.GetTableAsync("users"))
                        .Returns(Task.FromResult(new DBContextResult(mockAzureTable.Object, true)));
            ITableStorage<string, NullableEntity> testTable = new TableStorage<string, NullableEntity>("users", mockDBContext.Object);
            //Act
            var result = await testTable.DeleteByIdAsync(existingKey, existingKey);
            //Assert
            mockAzureTable.Verify(op => op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.Delete
         && x.Entity.RowKey == existingKey)), Times.Once);
            Assert.True(result.IsSuccess);
        }
        [Fact]
        public async Task Should_Fail_When_Entity_IsNotFound()
        {
            //Arrange
            string nonExistingKey = "nonExistingKey";
            Mock<CloudTable> mockAzureTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable")
                      , null);
            mockAzureTable.Setup(op =>
               op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.Retrieve)))
                    .ReturnsAsync(new TableResult() { HttpStatusCode = (int)HttpStatusCode.NotFound });
            Mock<IStorageDBContext> mockDBContext = new Mock<IStorageDBContext>();
            mockDBContext.Setup(op => op.GetTableAsync("users"))
                        .Returns(Task.FromResult(new DBContextResult(mockAzureTable.Object, true)));
            ITableStorage<string, NullableEntity> testTable = new TableStorage<string, NullableEntity>("users", mockDBContext.Object);
            //Act
            var result = await testTable.DeleteByIdAsync(nonExistingKey, nonExistingKey);
            //Assert
            mockAzureTable.Verify(op => op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.Retrieve
         && (_testHelper.GetInternalMember<string>(x, "RetrieveRowKey") == nonExistingKey))), Times.Once);
            Assert.False(result.IsSuccess);
            Assert.Equal(Constants.ErrorMessges.EntityIdNotFound, result.Error.Message);
            Assert.Equal(HttpStatusCode.NotFound, result.Error.ErrorCode);
        }
    }
}
