using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Strategies;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale transaction.
/// </summary>
public class Sale : BaseEntity
{
    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
        SaleNumber = GenerateSaleIdentifier();
        ApplyDiscounts();
    }

    /// <summary>
    /// Unique identifier for the sale.
    /// </summary>
    public string SaleNumber { get; private set; }

    /// <summary>
    /// Date when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Unique identifier for the customer (external identity).
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Name of the customer (denormalized).
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Total amount of the sale after discounts.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Total amount of the sale before discounts.
    /// </summary>
    public decimal TotalAmountBeforeDiscount { get; set; }

    /// <summary>
    /// Unique identifier for the branch (external identity).
    /// </summary>
    public int BranchId { get; set; }

    /// <summary>
    /// Name of the branch (denormalized).
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// List of items included in the sale.
    /// </summary>
    public List<SaleItem> Items { get; set; } = [];

    /// <summary>
    /// Status of the sale.
    /// </summary>
    public SaleStatus Status { get; private set; } = SaleStatus.None;

    /// <summary>
    /// Indicates the date and time when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Timestamp indicating when the sale was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets the total number of items in the sale.
    /// </summary>
    public int ItemCount => Items.Sum(item => item.Quantity);

    private static string GenerateSaleIdentifier()
    {
        return $"S-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

    /// <summary>
    /// Validates the <see cref="Sale"/> instance using the rules defined in the <see cref="SaleValidator"/> class.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> object containing the validation results,
    /// including whether the validation passed and any validation errors encountered.
    /// </returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Calculates the total amount for the sale, including the total before discounts and the final total after applying any applicable discounts.
    /// Updates the <see cref="TotalAmountBeforeDiscount"/> and <see cref="TotalAmount"/> properties with the computed values.
    /// </summary>
    public void CalculateTotalAmount()
    {
        TotalAmountBeforeDiscount = Items.Sum(item => item.Quantity * item.UnitPrice);
        TotalAmount = Items.Sum(item => item.CalculateTotal());
    }

    /// <summary>
    /// Cancels the sale.
    /// </summary>
    public void Cancel()
    {
        Status = SaleStatus.Canceled;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels the sale.
    /// </summary>
    public void Complete()
    {
        Status = SaleStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Applies any applicable discounts to the sale items. 
    /// </summary>
    private void ApplyDiscounts()
    {
        foreach (var item in Items.GroupBy(s => s.ProductId).Select(g => g.First()))
        {
            ApplyDiscount(item);
        }
    }

    /// <summary>
    /// Applies a discount to the sale item based on the quantity purchased.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="InvalidOperationException">Thrown when the quantity exceeds the maximum allowed for discount.</exception>
    private static void ApplyDiscount(SaleItem item)
    {
        var strategy = SaleRebateStrategy.Strategies.FirstOrDefault(pair => pair.Key(item.Quantity)).Value ??
                       throw new InvalidOperationException("Cannot sell more than 20 identical items.");
        item.Discount = strategy.CalculateDiscount(item.Quantity, item.UnitPrice);
    }
}