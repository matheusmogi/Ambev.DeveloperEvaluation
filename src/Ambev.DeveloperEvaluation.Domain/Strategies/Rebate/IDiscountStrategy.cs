namespace Ambev.DeveloperEvaluation.Domain.Strategies.Rebate;

/// <summary>
/// Defines a strategy for calculating discounts on sale items.
/// </summary>
internal interface IDiscountStrategy
{
    /// <summary>
    /// Calculates the discount amount for a sale item based on the specified quantity and unit price.
    /// </summary>
    /// <param name="quantity">The quantity of the product being purchased.</param>
    /// <param name="unitPrice">The unit price of the product.</param>
    /// <returns>The calculated discount amount.</returns>
    decimal CalculateDiscount(int quantity, decimal unitPrice);
}


