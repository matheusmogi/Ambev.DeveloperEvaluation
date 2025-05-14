using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Repository for managing sales in the relational database
/// </summary>
internal class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleRepository"/> class.
    /// </summary>
    /// <param name="context"></param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Asynchronously creates a new sale record in the database.
    /// </summary>
    /// <param name="sale">The sale entity to be created.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation, if required.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created sale entity.</returns>
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        sale.SaleDate = DateTime.SpecifyKind(sale.SaleDate, DateTimeKind.Utc);
        sale.CreatedAt = DateTime.SpecifyKind(sale.CreatedAt, DateTimeKind.Utc);

        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return sale;
    }

    /// <summary>
    /// Retrieves a sale by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the sale.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the sale with the specified identifier or null if not found.</returns>
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a paginated and filtered list of sales, ordered by sale date.
    /// </summary>
    /// <param name="page">The page number to retrieve, default is 1.</param>
    /// <param name="size">The number of items per page, default is 10.</param>
    /// <param name="order">The ordering of the sales by sale date ('asc' or 'desc'), default is ascending.</param>
    /// <param name="filters">A dictionary of filters to apply to the sales query, where the key is the field name and the value is the filter criteria.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, with a result of enumerable sales matching the specified criteria.</returns>
    public async Task<IEnumerable<Sale>> GetAllAsync(int page = 1,
        int size = 10,
        string order = Constants.Ordering.Ascending,
        Dictionary<string, string>? filters = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Sale> salesQuery = _context.Sales.Include(s => s.Items);

        if (filters != null)
        {
            salesQuery = GetFiltered(salesQuery, filters);
        }

        salesQuery = order.Equals(Constants.Ordering.Descending,
            StringComparison.CurrentCultureIgnoreCase)
            ? salesQuery.OrderByDescending(s => s.SaleDate)
            : salesQuery.OrderBy(s => s.SaleDate);

        salesQuery = salesQuery.Skip((page - 1) * size).Take(size);

        return await salesQuery.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Updates an existing sale asynchronously in the database.
    /// </summary>
    /// <param name="sale">The sale entity to be updated.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>Returns true if the operation was successful; otherwise, false.</returns>
    public async Task<bool> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        sale.UpdatedAt = DateTime.UtcNow;
        _context.Attach(sale);
        _context.Entry(sale).State = EntityState.Modified;
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }


    /// <summary>
    /// Deletes softly a sale by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await _context.Sales
                       .Include(s => s.Items)
                       .FirstOrDefaultAsync(s => s.Id == id, cancellationToken) ??
                   throw new InvalidOperationException($"Sale with ID {id} not found");

        sale.Cancel();
        _context.Sales.Update(sale);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    /// <summary>
    /// Checks if a sale with the specified ID is active, meaning its status is not canceled.
    /// </summary>
    /// <param name="id">The unique identifier of the sale.</param>
    /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean indicating whether the sale is active.</returns>
    public async Task<bool> IsActiveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales.AnyAsync(s => s.Id == id && s.Status != SaleStatus.Cancelled, cancellationToken);
    }

    private static IQueryable<Sale> GetFiltered(IQueryable<Sale> query, Dictionary<string, string> filters)
    {
        foreach (var filter in filters)
        {
            switch (filter.Key)
            {
                case Constants.Filter.CustomerName:
                    if (!string.IsNullOrEmpty(filter.Value))
                    {
                        query = query.Where(s => s.CustomerName.Contains(filter.Value.Replace("*", "")));
                    }

                    break;
                case Constants.Filter.SaleDate:
                    if (DateTime.TryParse(filter.Value, out var saleDate))
                    {
                        var convertedDate = DateTime.SpecifyKind(saleDate, DateTimeKind.Utc);
                        query = query.Where(s => s.SaleDate.Date == convertedDate.Date);
                    }

                    break;
                case Constants.Filter.SaleDateStart:
                    if (DateTime.TryParse(filter.Value, out var saleDateStart))
                    {
                        var convertedDateStart = DateTime.SpecifyKind(saleDateStart, DateTimeKind.Utc);
                        query = query.Where(s => s.SaleDate.Date >= convertedDateStart.Date);
                    }

                    break;
                case Constants.Filter.SaleDateEnd:
                    if (DateTime.TryParse(filter.Value, out var saleDateEnd))
                    {
                        var convertedDateEnd = DateTime.SpecifyKind(saleDateEnd, DateTimeKind.Utc);
                        query = query.Where(s => s.SaleDate.Date <= convertedDateEnd.Date);
                    }

                    break;
                case Constants.Filter.BranchName:
                    if (!string.IsNullOrEmpty(filter.Value))
                    {
                        query = query.Where(s => s.BranchName.Contains(filter.Value.Replace("*", "")));
                    }

                    break;
                case Constants.Filter.IsCancelled:
                    query = query.Where(s => s.Status == SaleStatus.Cancelled);
                    break;
            }
        }

        return query;
    }
}