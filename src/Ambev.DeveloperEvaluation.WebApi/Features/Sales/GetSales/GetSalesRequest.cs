namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// API request model for GetSales operation
/// </summary>
public class GetSalesRequest
{
    /// <summary>
    /// Page number (default: 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page (default: 10)
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Order of the elements in the collection (asc or desc, default: asc)
    /// </summary>
    public string Order { get; set; } = "asc";

    /// <summary>
    /// Dictionary of filters to apply
    /// </summary>
    public Dictionary<string, string>? Filters { get; set; }
}