using Ambev.DeveloperEvaluation.Domain.Strategies.Discount;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Strategies.Discount;

public class DiscountStrategyTests
{
    [Fact]
    public void Given_NoDiscountStrategy_When_CalculateDiscount_Then_NoDiscountShouldBeApplied()
    {
        // Arrange
        var strategy = new NoDiscountStrategy();
        const int quantity = 5;
        const decimal unitPrice = 10m;

        // Act
        var actual = strategy.Calculate(quantity, unitPrice);

        // Assert
        Assert.Equal(0m, actual);
    }

    [Fact]
    public void Given_TenPercentDiscountStrategy_When_CalculateDiscount_Then_TenPercentDiscountShouldBeApplied()
    {
        // Arrange
        var strategy = new TenPercentDiscountStrategy();
        const int quantity = 5;
        const decimal unitPrice = 10m;

        // Act
        var actual = strategy.Calculate(quantity, unitPrice);

        // Assert
        Assert.Equal(5m, actual);
    }

    [Fact]
    public void Given_TwentyPercentDiscountStrategy_When_CalculateDiscount_Then_TwentyPercentDiscountShouldBeApplied()
    {
        // Arrange
        var strategy = new TwentyPercentDiscountStrategy();
        const int quantity = 5;
        const decimal unitPrice = 10m;

        // Act
        var actual = strategy.Calculate(quantity, unitPrice);

        // Assert
        Assert.Equal(10m, actual);
    }
}