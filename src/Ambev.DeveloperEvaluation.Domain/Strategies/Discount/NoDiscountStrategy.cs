namespace Ambev.DeveloperEvaluation.Domain.Strategies.Discount;

internal class NoDiscountStrategy : IDiscountStrategy
{
    public decimal Calculate(int quantity, decimal unitPrice)
    {
        return 0;
    }
}
