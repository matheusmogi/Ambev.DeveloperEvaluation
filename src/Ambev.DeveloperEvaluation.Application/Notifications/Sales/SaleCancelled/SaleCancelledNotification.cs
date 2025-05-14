using MediatR;
using MongoDB.Bson;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCancelled;

/// <summary>
/// Notification that represents the cancellation of a sale.
/// </summary>
public class SaleCancelledNotification : INotification
{
    /// <summary>
    /// The unique identifier of the cancelled sale
    /// </summary>
    public Guid SaleId { get; set; }
}