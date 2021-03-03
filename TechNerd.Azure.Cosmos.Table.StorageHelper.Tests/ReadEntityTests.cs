using Microsoft.Azure.Cosmos.Table;
using Moq;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Core;
using TechNerd.Azure.Cosmos.Table.StorageHelper.DTO;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Helpers;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Interfaces;
using Xunit;
using static TechNerd.Azure.Cosmos.Table.StorageHelper.Tests.TestHelper;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Tests
{
    public class ReadEntityTests
    {
        private readonly TestHelper _testHelper = new TestHelper();
        [Fact]
        public async Task Should_Use_InputPartionKeyandRowKey_AsSupplied()
        {
            //Arrange
            string passedRowKey = "";
            string passedPartitionKey = "";
            Mock<CloudTable> mockAzureTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable")
                      , null);
            mockAzureTable.Setup(op =>
                op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.Retrieve)))
                    .Callback<TableOperation>(e =>
                    {
                        passedPartitionKey = _testHelper.GetInternalMember<string>(e, "RetrievePartitionKey");
                        passedRowKey = _testHelper.GetInternalMember<string>(e, "RetrieveRowKey");
                    })
                    .ReturnsAsync(new TableResult() { HttpStatusCode = (int)HttpStatusCode.OK, Result = new NullableEntity() });
            Mock<IStorageDBContext> mockDBContext = new Mock<IStorageDBContext>();
            mockDBContext.Setup(op => op.GetTableAsync("users"))
                        .Returns(Task.FromResult(new DBContextResult(mockAzureTable.Object, true)));
            ITableStorage<string, NullableEntity> testTable = new TableStorage<string, NullableEntity>("users", mockDBContext.Object);
            //Act
            var result = await testTable.ReadByIdAsync("inputPartitionKey", "inputKey");
            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("inputPartitionKey", passedPartitionKey);
            Assert.Equal("inputKey", passedRowKey);
        }
        [Fact]
        public async Task Should_Throw_Exception_When_ParitionKey_IsNull()
        {
            //Arrange
            Mock<CloudTable> mockAzureTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable")
                      , null);
            Mock<IStorageDBContext> mockDBContext = new Mock<IStorageDBContext>();
            mockDBContext.Setup(op => op.GetTableAsync("users"))
                        .Returns(Task.FromResult(new DBContextResult(mockAzureTable.Object, true)));
            ITableStorage<string, NullableEntity> testTable = new TableStorage<string, NullableEntity>("users", mockDBContext.Object);
            //Act
            var result = await testTable.ReadByIdAsync(null, "inputKey");
            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(string.Format(Constants.ErrorMessges.TableOperationFailure, "Value cannot be null. (Parameter 'partitionKey')"), result.Error.Message);
        }
        [Fact]
        public async Task Should_Fail_When_RowKey_IsnotFound()
        {
            //Arrange
            string mockExceptionMsg = "mockExceptionMsg";
            string existingKey = "mockExisting";
            Mock<CloudTable> mockAzureTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable")
                      , null);
            mockAzureTable.Setup(op =>
                op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.Retrieve
                && (_testHelper.GetInternalMember<string>(x, "RetrievePartitionKey") != existingKey
                || _testHelper.GetInternalMember<string>(x, "RetrieveRowKey") != existingKey))))
                    .ThrowsAsync(new Exception(mockExceptionMsg));
            Mock<IStorageDBContext> mockDBContext = new Mock<IStorageDBContext>();
            mockDBContext.Setup(op => op.GetTableAsync("users"))
                        .Returns(Task.FromResult(new DBContextResult(mockAzureTable.Object, true)));
            ITableStorage<string, NullableEntity> testTable = new TableStorage<string, NullableEntity>("users", mockDBContext.Object);
            //Act
            var result = await testTable.ReadByIdAsync("nonExistingKey", "nonExistingKey");
            //Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Entity);
            Assert.Equal(string.Format(Constants.ErrorMessges.TableOperationFailure, mockExceptionMsg), result.Error.Message);
        }
        [Fact]
        public async Task Should_Return_MatchingEntity_When_RowKey_IsFound()
        {
            //Arrange
            string existingKey = "mockExisting";
            Mock<CloudTable> mockAzureTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable")
                      , null);
            mockAzureTable.Setup(op =>
                op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.Retrieve
                && _testHelper.GetInternalMember<string>(x, "RetrievePartitionKey") == existingKey
                && _testHelper.GetInternalMember<string>(x, "RetrieveRowKey") == existingKey)))
                    .ReturnsAsync(new TableResult() { HttpStatusCode = (int)HttpStatusCode.OK, Result = new NullableEntity() { Id = existingKey } });
            Mock<IStorageDBContext> mockDBContext = new Mock<IStorageDBContext>();
            mockDBContext.Setup(op => op.GetTableAsync("users"))
                        .Returns(Task.FromResult(new DBContextResult(mockAzureTable.Object, true)));
            ITableStorage<string, NullableEntity> testTable = new TableStorage<string, NullableEntity>("users", mockDBContext.Object);
            //Act
            var result = await testTable.ReadByIdAsync("mockExisting", "mockExisting");
            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("mockExisting", result.Entity.Id);
        }
    }
}
