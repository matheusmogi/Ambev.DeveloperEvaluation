using Ambev.DeveloperEvaluation.Domain.Messages;
using MassTransit;
using MediatR;

using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleModified;

/// <summary>
/// Handler for processing SaleModifiedNotification
/// </summary>
public class SaleUpdatedNotificationHandler : INotificationHandler<SaleUpdatedNotification>
{
    private readonly ILogger<SaleUpdatedNotificationHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    /// <summary>
    /// Initializes a new instance of SaleModifiedNotificationHandler
    /// </summary>
    /// <param name="publishEndpoint">The publisher</param>
    /// <param name="logger">The logger instance</param>
    public SaleUpdatedNotificationHandler(IPublishEndpoint publishEndpoint,
        ILogger<SaleUpdatedNotificationHandler> logger)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the SaleModifiedNotification
    /// </summary>
    /// <param name="notification">The SaleModified notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(SaleUpdatedNotification notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification);
        _logger.LogInformation("Sale with ID modified: {SaleId}", notification.SaleId);

        var @event = new UpdateSaleMessage(notification.SaleId, DateTime.UtcNow);
        return _publishEndpoint.Publish(@event, cancellationToken);

    }
}