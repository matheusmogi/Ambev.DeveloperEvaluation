using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the SaleValidator class.
/// </summary>
public class SaleValidatorTests
{
    private readonly SaleValidator _validator = new();

    [Fact]
    public void Given_InvalidCustomerId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = new Sale { CustomerId = 0 };

        // Act & Assert
        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.CustomerId).WithErrorMessage("Customer ID must be valid.");
    }

    [Fact]
    public void Given_InvalidCustomerName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = new Sale { CustomerName = "" };

        // Act & Assert
        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.CustomerName).WithErrorMessage("'Customer Name' must not be empty.");
    }

    [Fact]
    public void Given_EmptyItemsList_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = new Sale { Items = new List<SaleItem>() };

        // Act & Assert
        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.Items).WithErrorMessage("Sale must have at least one item.");
    }

    [Fact]
    public void Given_InvalidBranchId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = new Sale { BranchId = 0 };

        // Act & Assert
        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.BranchId).WithErrorMessage("Branch ID must be valid.");
    }

    [Fact]
    public void Given_InvalidBranchName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = new Sale { BranchName = "" };

        // Act & Assert
        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.BranchName).WithErrorMessage("'Branch Name' must not be empty.");
    }

    [Fact]
    public void Given_TotalItemsCount_When_GreaterThanTwenty_Then_ShouldHaveError()
    {
        // Arrange
        var sale = new Sale
        {
            CustomerId = 2,
            CustomerName = "customerName",
            BranchId = 1,
            BranchName = "branchName",
            Items =
            [
                SaleTestData.GetValidSaleItem(10),
                SaleTestData.GetValidSaleItem(10),
                SaleTestData.GetValidSaleItem(10)
            ]
        };

        // Act & Assert
        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.Items.GroupBy(a=>a.ProductId))
            .WithErrorMessage("The total quantity of items of a product in the sale cannot exceed 20.");
    }
}