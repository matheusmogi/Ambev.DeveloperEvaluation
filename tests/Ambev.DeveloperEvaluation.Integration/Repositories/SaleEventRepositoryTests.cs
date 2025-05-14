using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.NoSqlStorage;
using Microsoft.Extensions.Logging;
using Mongo2Go;
using MongoDB.Driver;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Repositories;

public class SaleEventRepositoryTests
{
    [Theory]
    [InlineData(SaleEventType.SaleCreated)]
    [InlineData(SaleEventType.SaleUpdated)]
    [InlineData(SaleEventType.SaleDeleted)]
    public async Task CreateAsync_ShouldInsertSaleEvent(SaleEventType eventType)
    {
        // Arrange
        var logger = new LoggerFactory().CreateLogger<SaleEventRepository>();
     
        var runner = MongoDbRunner.Start();
        var client = new MongoClient(runner.ConnectionString);
        var database = client.GetDatabase("TestDatabase");
        var repository = new SaleEventRepository(database, logger);

        var eventData = new SaleEventData
        {
            SaleId = "12345",
            CustomerId = 100,
            ItemCount = 20,
            SaleDate = DateTime.UtcNow,
            TotalAmount = 200,
        };

        // Act
        await repository.CreateAsync(eventData, eventType);

        // Assert
        var collection = database.GetCollection<SaleEvent>("sale_events");
        var count = await collection.CountDocumentsAsync(FilterDefinition<SaleEvent>.Empty);
        Assert.Equal(1, count);
        runner.Dispose();
    } 
}