using Ambev.DeveloperEvaluation.Domain.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCreated;

/// <summary>
/// Handler for processing SaleCreatedNotification
/// </summary>
public class SaleCreatedNotificationHandler : INotificationHandler<SaleCreatedNotification>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<SaleCreatedNotificationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of SaleCreatedNotificationHandler
    /// </summary>
    /// <param name="publishEndpoint">The publisher</param>
    /// <param name="logger">The logger</param>
    /// <exception cref="ArgumentNullException"></exception>
    public SaleCreatedNotificationHandler(IPublishEndpoint publishEndpoint,
        ILogger<SaleCreatedNotificationHandler> logger)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the SaleCreatedNotification
    /// </summary>
    /// <param name="notification">The SaleCreated notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(SaleCreatedNotification notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification);
        
        _logger.LogInformation("Publishing SaleCreatedNotification for Sale ID: {SaleId}", notification.SaleId);
        var @event = new CreateSaleMessage(notification.SaleId, DateTime.UtcNow);
        return _publishEndpoint.Publish(@event, cancellationToken);
    }
}