using Ambev.DeveloperEvaluation.Domain.Strategies.Discount;

namespace Ambev.DeveloperEvaluation.Domain.Strategies;

public static class SaleDiscountStrategy
{
    private const int MinQuantity = 4;
    private const int MaxQuantityForTenPercent = 9;
    private const int MaxQuantityForTwentyPercent = 20;

    internal static readonly Dictionary<Func<int, bool>, IDiscountStrategy> Strategies = new()
    {
        { quantity => quantity < MinQuantity, new NoDiscountStrategy() },
        { quantity => quantity is >= MinQuantity and <= MaxQuantityForTenPercent, new TenPercentDiscountStrategy() },
        { quantity => quantity is > MaxQuantityForTenPercent and <= MaxQuantityForTwentyPercent, new TwentyPercentDiscountStrategy() }
    };
}