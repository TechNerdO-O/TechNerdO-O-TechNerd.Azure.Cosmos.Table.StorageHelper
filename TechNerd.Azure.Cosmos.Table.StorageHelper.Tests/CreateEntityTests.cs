using Microsoft.Azure.Cosmos.Table;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Core;
using TechNerd.Azure.Cosmos.Table.StorageHelper.DTO;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Helpers;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Interfaces;
using Xunit;
using static TechNerd.Azure.Cosmos.Table.StorageHelper.Tests.TestHelper;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Tests
{
    public class CreateEntityTests
    {

        private readonly TestHelper _testHelper = new TestHelper();
        [Fact]
        public async Task Should_Fail_When_TableName_HasUnSupported_Character()
        {
            //Arrange
            var mockErrorFromAzure = "mockErrorFromAzure";
            NullableEntity testEntity = new NullableEntity() { Id = "@#" };
            Mock<CloudTable> mockAzureTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable")
                      , null);
            Mock<IStorageDBContext> mockDBContext = new Mock<IStorageDBContext>();
            mockDBContext.Setup(op => op.GetTableAsync("users"))
                .ReturnsAsync(new DBContextResult(null, false, new Error(HttpStatusCode.BadRequest,
                    string.Format(Constants.ErrorMessges.UnableToCreateTable, mockErrorFromAzure))));
            ITableStorage<string, NullableEntity> testTable = new TableStorage<string, NullableEntity>("users", mockDBContext.Object);
            //Act
            var result = await testTable.CreateAsync(testEntity);
            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.ErrorCode);
            Assert.Equal(string.Format(Constants.ErrorMessges.UnableToCreateTable, mockErrorFromAzure), result.Error.Message);
        }
        [Fact]
        public async Task Should_Fail_When_Id_HasUnSupported_Character()
        {
            //Arrange
            var mockErrorFromAzure = "mockErrorFromAzure";
            NullableEntity testEntity = new NullableEntity() { Id = "@#" };
            Mock<CloudTable> mockAzureTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable")
                      , null);
            mockAzureTable.Setup(op =>
            op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.Insert)))
                .ThrowsAsync(new Exception(mockErrorFromAzure));
            //.ReturnsAsync(new TableResult() { HttpStatusCode = (int)HttpStatusCode.BadRequest });

            Mock<IStorageDBContext> mockDBContext = new Mock<IStorageDBContext>();
            mockDBContext.Setup(op => op.GetTableAsync("users"))
                .Returns(Task.FromResult(new DBContextResult(mockAzureTable.Object, true)));
            ITableStorage<string, NullableEntity> testTable = new TableStorage<string, NullableEntity>("users", mockDBContext.Object);
            //Act
            var result = await testTable.CreateAsync(testEntity);
            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.ErrorCode);
            Assert.Equal(string.Format(Constants.ErrorMessges.TableOperationFailure, mockErrorFromAzure), result.Error.Message);
        }
        [Fact]
        public async Task Should_Fail_When_Id_IsNull_ForEntity()
        {
            NullableEntity testEntity = new NullableEntity() { Id = null };
            Mock<IStorageDBContext> mockDBContext = new Mock<IStorageDBContext>();
            ITableStorage<string, NullableEntity> testTable = new TableStorage<string, NullableEntity>("users", mockDBContext.Object);
            //Act
            var result = await testTable.CreateAsync(testEntity);
            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.ErrorCode);
            Assert.Equal(Constants.ErrorMessges.NullReferenceForId, result.Error.Message);
        }
        [Fact]
        public async void Should_Create_Entity_In_Table()
        {
            //Arrange
            TestEntity passedEntity = null;
            TestEntity testEntity = _testHelper.GetEntity();
            Mock<CloudTable> mockAzureTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable")
                      , null);
            mockAzureTable.Setup(op =>
            op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.Insert)))
                .Callback<TableOperation>(e => { passedEntity = (TestEntity)e.Entity; })
                .ReturnsAsync(new TableResult() { HttpStatusCode = (int)HttpStatusCode.OK });

            Mock<IStorageDBContext> mockDBContext = new Mock<IStorageDBContext>();
            mockDBContext.Setup(op => op.GetTableAsync("users"))
                .Returns(Task.FromResult(new DBContextResult(mockAzureTable.Object, true)));
            ITableStorage<Guid, TestEntity> testTable = new TableStorage<Guid, TestEntity>("users", mockDBContext.Object);
            //Act
            var result = await testTable.CreateAsync(testEntity);
            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(passedEntity.Id, testEntity.Id);
            Assert.Equal(passedEntity.CreatedOn, testEntity.CreatedOn);
            Assert.Equal(passedEntity.Name, testEntity.Name);
            Assert.Equal(passedEntity.Email, testEntity.Email);
            Assert.Equal(passedEntity.Id.ToString(), testEntity.RowKey);
            Assert.Equal(passedEntity.Id.ToString(), testEntity.PartitionKey);
        }
        [Fact]
        public async Task Should_NotReplace_RowAndPartitionKey_WithID_When_RowAndPartitionKey_AreNotNull()
        {
            //Arrange
            TestEntity passedEntity = null;
            TestEntity testEntity = _testHelper.GetEntity();
            testEntity.PartitionKey = "SuppliedValue";
            testEntity.RowKey = "SuppliedValue";
            Mock<CloudTable> mockAzureTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable")
                      , null);
            mockAzureTable.Setup(op =>
                op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.Insert)))
                    .Callback<TableOperation>(e =>
                    {
                        passedEntity = (TestEntity)e.Entity;
                    })
                    .ReturnsAsync(new TableResult() { HttpStatusCode = (int)HttpStatusCode.OK });

            Mock<IStorageDBContext> mockDBContext = new Mock<IStorageDBContext>();
            mockDBContext.Setup(op => op.GetTableAsync("users"))
                        .Returns(Task.FromResult(new DBContextResult(mockAzureTable.Object, true)));
            ITableStorage<Guid, TestEntity> testTable = new TableStorage<Guid, TestEntity>("users", mockDBContext.Object);
            //Act
            var result = await testTable.CreateAsync(testEntity);
            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(passedEntity.Id, testEntity.Id);
            Assert.Equal(passedEntity.CreatedOn, testEntity.CreatedOn);
            Assert.Equal(passedEntity.Name, testEntity.Name);
            Assert.Equal(passedEntity.Email, testEntity.Email);
            Assert.Equal("SuppliedValue", testEntity.RowKey);
            Assert.Equal("SuppliedValue", testEntity.PartitionKey);
        }
    }
}
