using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Shared.Pagination;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Unit tests for GetSalesHandler
/// </summary>
public class GetSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSalesHandler _handler;

    public GetSalesHandlerTests()
    {
        var logger = Substitute.For<ILogger<GetSalesHandler>>();

        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSalesHandler(_saleRepository, _mapper, logger);
    }

    [Fact]
    public async Task Given_NoSalesFound_When_Handled_Then_ShouldReturnEmptyPaginatedList()
    {
        // Arrange
        var query = new GetSalesQuery
        {
            Page = 1,
            Size = 10,
            Order = "asc",
            Filters = new Dictionary<string, string>
            {
                { "BranchName", "Branch" }
            }
        };
        var sales = new List<Sale>();
        _saleRepository.GetAllAsync(query.Page, query.Size, query.Order, query.Filters).Returns(sales);
        _mapper.Map<List<SaleResult>>(sales).Returns([]);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.Equal(query.Page, result.CurrentPage);
        Assert.Equal(query.Size, result.PageSize);
        Assert.Equal(0, result.TotalCount);
        await _saleRepository
            .Received(1)
            .GetAllAsync(query.Page, query.Size, query.Order, query.Filters, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsPaginatedListOfSales()
    {
        // Arrange
        var query = new GetSalesQuery
        {
            Page = 1,
            Size = 10,
            Order = "asc",
            Filters = new Dictionary<string, string>
            {
                { "BranchName", "Branch" }
            }
        };

        List<Sale> sales =
        [
            CreateSaleTestData.GenerateValidSaleEntity(),
            CreateSaleTestData.GenerateValidSaleEntity()
        ];

        var salesResult = sales.ConvertAll(sale => new SaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            TotalAmount = sale.TotalAmount,
            IsCancelled = sale.Status == SaleStatus.Cancelled,
        });

        _mapper.Map<List<SaleResult>>(sales).Returns(salesResult);
        var paginatedList = new PaginatedList<SaleResult>(salesResult, salesResult.Count, query.Page, query.Size);

        _saleRepository.GetAllAsync(query.Page, query.Size, query.Order, query.Filters)
            .Returns(sales);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(query.Page, result.CurrentPage);
            Assert.Equal(query.Size, result.PageSize);
            Assert.Equal(paginatedList.Count, result.TotalCount);
        });
        await _saleRepository
            .Received(1)
            .GetAllAsync(query.Page, query.Size, query.Order, query.Filters, Arg.Any<CancellationToken>());
    }
}