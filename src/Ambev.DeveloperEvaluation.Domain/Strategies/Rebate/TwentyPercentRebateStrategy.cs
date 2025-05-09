namespace Ambev.DeveloperEvaluation.Domain.Strategies.Rebate;

internal class TwentyPercentRebateStrategy : IDiscountStrategy
{
    private const decimal DiscountRate = 0.20m;

    public decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        return quantity * unitPrice * DiscountRate;
    }
}
