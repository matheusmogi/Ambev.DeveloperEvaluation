using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCreated;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;
    private readonly ILogger<CreateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="publisher">The MediatR publisher instance</param>
    /// <param name="logger">The logger instance</param>
    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IPublisher publisher, ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateSaleCommand for Customer ID: {CustomerId}, Sale Date: {SaleDate}", command.CustomerId, command.SaleDate);
        var sale = _mapper.Map<Sale>(command);
        
        sale.ApplyDiscounts();
        sale.CalculateTotalAmount();

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
        var result = _mapper.Map<CreateSaleResult>(createdSale);
        result.ItemCount = sale.ItemCount;
        _logger.LogInformation("Sale created successfully with ID: {SaleId}, Total Amount: {TotalAmount}", createdSale.Id, createdSale.TotalAmount);

        await _publisher.Publish(new SaleCreatedNotification { SaleId = createdSale.Id }, cancellationToken);
        _logger.LogInformation("Published SaleCreatedNotification for Sale ID: {SaleId}", createdSale.Id);

        return result;
    }
}



