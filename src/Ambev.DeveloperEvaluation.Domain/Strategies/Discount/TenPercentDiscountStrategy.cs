namespace Ambev.DeveloperEvaluation.Domain.Strategies.Discount;

internal class TenPercentDiscountStrategy : IDiscountStrategy
{
    private const decimal DiscountRate = 0.10m;

    public decimal Calculate(int quantity, decimal unitPrice)
    {
        return quantity * unitPrice * DiscountRate;
    }
}
