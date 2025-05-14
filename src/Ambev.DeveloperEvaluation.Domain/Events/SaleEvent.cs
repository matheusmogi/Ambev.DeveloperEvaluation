using MongoDB.Bson;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Represents an event related to a sale to be stored in the noSQL database.
/// </summary>
public class SaleEvent
{
    public ObjectId Id { get; set; }
    public string Type { get; set; } = nameof(SaleEventType.SaleCreated);
    public SaleEventData? Data { get; set; }
    public int Version { get; set; }
    public DateTime Date { get; set; }
}

public enum SaleEventType
{
    SaleCreated = 1,
    SaleUpdated = 2,
    SaleDeleted = 3
}
public class SaleEventData
{
    public required string SaleId { get; set; }
    public int CustomerId { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int ItemCount { get; set; }
}