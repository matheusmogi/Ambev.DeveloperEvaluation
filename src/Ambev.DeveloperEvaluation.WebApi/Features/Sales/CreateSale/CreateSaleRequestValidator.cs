using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleRequest that defines validation rules for sale creation.
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
 
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleRequestValidator"/> class.
    /// </summary>
    public CreateSaleRequestValidator()
    {
        RuleFor(sale => sale.SaleDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Sale date must be a valid date and not in the future.");

        // Validate the total quantity of items in the sale event if the same product is added multiple times
        RuleFor(s => s.Items.GroupBy(g => g.ProductId)
                .Select(g => g.Sum(i => i.Quantity)))
            .Must(q => q.Sum() <= 20)
            .WithMessage("The total quantity of items of a product in the sale cannot exceed 20.");
        
        RuleForEach(s => s.Items).SetValidator(new CreateSaleItemValidator());
    }
}
