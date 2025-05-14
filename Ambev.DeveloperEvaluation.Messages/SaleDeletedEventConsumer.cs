using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Messages;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MassTransit;

namespace Ambev.DeveloperEvaluation.Messages;

public class SaleDeletedEventConsumer : BaseSaleConsumer, IConsumer<CancelSaleMessage>
{
    private readonly ISaleEventRepository _saleEventRepository;

    public SaleDeletedEventConsumer(ISaleRepository saleRepository, ISaleEventRepository saleEventRepository) : base(
        saleRepository)
    {
        _saleEventRepository = saleEventRepository;
    }

    public async Task Consume(ConsumeContext<CancelSaleMessage> context)
    {
        var sale = await GetSaleAsync(context.Message.SaleId);

        var saleUpdated = new SaleEventData
        {
            SaleId = sale.Id.ToString(),
            CustomerId = sale.CustomerId,
            SaleDate = sale.SaleDate,
            TotalAmount = sale.TotalAmount,
            ItemCount = sale.ItemCount
        };

        await _saleEventRepository.CreateAsync(saleUpdated, SaleEventType.SaleDeleted, context.CancellationToken);
    }
}