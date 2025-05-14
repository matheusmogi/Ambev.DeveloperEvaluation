using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Profile for mapping UpdateSale feature requests to commands and results to responses
/// </summary>
public class UpdateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateSale feature
    /// </summary>
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleRequest, UpdateSaleCommand>();
        CreateMap<UpdateSaleItemRequest, UpdateSaleItemCommand>();
        CreateMap<UpdateSaleResult, UpdateSaleResponse>();
        CreateMap<UpdateSaleItemResult, UpdateSaleItemResponse>();
    }
}