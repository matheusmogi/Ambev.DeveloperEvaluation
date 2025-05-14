using MongoDB.Bson;

namespace Ambev.DeveloperEvaluation.Domain.Messages;

public class UpdateSaleMessage: BaseSaleMessage
{
    public UpdateSaleMessage(Guid saleId, DateTime created) : base(saleId, created)
    {
    }
}