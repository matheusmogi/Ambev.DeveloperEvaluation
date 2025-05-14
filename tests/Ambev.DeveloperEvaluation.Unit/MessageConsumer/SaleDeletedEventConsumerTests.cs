using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Messages;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Messages;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.MessageConsumer;

public class SaleDeletedEventConsumerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly SaleDeletedEventConsumer _consumer;
    private readonly ISaleEventRepository _saleEventRepository;

    public SaleDeletedEventConsumerTests()
    {
        var logger = new LoggerFactory().CreateLogger<SaleDeletedEventConsumer>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _saleEventRepository = Substitute.For<ISaleEventRepository>();
        _consumer = new SaleDeletedEventConsumer(_saleRepository, _saleEventRepository, logger);
    }

    [Fact]
    public async Task Given_ExistingSale_When_Consumed_Then_ShouldSaveInTheSaleEventRepository()
    {
        // Arrange
        var sale = SaleTestData.GenerateSale();
        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(sale);

        var context = Substitute.For<ConsumeContext<CancelSaleMessage>>();
        context.Message.Returns(new CancelSaleMessage(sale.Id, DateTime.UtcNow));

        // Act
        await _consumer.Consume(context);

        // Assert
        await _saleEventRepository.Received(1).CreateAsync(
            Arg.Is<SaleEventData>(data => data.SaleId == sale.Id.ToString() && data.CustomerId == sale.CustomerId),
            SaleEventType.SaleDeleted,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Given_NotFoundSale_When_Consumed_Then_ShouldThrowException()
    {
        // Arrange
        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Sale)null!);
        var context = Substitute.For<ConsumeContext<CancelSaleMessage>>();
        context.Message.Returns(new CancelSaleMessage(Guid.Empty, DateTime.UtcNow));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _consumer.Consume(context));
        await _saleEventRepository.DidNotReceive().CreateAsync(
            Arg.Any<SaleEventData>(), Arg.Any<SaleEventType>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Consume_ShouldLogWarningAndReturn_WhenSaleIsAlreadyCancelled()
    {
        // Arrange
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
        };
        sale.Cancel();

        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(sale);

        var context = Substitute.For<ConsumeContext<CancelSaleMessage>>();
        context.Message.Returns(new CancelSaleMessage(Guid.Empty, DateTime.UtcNow));

        // Act
        await _consumer.Consume(context);

        // Assert
        await _saleEventRepository.DidNotReceive().CreateAsync(
            Arg.Any<SaleEventData>(), Arg.Any<SaleEventType>(), Arg.Any<CancellationToken>());
    }
}