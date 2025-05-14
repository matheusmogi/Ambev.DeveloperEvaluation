using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Command for deleting a sale
/// </summary>
public class DeleteSaleCommand : IRequest<bool>
{
    /// <summary>
    /// The unique identifier of the sale to delete
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of the DeleteSaleCommand
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    public DeleteSaleCommand(Guid id)
    {
        Id = id;
    }
}