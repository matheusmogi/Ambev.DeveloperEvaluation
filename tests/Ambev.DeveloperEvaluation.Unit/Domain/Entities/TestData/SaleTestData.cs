using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

internal static class SaleTestData
{
    public static Sale GenerateSale()
    {
        var saleFaker = new Faker<Sale>()
            .RuleFor(s => s.Id, _ => Guid.NewGuid())
            .RuleFor(s => s.SaleDate, f => f.Date.Past().ToUniversalTime())
            .RuleFor(s => s.CreatedAt, f => f.Date.Past().ToUniversalTime())
            .RuleFor(s => s.CustomerName, f => f.Person.FullName)
            .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
            .RuleFor(s => s.Items, _ => [GetValidSaleItem(10)]);

        return saleFaker.Generate();
    }

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