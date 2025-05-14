using Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleModified;
using Ambev.DeveloperEvaluation.Domain.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Notification;

public class SaleUpdatedNotificationHandlerTests
{
    private readonly SaleUpdatedNotificationHandler _handler;
    private readonly IPublishEndpoint _publishEndpoint;

    public SaleUpdatedNotificationHandlerTests()
    {
        var logger = new LoggerFactory().CreateLogger<SaleUpdatedNotificationHandler>();

        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        _handler = new SaleUpdatedNotificationHandler(_publishEndpoint, logger);
    }

    [Fact]
    public async Task Handle_ValidNotification_ShouldPublishNotification()
    {
        // Arrange
        var notification = new SaleUpdatedNotification
        {
            SaleId = Guid.NewGuid(),
        };
        var @event = new UpdateSaleMessage(notification.SaleId, DateTime.UtcNow);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        await _publishEndpoint.Received(1)
            .Publish(Arg.Is<UpdateSaleMessage>(c =>
                    c.SaleId == @event.SaleId &&
                    c.Created.Date == @event.Created.Date),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NullNotification_ShouldThrowArgumentNullException()
    {
        // Arrange
        SaleUpdatedNotification? notification = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(notification, CancellationToken.None));
    }
}