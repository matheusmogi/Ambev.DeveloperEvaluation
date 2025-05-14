using Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCancelled;
using Ambev.DeveloperEvaluation.Domain.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Notification;

public class SaleCancelledNotificationHandlerTests
{
    private readonly SaleCancelledNotificationHandler _handler;
    private readonly IPublishEndpoint _publishEndpoint;

    public SaleCancelledNotificationHandlerTests()
    {
        var logger = new LoggerFactory().CreateLogger<SaleCancelledNotificationHandler>();

        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        _handler = new SaleCancelledNotificationHandler(_publishEndpoint, logger);
    }

    [Fact]
    public async Task Handle_ValidNotification_ShouldPublishNotification()
    {
        // Arrange
        var notification = new SaleCancelledNotification
        {
            SaleId = Guid.NewGuid(),
        };
        var @event = new CancelSaleMessage(notification.SaleId, DateTime.UtcNow);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        await _publishEndpoint.Received(1)
            .Publish(Arg.Is<CancelSaleMessage>(c =>
                    c.SaleId == @event.SaleId &&
                    c.Created.Date == @event.Created.Date),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NullNotification_ShouldThrowArgumentNullException()
    {
        // Arrange
        SaleCancelledNotification? notification = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(notification, CancellationToken.None));
    }
}