using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Validator for GetSalesRequest that defines validation rules for retrieving sales
/// </summary>
public class GetSalesRequestValidator : AbstractValidator<GetSalesRequest>
{
    /// <summary>
    /// Initializes a new instance of the GetSalesRequestValidator with defined validation rules
    /// </summary>
    public GetSalesRequestValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(request => request.Size)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.");

        RuleFor(request => request.Order)
            .Must(order => order == "asc" || order == "desc")
            .WithMessage("Order must be 'asc' or 'desc'.");
    }
}