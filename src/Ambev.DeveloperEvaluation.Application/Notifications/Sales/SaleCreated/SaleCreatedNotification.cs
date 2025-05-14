using MediatR;
using MongoDB.Bson;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCreated;

/// <summary>
/// Notification for when a sale is created
/// </summary>
public class SaleCreatedNotification : INotification
{
    /// <summary>
    /// The unique identifier of the created sale
    /// </summary>
    public Guid SaleId { get; set; }
}