namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Result for retrieving sales with pagination, ordering, and filtering
/// </summary>
public class SaleResult
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
    /// Indicates whether the sale is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
}