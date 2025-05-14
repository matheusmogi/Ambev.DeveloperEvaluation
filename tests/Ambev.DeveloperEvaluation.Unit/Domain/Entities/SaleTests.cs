using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Theory(DisplayName = "Validation should fail for invalid sale data")]
    [MemberData(nameof(InvalidSaleData))]
    public void Given_InvalidSale_When_Validated_Then_ShouldReturnInvalid(
        int customerId, string customerName, int branchId, string branchName, List<SaleItem> saleItems)
    {
        // Arrange
        var sale = new Sale
        {
            CustomerId = customerId,
            CustomerName = customerName,
            BranchId = branchId,
            BranchName = branchName,
            Items = saleItems
        };

        // Act
        var result = sale.Validate();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        });
    }

    [Theory(DisplayName = "Validation should pass for valid sale data")]
    [MemberData(nameof(ValidSaleData))]
    public void Given_ValidSale_When_Validated_Then_ShouldReturnValid(
        int customerId, string customerName, int branchId, string branchName, List<SaleItem> saleItems)
    {
        // Arrange
        var sale = new Sale
        {
            CustomerId = customerId,
            CustomerName = customerName,
            BranchId = branchId,
            BranchName = branchName,
            Items = saleItems
        };

        // Act
        var result = sale.Validate();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        });
    }

    public static IEnumerable<object[]> ValidSaleData()
    {
        yield return
            [1, "Customer", 1, "Branch", new List<SaleItem> { SaleTestData.GetValidSaleItem() }]; // Valid: single item
        yield return
        [
            1, "Customer", 1, "Branch", new List<SaleItem> { SaleTestData.GetValidSaleItem(20) }
        ]; // Valid: max quantity
        yield return
        [
            1, "Customer", 1, "Branch",
            new List<SaleItem> { SaleTestData.GetValidSaleItem(10), SaleTestData.GetValidSaleItem(10) }
        ]; // Valid: max quantity in different items
    }

    public static IEnumerable<object[]> InvalidSaleData()
    {
        yield return [0, "", 0, "", new List<SaleItem>()]; // Invalid: all fields empty or zero
        yield return [-1, "Customer", 1, "Branch", new List<SaleItem>()]; // Invalid: negative customer ID
        yield return [1, "", 1, "Branch", new List<SaleItem>()]; // Invalid: empty customer name
        yield return [1, "Customer", 0, "", new List<SaleItem>()]; // Invalid: zero branch ID and empty branch name
        yield return
        [
            1, "Customer", 1, "Branch", new List<SaleItem> { new() { ProductId = 1, Quantity = 21, UnitPrice = 10 } }
        ]; // Invalid: item quantity exceeds max
        yield return
        [
            1, "Customer", 1, "Branch", new List<SaleItem>
            {
                new() { ProductId = 1, Quantity = 11, UnitPrice = 10 },
                new() { ProductId = 1, Quantity = 10, UnitPrice = 10 }
            }
        ]; // Invalid: item quantity exceeds max in different items
    }
}