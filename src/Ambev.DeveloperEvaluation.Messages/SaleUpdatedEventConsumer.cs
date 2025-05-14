using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Messages;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Messages;

public class SaleUpdatedEventConsumer : BaseSaleConsumer, IConsumer<UpdateSaleMessage>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly ILogger<SaleUpdatedEventConsumer> _logger;

    public SaleUpdatedEventConsumer(ISaleRepository saleRepository, ISaleEventRepository saleEventRepository,
        ILogger<SaleUpdatedEventConsumer> logger) : base(
        saleRepository)
    {
        _saleEventRepository = saleEventRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UpdateSaleMessage> context)
    {
        _logger.LogInformation("Consuming SaleUpdatedEvent for Sale ID: {SaleId}", context.Message.SaleId);
        var sale = await GetSaleAsync(context.Message.SaleId);

        var saleUpdated = new SaleEventData
        {
            SaleId = sale.Id.ToString(),
            CustomerId = sale.CustomerId,
            SaleDate = sale.SaleDate,
            TotalAmount = sale.TotalAmount,
            ItemCount = sale.ItemCount
        };

        _logger.LogInformation("Saving SaleUpdatedEvent to NoSQL database for Sale ID: {SaleId}",
            context.Message.SaleId);
        await _saleEventRepository.CreateAsync(saleUpdated, SaleEventType.SaleUpdated, context.CancellationToken);
    }
}