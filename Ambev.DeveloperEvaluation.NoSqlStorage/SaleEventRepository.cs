using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.NoSqlStorage;

/// <summary>
/// Repository for handling sale events in MongoDB
/// </summary>
public class SaleEventRepository : ISaleEventRepository
{
    private readonly ILogger<SaleEventRepository> _logger;
    private readonly IMongoCollection<SaleEvent> _collection;

    public SaleEventRepository(IMongoDatabase database, ILogger<SaleEventRepository> logger)
    {
        // Ensure the database is not null
        if (database == null)
        {
            logger.LogError("Database is null");
            throw new ArgumentNullException(nameof(database));
        }
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _collection = database.GetCollection<SaleEvent>("sale_events");
    }

    /// <summary>
    /// Saves a SaleCreatedEvent to the MongoDB collection
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="saleEventType"></param>
    /// <param name="cancellationToken"></param>
    public async Task CreateAsync(SaleEventData eventData, SaleEventType saleEventType, CancellationToken cancellationToken = default)
    {
        var saleEvent = new SaleEvent
        {
            Type = saleEventType.ToString(),
            Data = eventData,
            Version = 1,
            Date = DateTime.UtcNow,
        };
        
        await _collection.InsertOneAsync(saleEvent, null, cancellationToken);
        _logger.LogInformation("Sale event of type {SaleEventType} created with ID {SaleEventId}", saleEventType, saleEvent.Id);
    }
 
}