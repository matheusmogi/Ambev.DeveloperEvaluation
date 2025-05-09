namespace Ambev.DeveloperEvaluation.Domain.Strategies.Rebate;

internal class NoRebateStrategy : IDiscountStrategy
{
    public decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        return 0;
    }
}
