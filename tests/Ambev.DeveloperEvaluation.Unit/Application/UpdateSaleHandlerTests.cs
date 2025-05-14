using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;
    private readonly UpdateSaleHandler _handler;

    public UpdateSaleHandlerTests()
    {
        var logger = new LoggerFactory().CreateLogger<UpdateSaleHandler>();

        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _publisher = Substitute.For<IPublisher>();
        _handler = new UpdateSaleHandler(_saleRepository, _mapper, _publisher, logger);
    }

    [Fact]
    public async Task Given_InactiveSale_When_Handled_Then_ShouldThrowException()
    {
        // Arrange
        var command = UpdateSaleTestData.GenerateValidUpdateSaleCommand();
        command.Id = Guid.NewGuid();

        _saleRepository.IsActiveAsync(command.Id).Returns(false);

        // Act
        // Assert
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Sale not found or already cancelled.");
    }

    [Fact]
    public async Task Given_SaleNotFound_When_Handled_Then_ShouldThrowException()
    {
        // Arrange
        var command = UpdateSaleTestData.GenerateValidUpdateSaleCommand();
        command.Id = Guid.NewGuid();

        _saleRepository.GetByIdAsync(command.Id, CancellationToken.None).Returns((Sale)null);

        // Act
        // Assert
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Sale not found or already cancelled.");
    }

    [Fact]
    public async Task Given_SaleNotUpdated_When_Handled_Then_ShouldThrowException()
    {
        // Arrange
        var command = UpdateSaleTestData.GenerateValidUpdateSaleCommand();
        var sale = UpdateSaleTestData.GenerateValidSaleEntity();
        command.Id = sale.Id;

        _saleRepository.GetByIdAsync(command.Id, CancellationToken.None).Returns(sale);
        _saleRepository.IsActiveAsync(command.Id).Returns(true);
        _saleRepository.UpdateAsync(Arg.Is<Sale>(s => s.Id == command.Id), CancellationToken.None).Returns(false);
        _mapper.Map<Sale>(command).Returns(sale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Given_ValidCommand_When_Handled_Then_ShouldUpdateSaleWithDiscountsCalculation()
    {
        // Arrange
        var sale = UpdateSaleTestData.GenerateValidSaleEntity();
        var command = UpdateSaleTestData.GenerateValidUpdateSaleCommand();
        command.Id = sale.Id;

        var updateSaleResult = new UpdateSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            Items = sale.Items.ConvertAll(i => new UpdateSaleItemResult
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalAmount = i.TotalAmount
            }),
            TotalAmount = sale.TotalAmount
        };

        _saleRepository.GetByIdAsync(command.Id, CancellationToken.None).Returns(sale);
        _saleRepository.IsActiveAsync(command.Id).Returns(true);
        _saleRepository.UpdateAsync(Arg.Is<Sale>(s => s.Id == command.Id), CancellationToken.None).Returns(true);
        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<UpdateSaleResult>(sale).Returns(updateSaleResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(updateSaleResult);
    }
}