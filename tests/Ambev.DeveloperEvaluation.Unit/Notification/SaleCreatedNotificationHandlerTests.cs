using Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCreated;
using Ambev.DeveloperEvaluation.Domain.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Notification;

/// <summary>
/// Contains unit tests for the ItemCancelledNotificationHandler class.
/// </summary>
public class SaleCreatedNotificationHandlerTests
{
    private readonly SaleCreatedNotificationHandler _handler;
    private readonly IPublishEndpoint _publishEndpoint;

    public SaleCreatedNotificationHandlerTests()
    {
        var logger = new LoggerFactory().CreateLogger<SaleCreatedNotificationHandler>();

        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        _handler = new SaleCreatedNotificationHandler(_publishEndpoint, logger);
    }

    [Fact]
    public async Task Handle_ValidNotification_ShouldPublishNotification()
    {
        // Arrange
        var notification = new SaleCreatedNotification
        {
            SaleId = Guid.NewGuid(),
        };
        var @event = new CreateSaleMessage(notification.SaleId, DateTime.UtcNow);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        await _publishEndpoint.Received(1)
            .Publish(Arg.Is<CreateSaleMessage>(c => c.SaleId == @event.SaleId &&
                                                    c.Created.Date == @event.Created.Date),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NullNotification_ShouldThrowArgumentNullException()
    {
        // Arrange
        SaleCreatedNotification? notification = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(notification, CancellationToken.None));
    }
}