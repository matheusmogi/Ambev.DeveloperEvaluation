using MongoDB.Bson;

namespace Ambev.DeveloperEvaluation.Domain.Messages;

public class CreateSaleMessage : BaseSaleMessage
{
    public CreateSaleMessage(Guid saleId, DateTime created) : base(saleId, created)
    {
    }
}