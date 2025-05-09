using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item in a sale transaction, including details about the product,
/// quantity, unit price, discount, and total amount.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary> 
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets the discount applied to the sale item.
    /// </summary> 
    public decimal Discount { get; internal set; }

    /// <summary>
    /// Gets the total monetary amount for the sale item after applying the quantity, unit price, and discount.
    /// </summary>
    public decimal TotalAmount { get; internal set; }

    /// <summary>
    /// Gets or sets the unique identifier of the sale to which this item belongs.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Validates the sale item using the defined SaleItemValidator and generates a validation result.
    /// </summary>
    /// <returns>
    /// A ValidationResultDetail containing the validation outcome and any errors if validation fails.
    /// </returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Calculates the total amount for the sale item by considering the quantity, unit price, and discount.
    /// Updates the TotalAmount property with the computed value.
    /// </summary>
    /// <returns>
    /// The total calculated amount for the sale item after applying the discount.
    /// </returns>
    public decimal CalculateTotal()
    {
        TotalAmount = Quantity * UnitPrice - Discount;
        return TotalAmount;
    }

}