using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Shared.Pagination;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Handler for processing GetSalesQuery requests
/// </summary>
public class GetSalesHandler : IRequestHandler<GetSalesQuery, PaginatedList<SaleResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSalesHandler> _logger;

    /// <summary>
    /// Initializes a new instance of GetSalesHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<GetSalesHandler> logger)
    {
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the GetSalesQuery request
    /// </summary>
    /// <param name="query">The GetSales query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paginated list of sales</returns>
    public async Task<PaginatedList<SaleResult>> Handle(GetSalesQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetSalesQuery. Page: {Page}, Size: {Size}, Order: {Order}, Filters: {Filters}",
            query.Page, query.Size, query.Order, query.Filters);

        var sales = await _saleRepository.GetAllAsync(query.Page, query.Size, query.Order, query.Filters, cancellationToken);
        var salesResult = _mapper.Map<List<SaleResult>>(sales);

        var paginatedList = new PaginatedList<SaleResult>(salesResult, salesResult.Count, query.Page, query.Size);

        _logger.LogInformation("Returning paginated list of sales. CurrentPage: {CurrentPage}, TotalPages: {TotalPages}, TotalCount: {TotalCount}",
            paginatedList.CurrentPage, paginatedList.TotalPages, paginatedList.TotalCount);

        return paginatedList;
    }
}

