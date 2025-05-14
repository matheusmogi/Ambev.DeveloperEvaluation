using Ambev.DeveloperEvaluation.Domain.Messages;
using MassTransit;
using MediatR;

using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCancelled;

/// <summary>
/// Handler for processing SaleCancelledNotification
/// </summary>
public class SaleCancelledNotificationHandler : INotificationHandler<SaleCancelledNotification>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<SaleCancelledNotificationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of SaleCancelledNotificationHandler
    /// </summary>
    /// <param name="publishEndpoint">The publisher</param>
    /// <param name="logger">The logger</param>
    /// <exception cref="ArgumentNullException"></exception>
    public SaleCancelledNotificationHandler(IPublishEndpoint publishEndpoint,
        ILogger<SaleCancelledNotificationHandler> logger)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the SaleCancelledNotification
    /// </summary>
    /// <param name="notification">The SaleCancelled notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(SaleCancelledNotification notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification);
        _logger.LogInformation("Sale with ID cancelled: {SaleId}", notification.SaleId);

        var @event = new CancelSaleMessage(notification.SaleId, DateTime.UtcNow);
        return _publishEndpoint.Publish(@event, cancellationToken);
    }
}