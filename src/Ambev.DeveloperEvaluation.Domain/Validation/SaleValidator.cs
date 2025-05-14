using Ambev.DeveloperEvaluation.Domain.Entities;

using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(s => s.SaleNumber)
            .NotEmpty().WithMessage("Sale number is mandatory.");

        RuleFor(s => s.CustomerId)
            .GreaterThan(0).WithMessage("Customer ID must be valid.");
        
        RuleFor(s => s.CustomerName)
            .NotEmpty().MaximumLength(200).WithMessage("Customer name is mandatory and must be less than 200 characters.");

        RuleFor(s => s.BranchId)
            .GreaterThan(0).WithMessage("Branch ID must be valid."); 
        
        RuleFor(s => s.BranchName)
            .NotEmpty().MaximumLength(200).WithMessage("Branch name is mandatory and must be less than 200 characters.");

        RuleFor(s => s.Items)
            .NotEmpty().WithMessage("Sale must have at least one item.");
        
        // Validate the total quantity of items in the sale event if the same product is added multiple times
        RuleFor(s => s.Items.GroupBy(g => g.Id)
                .Select(g => g.Sum(i => i.Quantity)))
            .Must(q => q.Sum() <= 20).WithMessage("The total quantity of items of a product in the sale cannot exceed 20.");

        RuleForEach(s => s.Items).SetValidator(new SaleItemValidator());
    }
}
