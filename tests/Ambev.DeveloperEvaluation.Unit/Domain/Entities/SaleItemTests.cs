using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleItemTests
{
    [Theory(DisplayName = "Validation should fail for invalid sale item data")]
    [InlineData(0, "", 0, 0)] // Invalid: default ID, empty name, zero quantity, zero price
    [InlineData(1, "", 1, 10)] // Invalid: empty name
    [InlineData(1, "Product", 21, 10)] // Invalid: quantity exceeds maximum
    [InlineData(1, "Product", 1, -10)] // Invalid: negative price
    [InlineData(-1, "Product", 1, 10)] // Invalid: negative ID
    [InlineData(1, "Product", 0, 10)] // Invalid: zero quantity
    [InlineData(1, "Product", 1, 0)] // Invalid: zero price
    public void Given_InvalidSaleItem_When_Validated_Then_ShouldReturnInvalid(
        int productId, string productName, int quantity, decimal unitPrice)
    {
        // Arrange
        var saleItem = new SaleItem
        {
            ProductId = productId,
            ProductName = productName,
            Quantity = quantity,
            UnitPrice = unitPrice
        };

        // Act
        var result = saleItem.Validate();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        });
    }

    [Theory(DisplayName = "Validation should pass for valid sale item data")]
    [InlineData(1, "Product", 1, 10)] // Valid: positive ID, non-empty name, positive quantity and price
    [InlineData(1, "Product", 20, 10)] // Valid: maximum quantity
    public void Given_ValidSaleItem_When_Validated_Then_ShouldReturnValid(
        int productId, string productName, int quantity, decimal unitPrice)
    {
        // Arrange
        var saleItem = new SaleItem
        {
            ProductId = productId,
            ProductName = productName,
            Quantity = quantity,
            UnitPrice = unitPrice
        };

        // Act
        var result = saleItem.Validate();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        });
    }
}