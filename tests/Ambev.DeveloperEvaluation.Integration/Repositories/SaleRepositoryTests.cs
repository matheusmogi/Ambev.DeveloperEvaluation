using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Repositories;

/// <summary>
/// Tests for SaleRepository
/// </summary>
public class SaleRepositoryTests
{
    private readonly DbContextOptions<DefaultContext> _dbContextOptions =
        new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;

    [Fact]
    public async Task GivenValidSale_WhenCreateAsyncCalled_ThenAddsSaleToDatabase()
    {
        // Arrange
        await using var context = new DefaultContext(_dbContextOptions);
        var sut = new SaleRepository(context);
        var newSale = SaleRepositoryTestData.GenerateSale();

        // Act
        var createdSale = await sut.CreateAsync(newSale);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(createdSale);
            Assert.Equal(newSale.Id, createdSale.Id);
        });
    }

    [Fact]
    public async Task GivenExistingSaleId_WhenGetByIdAsyncCalled_ThenReturnsSale()
    {
        // Arrange
        await using var context = new DefaultContext(_dbContextOptions);
        var sut = new SaleRepository(context);
        var existingSale = SaleRepositoryTestData.GenerateSale();
        await context.Sales.AddAsync(existingSale);
        await context.SaveChangesAsync();

        // Act
        var retrievedSale = await sut.GetByIdAsync(existingSale.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(retrievedSale);
            Assert.Equal(existingSale.Id, retrievedSale.Id);
        });
    }

    [Fact]
    public async Task GivenExistingSale_WhenUpdateAsyncCalled_ThenUpdatesSaleDetails()
    {
        // Arrange
        await using var context = new DefaultContext(_dbContextOptions);
        var sut = new SaleRepository(context);
        var originalSale = SaleRepositoryTestData.GenerateSale();
        await context.Sales.AddAsync(originalSale);
        await context.SaveChangesAsync();

        // Act
        originalSale.CustomerName = "Updated Customer";
        var updateResult = await sut.UpdateAsync(originalSale);

        // Assert
        var updatedSale = await sut.GetByIdAsync(originalSale.Id);
        Assert.Multiple(() =>
        {
            Assert.True(updateResult);
            Assert.Equal("Updated Customer", updatedSale?.CustomerName);
            Assert.Equal(originalSale.Id, updatedSale?.Id);
        });
    }

    [Fact]
    public async Task GivenExistingSaleId_WhenDeleteAsyncCalled_ThenCancelsSale()
    {
        // Arrange
        await using var context = new DefaultContext(_dbContextOptions);
        var sut = new SaleRepository(context);
        var saleToCancel = SaleRepositoryTestData.GenerateSale();
        await context.Sales.AddAsync(saleToCancel);
        await context.SaveChangesAsync();

        // Act
        var deleteResult = await sut.DeleteAsync(saleToCancel.Id);

        // Assert
        var cancelledSale = await sut.GetByIdAsync(saleToCancel.Id);
        Assert.Multiple(() =>
        {
            Assert.True(deleteResult);
            Assert.NotNull(cancelledSale);
            Assert.Equal(SaleStatus.Cancelled, cancelledSale.Status);
        });
    }

    [Fact]
    public async Task GivenSalesExist_WhenGetAllAsyncCalled_ThenReturnsPaginatedSales()
    {
        // Arrange
        await using var context = new DefaultContext(_dbContextOptions);
        var sut = new SaleRepository(context);
        for (var i = 0; i < 20; i++)
        {
            var sale = SaleRepositoryTestData.GenerateSale();
            await context.Sales.AddAsync(sale);
        }

        await context.SaveChangesAsync();

        // Act
        var paginatedSales = await sut.GetAllAsync(page: 2, size: 5);

        // Assert
        Assert.Equal(5, paginatedSales.Count());
    }
}