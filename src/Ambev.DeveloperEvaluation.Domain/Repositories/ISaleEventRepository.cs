using Ambev.DeveloperEvaluation.Domain.Events;
using MongoDB.Bson;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Interface for SaleEvent repository
/// </summary>
public interface ISaleEventRepository 
{
    /// <summary>
    /// Saves a SaleCreatedEvent to the repository
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="saleEventType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CreateAsync(SaleEventData eventData, SaleEventType saleEventType,
        CancellationToken cancellationToken = default);
}