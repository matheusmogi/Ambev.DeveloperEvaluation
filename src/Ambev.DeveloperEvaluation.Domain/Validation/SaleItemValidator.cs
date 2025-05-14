using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(item => item.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be valid.");
        
        RuleFor(item => item.ProductName)
            .NotEmpty().MaximumLength(200).WithMessage("Product name is mandatory and must be less than 200 characters.");

        RuleFor(item => item.Quantity)
            .InclusiveBetween(1, 20).WithMessage("Quantity must be between 1 and 20.");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0.");
    }
}