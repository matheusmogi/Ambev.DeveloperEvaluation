using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Repositories;

public static class SaleRepositoryTestData
{
    public static Sale GenerateSale()
    {
        var saleFaker = new Faker<Sale>()
            .RuleFor(s => s.Id, f => Guid.NewGuid())
            .RuleFor(s => s.SaleDate, f => f.Date.Past().ToUniversalTime())
            .RuleFor(s => s.CreatedAt, f => f.Date.Past().ToUniversalTime())
            .RuleFor(s => s.CustomerName, f => f.Person.FullName)
            .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
            .RuleFor(s => s.Items, f => GenerateSaleItems());

        return saleFaker.Generate();
    }

    public static List<SaleItem> GenerateSaleItems(int count = 3)
    {
        var saleItemFaker = new Faker<SaleItem>()
            .RuleFor(si => si.Id, f => Guid.NewGuid())
            .RuleFor(si => si.ProductId, f => f.Random.Int(1, 1000))
            .RuleFor(si => si.ProductName, f => f.Commerce.ProductName())
            .RuleFor(si => si.Quantity, f => f.Random.Int(1, 20))
            .RuleFor(si => si.UnitPrice, f => f.Finance.Amount())
            .RuleFor(si => si.Discount, f => f.Finance.Amount(0, 10))
            .RuleFor(si => si.TotalAmount, (f, si) => si.Quantity * si.UnitPrice - si.Discount)
            .RuleFor(si => si.SaleId, f => Guid.NewGuid());

        return saleItemFaker.Generate(count);
    }
}
