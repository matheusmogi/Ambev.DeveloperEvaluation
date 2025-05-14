using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

internal static class SaleTestData
{
    internal static SaleItem GetValidSaleItem(int? quantity = null)
    {
        return FakerItem(quantity).Generate();
    }

    private static Faker<SaleItem> FakerItem(int? quantity)
    {
        return new Faker<SaleItem>()
            .RuleFor(r => r.Quantity, f => quantity ?? f.Random.Int(1, 20))
            .RuleFor(r => r.ProductId, f => f.Random.Int(1, 1000))
            .RuleFor(r => r.ProductName, f => f.Commerce.ProductName())
            .RuleFor(r => r.UnitPrice, f => f.Random.Decimal(0.01m, 1000.00m));
    }
}