using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Messages;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Messages;

public class SaleCreatedEventConsumer : BaseSaleConsumer, IConsumer<CreateSaleMessage>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly ILogger<SaleCreatedEventConsumer> _logger;

    public SaleCreatedEventConsumer(ISaleRepository saleRepository, ISaleEventRepository saleEventRepository,
        ILogger<SaleCreatedEventConsumer> logger) : base(
        saleRepository)
    {
        _saleEventRepository = saleEventRepository ?? throw new ArgumentNullException(nameof(saleEventRepository));
        _logger = logger?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Consume(ConsumeContext<CreateSaleMessage> context)
    {
        _logger.LogInformation("Consuming SaleCreatedEvent for Sale ID: {SaleId}", context.Message.SaleId);
        var sale = await GetSaleAsync(context.Message.SaleId);

        var saleCreated = new SaleEventData
        {
            SaleId = sale.Id.ToString(),
            CustomerId = sale.CustomerId,
            SaleDate = sale.SaleDate,
            TotalAmount = sale.TotalAmount,
            ItemCount = sale.ItemCount
        };

        // Save the event to the NoSQL database
        _logger.LogInformation("Saving SaleCreatedEvent to NoSQL database for Sale ID: {SaleId}", context.Message.SaleId);
        await _saleEventRepository.CreateAsync(saleCreated, SaleEventType.SaleCreated, context.CancellationToken);
    }
}