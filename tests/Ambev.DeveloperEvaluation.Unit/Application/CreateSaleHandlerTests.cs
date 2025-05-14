using Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCreated;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Unit tests for the CreateSaleHandler class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        var logger = Substitute.For<ILogger<CreateSaleHandler>>();

        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _publisher = Substitute.For<IPublisher>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _publisher, logger);
    }

    [Fact]
    public async Task Given_ValidCommand_When_Handled_Then_ShouldCreateSaleWithDiscountsCalculation()
    {
        // Arrange
        var command = CreateSaleTestData.GenerateValidCreateSaleCommand();
        var sale = CreateSaleTestData.GenerateValidSaleEntity();
        var createSaleResult = new CreateSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            Items = sale.Items.ConvertAll(i => new CreateSaleItemResult
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalAmount = i.TotalAmount
            }),
            TotalAmount = sale.TotalAmount,
            ItemCount = sale.Items.Count,
            IsCancelled = sale.Status == SaleStatus.Cancelled
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(createSaleResult);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(createSaleResult, result);
        });
        await _saleRepository
            .Received(1)
            .CreateAsync(Arg.Is<Sale>(s => s == sale), Arg.Any<CancellationToken>());
        await _publisher
            .Received(1)
            .Publish(Arg.Is<SaleCreatedNotification>(n => n.SaleId == sale.Id),
                Arg.Any<CancellationToken>());
    }
}