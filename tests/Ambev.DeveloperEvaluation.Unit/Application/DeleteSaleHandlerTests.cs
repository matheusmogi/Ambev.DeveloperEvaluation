using Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCancelled;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the DeleteSaleHandler class.
/// </summary>
public class DeleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IPublisher _publisher;
    private readonly DeleteSaleHandler _handler;

    public DeleteSaleHandlerTests()
    {
        var logger = Substitute.For<ILogger<DeleteSaleHandler>>();

        _saleRepository = Substitute.For<ISaleRepository>();
        _publisher = Substitute.For<IPublisher>();
        _handler = new DeleteSaleHandler(_saleRepository, _publisher, logger);
    }

    [Fact]
    public async Task Given_InvalidCommand_When_Handled_Then_ShouldThrowValidationException()
    {
        // Arrange
        var saleId = Guid.Empty;
        var command = new DeleteSaleCommand(saleId);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Given_SaleInactiveOrNonExistent_When_Handled_Then_ShouldReturnFalse()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new DeleteSaleCommand(saleId);

        _saleRepository.IsActiveAsync(command.Id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        await _saleRepository
            .Received(0)
            .DeleteAsync(command.Id, Arg.Any<CancellationToken>());

        await _publisher
            .Received(0)
            .Publish(Arg.Is<SaleCancelledNotification>(n => n.SaleId == saleId),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Given_ActiveSale_When_Handled_Then_ShouldDeleteSaleSuccessfully()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new DeleteSaleCommand(saleId);

        _saleRepository.IsActiveAsync(command.Id, Arg.Any<CancellationToken>()).Returns(true);
        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        await _saleRepository
            .Received(1)
            .DeleteAsync(command.Id, Arg.Any<CancellationToken>());

        await _publisher
            .Received(1)
            .Publish(Arg.Is<SaleCancelledNotification>(n => n.SaleId == saleId),
                Arg.Any<CancellationToken>());
    }
}