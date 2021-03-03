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
    public class UpdateEntityTests
    {
        private readonly TestHelper _testHelper = new TestHelper();
        [Fact]
        public async Task Should_Update_Entity_In_Table_When_Found()
        {
            //Arrange
            Guid initialGuid = Guid.NewGuid();
            Guid inputGuid = Guid.NewGuid();
            TestEntity existingEntity = new TestEntity()
            {
                Id = initialGuid,
                RowKey = initialGuid.ToString(),
                PartitionKey = initialGuid.ToString()
            };
            TestEntity inputEntity = new TestEntity()
            {
                Id = inputGuid,
                RowKey = inputGuid.ToString(),
                PartitionKey = inputGuid.ToString()
            };
            Mock<CloudTable> mockAzureTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable")
                      , null);
            //mockAzureTable.Setup(op => op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.Retrieve)))
            //    .ReturnsAsync(new TableResult() { HttpStatusCode = (int)HttpStatusCode.OK, Result = existingEntity });
            mockAzureTable.Setup(op =>
            op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.InsertOrReplace)))
            .Callback<TableOperation>(e => { existingEntity = (TestEntity)e.Entity; })
            .ReturnsAsync(new TableResult() { HttpStatusCode = (int)HttpStatusCode.OK });
            Mock<IStorageDBContext> mockDBContext = new Mock<IStorageDBContext>();
            mockDBContext.Setup(op => op.GetTableAsync("users"))
                        .Returns(Task.FromResult(new DBContextResult(mockAzureTable.Object, true)));
            ITableStorage<Guid, TestEntity> testTable = new TableStorage<Guid, TestEntity>("users", mockDBContext.Object);
            //Act
            var result = await testTable.UpdateAsync(inputEntity);
            //Assert
            mockAzureTable.Verify(op => op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.InsertOrReplace
         && x.Entity == inputEntity)), Times.Once);
            Assert.True(result.IsSuccess);
            Assert.Equal(inputGuid, existingEntity.Id);
            Assert.Equal(inputGuid.ToString(), existingEntity.RowKey);
            Assert.Equal(inputGuid.ToString(), existingEntity.PartitionKey);
        }
    }
}
