using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleRequest that defines rules for validating the properties of a sale update operation,
/// including sale date, total item quantities, and individual sale item validation.
/// </summary>
public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    /// <summary>
    /// Validates an UpdateSaleRequest by enforcing rules on the sale items, including constraints on the total quantity
    /// of a product in the sale and delegating validation of individual sale items to the UpdateSaleItemRequestValidator.
    /// </summary>
    public UpdateSaleRequestValidator()
    {
        // Validate the total quantity of items in the sale event if the same product is added multiple times
        RuleFor(s => s.Items.GroupBy(g => g.ProductId)
                .Select(g => g.Sum(i => i.Quantity)))
            .Must(q => q.Sum() <= 20)
            .WithMessage("The total quantity of items of a product in the sale cannot exceed 20.");
        
        RuleForEach(s => s.Items).SetValidator(new UpdateSaleItemRequestValidator());
    }
}