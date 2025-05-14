using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Messages;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Messages;

public class SaleDeletedEventConsumer : BaseSaleConsumer, IConsumer<CancelSaleMessage>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly ILogger<SaleDeletedEventConsumer> _logger;

    public SaleDeletedEventConsumer(ISaleRepository saleRepository, ISaleEventRepository saleEventRepository,
        ILogger<SaleDeletedEventConsumer> logger) : base(
        saleRepository)
    {
        _saleEventRepository = saleEventRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CancelSaleMessage> context)
    {
        _logger.LogInformation("Consuming SaleDeletedEvent for Sale ID: {SaleId}", context.Message.SaleId);
        var sale = await GetSaleAsync(context.Message.SaleId);
        if (sale.Status == SaleStatus.Cancelled)
        {
            _logger.LogWarning("Sale ID: {SaleId} is already cancelled. No action taken", context.Message.SaleId);
            return;
        }

        var saleUpdated = new SaleEventData
        {
            SaleId = sale.Id.ToString(),
            CustomerId = sale.CustomerId,
            SaleDate = sale.SaleDate,
            TotalAmount = sale.TotalAmount,
            ItemCount = sale.ItemCount
        };

        _logger.LogInformation("Saving SaleDeletedEvent to NoSQL database for Sale ID: {SaleId}",
            context.Message.SaleId);
        await _saleEventRepository.CreateAsync(saleUpdated, SaleEventType.SaleDeleted, context.CancellationToken);
    }
}