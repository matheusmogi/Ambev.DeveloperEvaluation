using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="GetSaleHandler"/> class.
/// </summary>
public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleHandler _handler;

    public GetSaleHandlerTests()
    {
        var logger = new LoggerFactory().CreateLogger<GetSaleHandler>();

        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSaleHandler(_saleRepository, _mapper, logger);
    }

    [Fact]
    public async Task Given_SaleNotFound_When_Handled_Then_ShouldReturnNull()
    {
        // Arrange
        var query = new GetSaleQuery(Guid.NewGuid());
        _saleRepository.GetByIdAsync(query.Id).Returns((Sale?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
        await _saleRepository
            .Received(1)
            .GetByIdAsync(query.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Given_ValidQuery_When_Handled_Then_ShouldReturnSaleResult()
    {
        // Arrange
        var sale = CreateSaleTestData.GenerateValidSaleEntity();
        var query = new GetSaleQuery(sale.Id);

        var saleResult = new GetSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            Items = sale.Items.ConvertAll(i => new GetSaleItemResult
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalAmount = i.TotalAmount
            }),
            IsCancelled = sale.Status == SaleStatus.Cancelled
        };

        _saleRepository.GetByIdAsync(query.Id).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(saleResult);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(saleResult);
    }
}