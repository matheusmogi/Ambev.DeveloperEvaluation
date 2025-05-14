using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Shared.Pagination;
using Ambev.DeveloperEvaluation.Shared.Utils;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// The SalesController handles operations related to managing sales data.
/// </summary>
/// <remarks>
/// This class provides operations for creating, retrieving, updating, and deleting sales entries.
/// It utilizes the MediatR library for sending commands and queries and AutoMapper for mapping objects.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;


    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new sale based on the provided request
    /// </summary>
    /// <param name="request">The request containing the details for the sale to be created</param>
    /// <param name="cancellationToken">Token to observe for cancellation requests</param>
    /// <returns>A response indicating the result of the sale creation operation</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request,
        CancellationToken cancellationToken)
    {
        var validator = new CreateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<CreateSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
        {
            Success = true,
            Message = "Sale created successfully",
            Data = _mapper.Map<CreateSaleResponse>(response)
        });
    }

    /// <summary>
    /// Updates an existing sale with the provided details
    /// </summary>
    /// <param name="id">The unique identifier of the sale to be updated</param>
    /// <param name="request">The request containing the updated details of the sale</param>
    /// <param name="cancellationToken">Token to observe for cancellation requests</param>
    /// <returns>A response indicating the result of the sale update operation</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSale([FromRoute] Guid id, [FromBody] UpdateSaleRequest request,
        CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateSaleCommand>(request);
        command.Id = id;
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(data: new ApiResponseWithData<UpdateSaleResponse>
        {
            Success = true,
            Message = "Sale updated successfully",
            Data = _mapper.Map<UpdateSaleResponse>(response)
        });
    }

    /// <summary>
    /// Deletes an existing sale identified by the provided ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale to be deleted</param>
    /// <param name="cancellationToken">Token to observe for cancellation requests</param>
    /// <returns>A response indicating the result of the delete operation</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteSaleRequest { Id = id };
        var validator = new DeleteSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<DeleteSaleCommand>(request.Id);
        var response = await _mediator.Send(command, cancellationToken);

        if (!response)
            return NotFound($"Sale with ID {command.Id} not found or already cancelled");

        return Ok(data: response);
    }

    /// <summary>
    /// Retrieves a sale by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale to be retrieved</param>
    /// <param name="cancellationToken">Token to observe for cancellation requests</param>
    /// <returns>An HTTP response containing the sale data if found, or an appropriate error response otherwise</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetSaleRequest { Id = id };
        var validator = new GetSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var query = _mapper.Map<GetSaleQuery>(request.Id);
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(_mapper.Map<GetSaleResponse>(response));
    }

    /// <summary>
    /// Retrieves a paginated list of sales based on the provided query parameters.
    /// </summary>
    /// <param name="cancellationToken">Token to observe for cancellation requests.</param>
    /// <param name="page">The page number to retrieve. Default value is 1.</param>
    /// <param name="size">The size of the page to retrieve. Default value is 10.</param>
    /// <param name="order">The sort order for the results ("asc" for ascending or "desc" for descending). Default value is "asc".</param>
    /// <param name="filters">A dictionary of filters to apply to the sales query, where the key is the field and the value is the filter criterion. Filters are optional.</param>
    /// <returns>An IActionResult containing either the paginated list of sales wrapped in a successful ApiResponseWithData, or a bad request ApiResponse if validation fails.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<PaginatedList<GetSalesResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSales(CancellationToken cancellationToken,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string order = Constants.Ordering.Ascending,
        [FromQuery] Dictionary<string, string>? filters = null)
    {
        var request = new GetSalesRequest { Page = page, Size = size, Order = order, Filters = filters };
        var validator = new GetSalesRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var query = _mapper.Map<GetSalesQuery>(request);
        var result = await _mediator.Send(query, cancellationToken);
        var mappedResponse = _mapper.Map<List<GetSalesResponse>>(result);
        var response = new PaginatedList<GetSalesResponse>(mappedResponse,
            result.TotalCount,
            result.CurrentPage,
            result.PageSize);
        
        return OkPaginated(response);
    }
}