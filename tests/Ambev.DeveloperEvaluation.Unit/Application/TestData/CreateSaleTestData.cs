using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class CreateSaleTestData
{
    public static CreateSaleCommand GenerateValidCreateSaleCommand()
    {
        return new Faker<CreateSaleCommand>()
            .RuleFor(c => c.SaleDate, f => f.Date.Past())
            .RuleFor(c => c.CustomerId, f => f.Random.Int(1, 1000))
            .RuleFor(c => c.CustomerName, f => f.Person.FullName)
            .RuleFor(c => c.BranchId, f => f.Random.Int(1, 100))
            .RuleFor(c => c.BranchName, f => f.Company.CompanyName())
            .RuleFor(c => c.Items, f => GenerateValidCreateSaleItemCommands());
    }

    private static List<CreateSaleItemCommand> GenerateValidCreateSaleItemCommands()
    {
        return new Faker<CreateSaleItemCommand>()
            .RuleFor(i => i.ProductId, f => f.Random.Int(1, 10000))
            .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
            .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(1, 1000))
            .Generate(3);
    }

    public static Sale GenerateValidSaleEntity()
    {
        return new Faker<Sale>()
            .RuleFor(s => s.Id, f => Guid.NewGuid())
            .RuleFor(s => s.SaleNumber, f => f.Commerce.Ean8())
            .RuleFor(s => s.SaleDate, f => f.Date.Past())
            .RuleFor(s => s.CustomerId, f => f.Random.Int(1, 10000))
            .RuleFor(s => s.CustomerName, f => f.Person.FullName)
            .RuleFor(s => s.BranchId, f => f.Random.Int(1, 1000))
            .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
            .RuleFor(s => s.Items, f => GenerateValidSaleItemsEntity())
            .RuleFor(s => s.Status, f => f.PickRandom<SaleStatus>());
    }

    private static List<SaleItem> GenerateValidSaleItemsEntity()
    {
        return new Faker<SaleItem>()
            .RuleFor(i => i.Id, f => Guid.NewGuid())
            .RuleFor(i => i.ProductId, f => f.Random.Int(1, 10000))
            .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
            .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(1, 100))
            .Generate(3);
    }
}