using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Messages;

public abstract class BaseSaleConsumer
{
    private readonly ISaleRepository _saleRepository;

    public BaseSaleConsumer(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    protected async Task<Sale> GetSaleAsync(Guid saleId)
    {
        var sale = await _saleRepository.GetByIdAsync(saleId);
        if (sale == null)
        {
            throw new ArgumentNullException(nameof(sale));
        }

        return sale;
    }
}