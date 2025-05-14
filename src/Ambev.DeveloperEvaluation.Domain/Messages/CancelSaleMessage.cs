using MongoDB.Bson;

namespace Ambev.DeveloperEvaluation.Domain.Messages;

public class CancelSaleMessage : BaseSaleMessage
{
    public CancelSaleMessage(Guid saleId, DateTime created) : base(saleId, created)
    {
    }
}