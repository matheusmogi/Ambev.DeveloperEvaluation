using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Query for retrieving a sale by its unique identifier
/// </summary>
public class GetSaleQuery : IRequest<GetSaleResult>
{
    /// <summary>
    /// The unique identifier of the sale
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of the GetSaleQuery
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    public GetSaleQuery(Guid id)
    {
        Id = id;
    }
}