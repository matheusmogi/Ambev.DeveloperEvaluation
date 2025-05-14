using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Handler for processing GetSaleQuery requests
/// </summary>
public class GetSaleHandler : IRequestHandler<GetSaleQuery, GetSaleResult?>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of GetSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public GetSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<GetSaleHandler> logger)
    {
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the GetSaleQuery request
    /// </summary>
    /// <param name="query">The GetSale query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale details if found</returns>
    public async Task<GetSaleResult?> Handle(GetSaleQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetSaleQuery for Sale ID: {SaleId}", query.Id);
        var sale = await _saleRepository.GetByIdAsync(query.Id, cancellationToken);
        if (sale is null)
        {
            _logger.LogWarning("Sale with ID {SaleId} not found", query.Id);
            return default;
        }

        var result = _mapper.Map<GetSaleResult>(sale);
        _logger.LogInformation("Successfully retrieved Sale with ID: {SaleId}", query.Id);

        return result;
    }
}


