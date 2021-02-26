using Microsoft.Azure.Cosmos.Table;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Core;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Interfaces;
using Xunit;
using static TechNerd.Azure.Cosmos.Table.StorageHelper.Tests.TestHelper;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Tests
{
    public class CreateEntityTests
    {
        private readonly TestHelper _testHelper = new TestHelper();
        [Fact]
        public async void Should_Fail_When_Entities_AreInvalid() { }
        [Fact]
        public async void Should_Create_Entity_In_Table()
        {
            //Arrange
            TestEntity passedEntity = null;
            Mock<CloudTable> mockAzureTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable")
                   , null);
            mockAzureTable.Setup(op =>
            op.ExecuteAsync(It.Is<TableOperation>(x => x.OperationType == TableOperationType.Insert)))
                .Callback<TableOperation>(e => { passedEntity = (TestEntity)e.Entity; })
                .ReturnsAsync(new TableResult() { HttpStatusCode = (int)HttpStatusCode.OK });

            Mock<IStorageDBContext> mockDBContext = new Mock<IStorageDBContext>();
            mockDBContext.Setup(op => op.GetTableAsync("users"))
                .Returns(Task.FromResult(mockAzureTable.Object));
            TestEntity testEntity = _testHelper.GetEntity();
            ITableStorage<Guid, TestEntity> testTable = new TableStorage<Guid, TestEntity>("users", mockDBContext.Object);
            //Act
            var result = await testTable.CreateAsync(testEntity);
            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(passedEntity.Id, testEntity.Id);
            Assert.Equal(passedEntity.CreatedOn, testEntity.CreatedOn);
            Assert.Equal(passedEntity.Name, testEntity.Name);
            Assert.Equal(passedEntity.Email, testEntity.Email);
        }
    }
}
