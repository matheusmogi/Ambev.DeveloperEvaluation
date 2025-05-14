namespace Ambev.DeveloperEvaluation.Domain.Strategies.Discount;

internal class TwentyPercentDiscountStrategy : IDiscountStrategy
{
    private const decimal DiscountRate = 0.20m;

    public decimal Calculate(int quantity, decimal unitPrice)
    {
        return quantity * unitPrice * DiscountRate;
    }
}
