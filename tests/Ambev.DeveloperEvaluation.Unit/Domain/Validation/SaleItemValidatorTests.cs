using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;

using FluentValidation.TestHelper;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the SaleItemValidator class.
/// </summary>
public class SaleItemValidatorTests
{
    private readonly SaleItemValidator _validator = new();

    [Fact]
    public void Given_InvalidQuantity_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = new SaleItem { Quantity = 0 };

        // Act & Assert
        var result = _validator.TestValidate(saleItem);
        result.ShouldHaveValidationErrorFor(item => item.Quantity).WithErrorMessage("Quantity must be between 1 and 20.");
    }

    [Fact]
    public void Given_InvalidUnitPrice_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = new SaleItem { UnitPrice = 0m };

        // Act & Assert
        var result = _validator.TestValidate(saleItem);
        result.ShouldHaveValidationErrorFor(item => item.UnitPrice).WithErrorMessage("Unit price must be greater than 0.");
    }
    
    [Fact]
    public void Given_InvalidProductName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = new SaleItem { ProductName = "" };

        // Act & Assert
        var result = _validator.TestValidate(saleItem);
        result.ShouldHaveValidationErrorFor(item => item.ProductName).WithErrorMessage("'Product Name' must not be empty.");
    }
    
    [Fact]
    public void Given_InvalidProductId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = new SaleItem { ProductId = 0 };

        // Act & Assert
        var result = _validator.TestValidate(saleItem);
        result.ShouldHaveValidationErrorFor(item => item.ProductId).WithErrorMessage("Product ID must be valid.");
    }
}