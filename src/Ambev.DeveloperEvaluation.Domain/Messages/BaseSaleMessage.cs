using MongoDB.Bson;

namespace Ambev.DeveloperEvaluation.Domain.Messages;

public abstract class BaseSaleMessage
{
    protected BaseSaleMessage(Guid saleId, DateTime created)
    {
        SaleId = saleId;
        Created = created;
    }
    public Guid SaleId { get; private set; } 
    public DateTime Created { get; private set; }

}