using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Profile for mapping between Sale entity and GetSaleResult
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSale operation
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<Sale, GetSaleResult>()
            .ForMember(dest => dest.IsCancelled,
                opt => opt.MapFrom(src => src.Status == SaleStatus.Cancelled));
        CreateMap<SaleItem, GetSaleItemResult>();
    }
}