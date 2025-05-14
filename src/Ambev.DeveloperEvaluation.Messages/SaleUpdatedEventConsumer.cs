using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Messages;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MassTransit;

namespace Ambev.DeveloperEvaluation.Messages;

public class SaleUpdatedEventConsumer : BaseSaleConsumer, IConsumer<UpdateSaleMessage>
{
    private readonly ISaleEventRepository _saleEventRepository;

    public SaleUpdatedEventConsumer(ISaleRepository saleRepository, ISaleEventRepository saleEventRepository) : base(
        saleRepository)
    {
        _saleEventRepository = saleEventRepository;
    }

    public async Task Consume(ConsumeContext<UpdateSaleMessage> context)
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

        await _saleEventRepository.CreateAsync(saleUpdated, SaleEventType.SaleUpdated, context.CancellationToken);
    }
}