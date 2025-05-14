using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleItemRequest that defines validation rules
/// for validating the properties of individual sale items in an update operation
/// </summary>
public class UpdateSaleItemRequestValidator : AbstractValidator<UpdateSaleItemRequest>
{
    /// <summary>
    /// Validates an UpdateSaleItemRequest by defining rules for properties such as product ID, name, quantity, unit price, and others
    /// </summary>
    public UpdateSaleItemRequestValidator()
    {
        RuleFor(item => item.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

        RuleFor(item => item.ProductName)
            .NotEmpty().Length(3, 100).WithMessage("Product name must be between 3 and 100 characters.");

        RuleFor(item => item.Quantity)
            .InclusiveBetween(1, 20).WithMessage("Quantity must be between 1 and 20.");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0.");
    }
}