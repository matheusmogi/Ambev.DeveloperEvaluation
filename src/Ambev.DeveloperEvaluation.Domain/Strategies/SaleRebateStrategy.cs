using Ambev.DeveloperEvaluation.Domain.Strategies.Rebate;

namespace Ambev.DeveloperEvaluation.Domain.Strategies;

internal static class SaleRebateStrategy
{
    private const int MinQuantity = 4;
    private const int MaxQuantityForTenPercent = 9;
    private const int MaxQuantityForTwentyPercent = 20;

    internal static readonly Dictionary<Func<int, bool>, IDiscountStrategy> Strategies = new()
    {
        { quantity => quantity < MinQuantity, new NoRebateStrategy() },
        { quantity => quantity is >= MinQuantity and <= MaxQuantityForTenPercent, new TenPercentRebateStrategy() },
        { quantity => quantity is > MaxQuantityForTenPercent and <= MaxQuantityForTwentyPercent, new TwentyPercentRebateStrategy() }
    };
}