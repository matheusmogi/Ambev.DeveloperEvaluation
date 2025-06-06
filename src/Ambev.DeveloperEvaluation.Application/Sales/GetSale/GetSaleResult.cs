﻿namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Result for retrieving a sale by its unique identifier
/// </summary>
public class GetSaleResult
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
    public List<GetSaleItemResult> Items { get; set; } = new();

    /// <summary>
    /// Indicates whether the sale is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
}

/// <summary>
/// Result for a sale item in the GetSaleResult
/// </summary>
public class GetSaleItemResult
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




