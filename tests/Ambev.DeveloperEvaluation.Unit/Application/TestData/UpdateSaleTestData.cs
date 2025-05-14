using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class UpdateSaleTestData
{
    public static UpdateSaleCommand GenerateValidUpdateSaleCommand()
    {
        var faker = new Faker();
        return new Faker<UpdateSaleCommand>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(c => c.SaleNumber, f => f.Commerce.Ean8())
            .RuleFor(c => c.SaleDate, f => f.Date.Past())
            .RuleFor(c => c.CustomerId, f => f.Random.Int(1, 1000))
            .RuleFor(c => c.CustomerName, f => f.Person.FullName)
            .RuleFor(c => c.BranchId, f => f.Random.Int(1, 100))
            .RuleFor(c => c.BranchName, f => f.Company.CompanyName())
            .RuleFor(c => c.Items, f => GenerateValidUpdateSaleItemCommands())
            .RuleFor(c => c.IsCancelled, f => f.Random.Bool());
    }

    public static UpdateSaleCommand GenerateInvalidUpdateSaleCommand()
    {
        var faker = new Faker();
        return new Faker<UpdateSaleCommand>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(c => c.SaleNumber, f => "")
            .RuleFor(c => c.CustomerId, f => 0)
            .RuleFor(c => c.CustomerName, f => "")
            .RuleFor(c => c.BranchId, f => 0)
            .RuleFor(c => c.BranchName, f => "")
            .RuleFor(c => c.Items, f => new List<UpdateSaleItemCommand>())
            .Generate();
    }

    public static List<UpdateSaleItemCommand> GenerateValidUpdateSaleItemCommands()
    {
        return new Faker<UpdateSaleItemCommand>()
            .RuleFor(i => i.ProductId, f => f.Random.Int(1, 1000))
            .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
            .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(1, 100))
            .Generate(3);
    }

    public static Sale GenerateValidSale()
    {
        var faker = new Faker();
        return new Faker<Sale>()
            .RuleFor(s => s.Id, f => Guid.NewGuid())
            .RuleFor(s => s.SaleNumber, f => f.Commerce.Ean8())
            .RuleFor(s => s.SaleDate, f => f.Date.Past())
            .RuleFor(s => s.CustomerId, f => f.Random.Int(1, 1000))
            .RuleFor(s => s.CustomerName, f => f.Person.FullName)
            .RuleFor(s => s.BranchId, f => f.Random.Int(1, 100))
            .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
            .RuleFor(s => s.Items, f => GenerateValidSaleItems())
            .RuleFor(s => s.Status, f => f.PickRandom<SaleStatus>());
    }

    public static List<SaleItem> GenerateValidSaleItems()
    {
        return new Faker<SaleItem>()
            .RuleFor(i => i.ProductId, f => f.Random.Int(1, 1000))
            .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
            .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(1, 100))
            .Generate(3);
    }
    
    public static Sale GenerateValidSaleEntity()
    {
        var faker = new Faker();
        return new Faker<Sale>()
            .RuleFor(s => s.Id, f => Guid.NewGuid())
            .RuleFor(s => s.SaleNumber, f => f.Commerce.Ean8())
            .RuleFor(s => s.SaleDate, f => f.Date.Past())
            .RuleFor(s => s.CustomerId, f => f.Random.Int(1, 1000))
            .RuleFor(s => s.CustomerName, f => f.Person.FullName)
            .RuleFor(s => s.BranchId, f => f.Random.Int(1, 100))
            .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
            .RuleFor(s => s.Items, f => GenerateValidSaleItems())
            .RuleFor(s => s.Status, f => f.PickRandom<SaleStatus>());
    }
}