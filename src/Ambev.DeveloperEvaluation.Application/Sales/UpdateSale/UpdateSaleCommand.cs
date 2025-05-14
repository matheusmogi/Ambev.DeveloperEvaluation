using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Command for updating a sale
/// </summary>
public class UpdateSaleCommand : IRequest<UpdateSaleResult>
{
    /// <summary>
    /// The unique identifier of the sale
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The sale number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// The date when the sale was made
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// The customer ID (external identity)
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// The customer name (denormalized)
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// The total sale amount
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// The branch ID (external identity)
    /// </summary>
    public int BranchId { get; set; }

    /// <summary>
    /// The branch name (denormalized)
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// The list of items in the sale
    /// </summary>
    public List<UpdateSaleItemCommand> Items { get; set; } = new();

    /// <summary>
    /// Indicates whether the sale is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
}

/// <summary>
/// Command for a sale item in the UpdateSaleCommand
/// </summary>
public class UpdateSaleItemCommand
{
    /// <summary>
    /// The unique identifier of the sale item
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The product ID
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// The product name
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// The quantity of the product
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// The discount amount
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// The total amount of the sale item
    /// </summary>
    public decimal TotalAmount { get; set; }
}